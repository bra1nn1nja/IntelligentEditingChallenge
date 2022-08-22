using Xunit;
using Shouldly;
using Moq;
using Intelligent_Editing.Parsers;

namespace Intelligent_Editing.tests.Parsers
{
    public class EnglishParserTest
    {
        [Fact]
        public void Parse_EnglishArticle_ReturnsTrue()
        {
            // Assemble
            var test = new EnglishParser();
            // Act
            var result = test.Parse("The");
            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Parse_EnglishArticleUpperCase_ReturnsTrue()
        {
            // Assemble
            var test = new EnglishParser();
            // Act
            var result = test.Parse("THE");
            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Parse_EnglishPronoun_ReturnsTrue()
        {
            // Assemble
            var test = new EnglishParser();
            // Act
            var result = test.Parse("She");
            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Parse_GermanArticle_ReturnsFalse()
        {
            // Assemble
            var test = new EnglishParser();
            // Act
            var result = test.Parse("Der");
            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Parse_GermanPronoun_ReturnsFalse()
        {
            // Assemble
            var test = new EnglishParser();
            // Act
            var result = test.Parse("sie");
            // Assert
            result.ShouldBeFalse();
        }
    }
}
