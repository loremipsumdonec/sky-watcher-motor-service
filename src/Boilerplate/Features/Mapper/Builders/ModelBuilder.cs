using Boilerplate.Features.Core;

namespace Boilerplate.Features.Mapper.Services
{
    public abstract class ModelBuilder<T, M>
        : IModelBuilder
    {
        public bool CanBuild(object content, IModel model)
        {
            return CanBuild((T)content, (M)model);
        }

        public virtual bool CanBuild(T content, M model)
        {
            return true;
        }

        public virtual Task BuildAsync(object content, IModel model)
        {
            Validate(content, model);
            return BuildAsync((T)content, (M)model);
        }

        public abstract Task BuildAsync(T content, M model);

        protected void Validate(object content, IModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException($"model is null");
            }

            if (content != null)
            {
                if (!(content is T))
                {
                    throw new ArgumentException($"Expected content as {typeof(T)} but got {content.GetType()}");
                }

                if (!(model is M))
                {
                    throw new ArgumentException($"Expected model as {typeof(M)} but got {model.GetType()}");
                }
            }
        }
    }
}
