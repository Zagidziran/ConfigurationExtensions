namespace Tests.Unit.CodeParser
{
    using AutoFixture.Xunit2;
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Reflection;
    using Xunit;
    using Zagidziran.ConfigurationExtensions.Exceptions;
    using Zagidziran.ConfigurationExtensions.Transformations.Code;

    public class CodeParserTests
    {
        [Theory]
        [AutoData]
        public void ShouldTransormToString(int value)
        {
            // Arrange
            var expected = value.ToString();
            var transformer = CodeParser.Parse($"return \"{value}\";");

            // Act
            var result = transformer.Transform(new Dictionary<string, string>());

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [AutoData]
        public void ShouldTransormToDictionary(string key, string value)
        {
            // Arrange
            var expected = new Dictionary<string, string> { [key] = value };
            var transformer = CodeParser.Parse($"return new Dictionary<string, string> {{ [\"{key}\"] = \"{value}\" }};");

            // Act
            var result = transformer.Transform(new Dictionary<string, string>());

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldTransormToNull()
        {
            // Arrange
            var transformer = CodeParser.Parse("return null;");

            // Act
            var result = transformer.Transform(new Dictionary<string, string>());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ShouldAccessReferencedType()
        {
            // Arrange
            var expected = TestClass.Name;
            var code = @"
                using Tests.Unit.CodeParser;

                return TestClass.Name;";

            var transformer = CodeParser.Parse(code, Assembly.GetExecutingAssembly());


            // Act
            var result = transformer.Transform(new Dictionary<string, string>());

            // Assert
            result.Should().Be(expected);
        }
    }
}
