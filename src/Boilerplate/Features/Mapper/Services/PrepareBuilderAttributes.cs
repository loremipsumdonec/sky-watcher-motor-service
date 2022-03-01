using Boilerplate.Features.Mapper.Attributes;

namespace Boilerplate.Features.Mapper.Services
{
    public class PrepareBuilderAttributes
        : IPrepareBuilderAttributes
    {
        public void Prepare(List<BuilderForAttribute> attributes)
        {
            Build(attributes);
            Sort(attributes);
            Clean(attributes);
        }

        private void Build(List<BuilderForAttribute> pool)
        {
            foreach (var attribute in pool)
            {
                var types = pool
                    .Where(a =>
                        a.ModelType.IsAssignableFrom(attribute.ModelType) &&
                        a.WhenType.IsAssignableFrom(attribute.WhenType) &&
                        a != attribute
                    )
                    .ToList();

                types.Insert(0, attribute);

                SortWithClassHierarchy(types);
                SortWithFixedBuildSequence(types);
                SortWithRelativeBuildSequence(types);

                attribute.BuilderTypes = types.Select(t => t.BuilderType)
                    .Distinct()
                    .ToList();
            }
        }

        private static void SortWithClassHierarchy(List<BuilderForAttribute> pool)
        {
            pool.Sort((a, b) =>
                    a.ModelType.IsSubclassOf(b.ModelType) ? 0 : 1
                );
        }

        private void Sort(List<BuilderForAttribute> pool)
        {
            List<BuilderForAttribute> visited = new List<BuilderForAttribute>();

            foreach (var attribute in pool)
            {
                if (!visited.Contains(attribute))
                {
                    var hits = pool.Where(a =>
                                    a.ModelType.IsAssignableFrom(attribute.ModelType) ||
                                    attribute.ModelType.IsAssignableFrom(a.ModelType)
                                ).ToList();

                    hits.Sort(PrioritizeWithModelClassHierarchy);
                    hits.Sort(PrioritizeWithWhenTypeClassHierarchy);
                    hits.Sort(PrioritizeBuildersWithMoreBuilders);

                    visited.AddRange(hits);
                }
            }

            pool.Clear();
            pool.AddRange(visited);
        }

        private static int PrioritizeWithModelClassHierarchy(BuilderForAttribute a, BuilderForAttribute b)
        {
            return a.ModelType.IsSubclassOf(b.ModelType) ? -1 : 1;
        }

        private static int PrioritizeWithWhenTypeClassHierarchy(BuilderForAttribute a, BuilderForAttribute b)
        {
            if (a.ModelType.IsInterface && a.ModelType.Equals(b.ModelType))
            {
                if (a.WhenType.IsAssignableFrom(b.WhenType))
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            return 0;
        }

        private static int PrioritizeBuildersWithMoreBuilders(BuilderForAttribute a, BuilderForAttribute b)
        {
            if (a.ModelType.Equals(b.ModelType))
            {
                if (a.BuilderTypes.Count < b.BuilderTypes.Count)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            return 0;
        }

        private static void SortWithFixedBuildSequence(List<BuilderForAttribute> pool)
        {
            List<BuilderForAttribute> visited = new List<BuilderForAttribute>();
            Queue<BuilderForAttribute> first = new Queue<BuilderForAttribute>();
            Queue<BuilderForAttribute> last = new Queue<BuilderForAttribute>();

            foreach (var attribute in pool)
            {
                switch (attribute.PositionInBuildSequence)
                {
                    case BuildSequencePosition.First:
                        first.Enqueue(attribute);
                        break;
                    case BuildSequencePosition.Last:
                        last.Enqueue(attribute);
                        break;
                    default:
                        visited.Add(attribute);
                        break;
                }
            }

            visited.InsertRange(0, first);
            visited.AddRange(last);

            pool.Clear();
            pool.AddRange(visited);
        }

        private void SortWithRelativeBuildSequence(List<BuilderForAttribute> pool)
        {
            List<BuilderForAttribute> visited = new List<BuilderForAttribute>();

            foreach (var attribute in pool)
            {
                var dependencies = pool.FindAll(a => a.IsDependentOf(attribute));

                if (dependencies.Count > 0)
                {
                    var ordered = SortWithDependency(attribute, pool);

                    foreach (var a in ordered)
                    {
                        visited.Add(a);
                    }
                }
                else if (attribute.Dependency == null)
                {
                    visited.Add(attribute);
                }
            }

            pool.Clear();
            pool.AddRange(visited);
        }

        private static List<BuilderForAttribute> SortWithDependency(
            BuilderForAttribute root, List<BuilderForAttribute> pool)
        {
            List<BuilderForAttribute> visited = new List<BuilderForAttribute>();
            Queue<BuilderForAttribute> queue = new Queue<BuilderForAttribute>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var attribute = queue.Dequeue();

                if (attribute.Dependency != null)
                {
                    var dependency = pool.Find(d => attribute.IsDependentOf(d));
                    int indexOf = visited.IndexOf(dependency);

                    switch (attribute.PositionInBuildSequence)
                    {
                        case BuildSequencePosition.Before:
                            visited.Insert(indexOf, attribute);
                            break;
                        case BuildSequencePosition.After:
                        case BuildSequencePosition.Unknown:
                            visited.Add(attribute);
                            break;
                    }
                }
                else
                {
                    visited.Add(attribute);
                }

                var dependencies = pool.FindAll(a => a.IsDependentOf(attribute));
                dependencies.ForEach(d => queue.Enqueue(d));
            }

            return visited;
        }

        private void Clean(List<BuilderForAttribute> pool)
        {
            List<BuilderForAttribute> cleaned = new List<BuilderForAttribute>();

            foreach (var attribute in pool)
            {
                var exists = cleaned.Find(a =>
                    a.ModelType.Equals(attribute.ModelType)
                    && a.WhenType.Equals(attribute.WhenType)
                );

                if (exists == null)
                {
                    cleaned.Add(attribute);
                }
            }

            pool.Clear();
            pool.AddRange(cleaned);
        }
    }
}
