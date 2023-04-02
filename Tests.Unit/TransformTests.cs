namespace Tests.Unit
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Xunit;
    using Zagidziran.ConfigurationExtensions;
    using Zagidziran.ConfigurationExtensions.Exceptions;

    public class TransformTests
    {
        [Fact]
        public void ShouldTransform()
        {
            // Arrange
            var expected = new Dictionary<string, string>()
            {
                ["referncedData:value1"] = "1",
                ["referncedData2"] = "1",
                ["referncedData3"] = "Text1",
            };

            var confgiurationManger = new ConfigurationManager();
            confgiurationManger.AddYamlFile(@".\Configuration\SimpleMapping.yaml");

            // Act
            confgiurationManger.Transform();
            var configuration = confgiurationManger.AsEnumerable();

            // Assert
            configuration.Should().Contain(expected);
        }

        [Fact]
        public void ShouldThrowOnMissingRef()
        {
            // Arrange
            var confgiurationManger = new ConfigurationManager();
            confgiurationManger.AddYamlFile(@".\Configuration\BrokenMapping.yaml");

            // Act
            var action = confgiurationManger.Transform;

            // Assert
            action.Should().Throw<ReferencedKeyNotFoundExcepion>()
                .Which.ReferencedKey.Should().Be("someMissingData:data");
        }
    }
}