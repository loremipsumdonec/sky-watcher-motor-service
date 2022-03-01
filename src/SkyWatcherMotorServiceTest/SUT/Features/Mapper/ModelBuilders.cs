using Boilerplate.Features.Core.Services;
using Boilerplate.Features.Mapper.Attributes;
using Boilerplate.Features.Mapper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SkyWatcherMotorServiceTest.SUT.Features.Mapper
{
    [Trait("type", "Unit")]
    public class BuilderAttributes
    {
        public BuilderAttributes()
        {
            Attributes = new List<BuilderForAttribute>();
        }

        public List<BuilderForAttribute> Attributes { get; set; }

        public BuilderForAttribute Add(
            Type modelType,
            Type whenType,
            Type builderType,
            Type dependency = null,
            BuildSequencePosition positionInBuildSequence = BuildSequencePosition.Unknown)
        {
            var attribute = new BuilderForAttribute(modelType, whenType)
            {
                BuilderType = builderType,
                Dependency = dependency,
                PositionInBuildSequence = positionInBuildSequence
            };

            Attributes.Add(attribute);

            return attribute;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [Trait("severity", "Critical")]
        public void WhenBuilderForBelongsInClassHierarchy_BuilderForHasTwoBuilderTypes(bool reverse)
        {
            Add(typeof(BaseClass), typeof(BaseClassModel), typeof(BaseClassBuilder));
            var attribute = Add(typeof(AlphaClass), typeof(AlphaClassModel), typeof(AlphaClassBuilder));

            if (reverse)
            {
                Attributes.Reverse();
            }

            PrepareBuilderAttributes prepareBuilderAttributes = new PrepareBuilderAttributes();
            prepareBuilderAttributes.Prepare(Attributes);

            Assert.Equal(2, attribute.BuilderTypes.Count);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [Trait("severity", "Critical")]
        public void OrderIsBasedOnModelClassHierarchy(bool reverse)
        {
            Add(typeof(BaseClass), typeof(BaseClassModel), typeof(BaseClassBuilder));
            Add(typeof(LoremClass), typeof(LoremClassModel), typeof(LoremClassBuilder));

            var attribute = Add(typeof(AlphaClass), typeof(AlphaClassModel), typeof(AlphaClassBuilder));

            if (reverse)
            {
                Attributes.Reverse();
            }

            PrepareBuilderAttributes prepareBuilderAttributes = new PrepareBuilderAttributes();
            prepareBuilderAttributes.Prepare(Attributes);

            Assert.Equal(attribute, Attributes[0]);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public void WithBuildSequencePositionLast_BuilderTypeIsLast()
        {
            Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(OmegaClassBuilder),
                positionInBuildSequence: BuildSequencePosition.Last
            );
            Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(BetaClassBuilder));
            Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(AlphaClassBuilder));

            PrepareBuilderAttributes prepareBuilderAttributes = new PrepareBuilderAttributes();
            prepareBuilderAttributes.Prepare(Attributes);

            var builderTypes = Attributes[0].BuilderTypes;
            Assert.Equal(typeof(OmegaClassBuilder), builderTypes.Last());
        }

        [Fact]
        [Trait("severity", "Critical")]
        public void WithBuildSequencePositionFirst_BuilderTypeIsFirst()
        {
            Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(BetaClassBuilder));
            Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(AlphaClassBuilder));
            Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(OmegaClassBuilder),
                positionInBuildSequence: BuildSequencePosition.First
            );

            PrepareBuilderAttributes prepareBuilderAttributes = new PrepareBuilderAttributes();
            prepareBuilderAttributes.Prepare(Attributes);

            var builderTypes = Attributes[0].BuilderTypes;
            Assert.Equal(typeof(OmegaClassBuilder), builderTypes[0]);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public void WithBuildSequencePositionBefore_BuilderTypeIsBeforeDepenecy()
        {
            Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(BetaClassBuilder));
            var before = Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(OmegaClassBuilder),
                positionInBuildSequence: BuildSequencePosition.Before,
                dependency: typeof(AlphaClassBuilder)
            );
            var dependency = Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(AlphaClassBuilder));

            PrepareBuilderAttributes prepareBuilderAttributes = new PrepareBuilderAttributes();
            prepareBuilderAttributes.Prepare(Attributes);

            var builderTypes = Attributes[0].BuilderTypes;
            var indexOfBefore = builderTypes.IndexOf(before.BuilderType);
            var indexOfBeforeDependency = builderTypes.IndexOf(dependency.BuilderType);

            Assert.True(indexOfBefore < indexOfBeforeDependency);
        }

        [Fact]
        [Trait("severity", "Critical")]
        public void WithBuildSequencePositionAfter_BuilderTypeIsAfterDepenecy()
        {
            Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(BetaClassBuilder));
            var dependency = Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(AlphaClassBuilder));

            var after = Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(OmegaClassBuilder),
                positionInBuildSequence: BuildSequencePosition.After,
                dependency: typeof(AlphaClassBuilder)
            );

            PrepareBuilderAttributes prepareBuilderAttributes = new PrepareBuilderAttributes();
            prepareBuilderAttributes.Prepare(Attributes);

            var builderTypes = Attributes[0].BuilderTypes;
            var indexOfAfter = builderTypes.IndexOf(after.BuilderType);
            var indexOfBeforeDependency = builderTypes.IndexOf(dependency.BuilderType);

            Assert.True(indexOfAfter > indexOfBeforeDependency);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [Trait("severity", "Critical")]
        public void WithSameModel_BuilderWitMoreBuildersFirst(bool reverse)
        {
            var attribute = Add(typeof(AlphaClassModel), typeof(AlphaClass), typeof(AlphaClassBuilder));
            Add(typeof(AlphaClassModel), typeof(IHasCoolFunction), typeof(HasCoolFunctionBuilder));

            if (reverse)
            {
                Attributes.Reverse();
            }

            PrepareBuilderAttributes prepareBuilderAttributes = new PrepareBuilderAttributes();
            prepareBuilderAttributes.Prepare(Attributes);

            Assert.Equal(attribute, Attributes[0]);
            Assert.True(attribute.BuilderTypes.Count > Attributes[1].BuilderTypes.Count);
        }

        private abstract class BaseClass
        {
        }

        private class BaseClassBuilder
        {
        }

        private class BaseClassModel
        {
        }

        private class AlphaClass
            : BaseClass, IHasCoolFunction
        {
        }

        private class AlphaClassModel
            : BaseClassModel
        {
        }

        private class AlphaClassBuilder
        {
        }


        private interface IHasCoolFunction
        {
        }

        private class HasCoolFunctionBuilder 
        { 
        }

        private class BetaClass
        {
        }

        private class BetaClassBuilder
        {
        }

        private class OmegaClass
        {
        }

        private class OmegaClassBuilder
        {
        }

        private class LoremClass
        {
        }

        private class LoremClassModel
        {
        }

        private class LoremClassBuilder
        {
        }

    }
}
