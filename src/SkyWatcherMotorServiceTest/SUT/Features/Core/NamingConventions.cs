using Boilerplate.Features.Core.NamingConventions;
using System;
using Xunit;

namespace SkyWatcherMotorServiceTest.SUT.Features.Core
{
    [Trait("type", "Unit")]
    public class NamingConventions
    {
        private INamingConvention Create(Type type) 
        {
            return (INamingConvention)Activator.CreateInstance(type);
        }

        [Theory]
        [InlineData(typeof(SnakeCase), "LoremDonec", "lorem_donec")]
        [InlineData(typeof(SnakeCase), "Lorem Donec", "lorem_donec")]
        [InlineData(typeof(SnakeCase), "Lorem.Donec", "lorem_donec")]
        [InlineData(typeof(SnakeCase), "Lorem_Donec", "lorem_donec")]
        [InlineData(typeof(KebabCase), "LoremDonec", "lorem-donec")]
        [InlineData(typeof(KebabCase), "Lorem Donec", "lorem-donec")]
        [InlineData(typeof(KebabCase), "Lorem.Donec", "lorem-donec")]
        [InlineData(typeof(KebabCase), "Lorem_Donec", "lorem-donec")]
        [InlineData(typeof(CamelCase), "LoremDonec", "loremDonec")]
        [InlineData(typeof(CamelCase), "Lorem Donec", "loremDonec")]
        [InlineData(typeof(CamelCase), "Lorem.Donec", "loremDonec")]
        [InlineData(typeof(CamelCase), "Lorem_Donec", "loremDonec")]
        [Trait("severity", "Critical")]
        public void DelimiterSeparated_To_HasExpected(Type type, string input, string expected) 
        {
            INamingConvention namingConvention = Create(type);
            string actual = namingConvention.To(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(SnakeCase), "lorem_donec", "LoremDonec")]
        [InlineData(typeof(KebabCase), "lorem-donec", "LoremDonec")]
        [InlineData(typeof(DotNotation), "lorem.donec", "LoremDonec")]
        [Trait("severity", "Critical")]
        public void DelimiterSeparated_From_HasExpected(Type type, string input, string expected)
        {
            INamingConvention namingConvention = Create(type);
            string actual = namingConvention.From(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(typeof(SnakeCase), "lorem_donec", typeof(DotNotation), "lorem.donec")]
        [InlineData(typeof(DotNotation), "Lorem.donec", typeof(SnakeCase), "lorem_donec")]
        [InlineData(typeof(DotNotation), "Lorem.donec", typeof(CamelCase), "loremDonec")]
        [InlineData(typeof(DotNotation), "LoremDonec.ipsum", typeof(CamelCase), "loremDonecIpsum")]
        [Trait("severity", "Critical")]
        public void DelimiterSeparated_FromWithTo_HasExpected(Type type, string input, Type to, string expected)
        {
            INamingConvention from = Create(type);
            string actual = from.From(input, Create(to));

            Assert.Equal(expected, actual);
        }
    }
}
