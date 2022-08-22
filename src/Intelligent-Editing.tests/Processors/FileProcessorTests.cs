using Xunit;
using Shouldly;
using Moq;
using System.IO;
using Microsoft.AspNetCore.Http;
using Intelligent_Editing.Configuration;
using Microsoft.Extensions.Options;
using Intelligent_Editing.Processors;
using Intelligent_Editing.Models;
using Intelligent_Editing.Parsers;
using Intelligent_Editing.Exceptions;

namespace Intelligent_Editing.tests.Processors
{
    public class FileProcessorTests
    {
        private Mock<IFormFile> _file;
        private Mock<IOptions<AnalysisSettings>> _settings;
        private Mock<ILanguageParser> _languageParser;

        [Fact]
        public void ProcessFile_EnglishLanguageOverMinWordCount_ReturnsSuccess()
        {
            // Assemble
            SetupSettings(new AnalysisSettings { MinWordCount = 8, LanguageThreshold = 5});
            SetupFile("The quick brown fox jumps over the lazy dog");
            SetupLanguage(true);
            var test = new FileProcessor(_settings.Object, _languageParser.Object);
            // Act
            var result = test.ProcessFile(_file.Object);
            // Assert
            result.ShouldBeOfType<ReportModel>();
            result.IsEnglishDocument.ShouldBeTrue();
            result.HasRequiredWordCount.ShouldBeTrue();
        }

        [Fact]
        public void ProcessFile_GermanLanguageOverMinWordCount_ReturnsFailure()
        {
            // Assemble
            SetupSettings(new AnalysisSettings { MinWordCount = 8, LanguageThreshold = 5 });
            SetupFile("Der Schnelle Braune Fuchs Springt Uber Den Faulen Hund");
            SetupLanguage(false);
            var test = new FileProcessor(_settings.Object, _languageParser.Object);
            // Act
            // Assert
            Should.Throw<InvalidLanguageException>(() => test.ProcessFile(_file.Object));
        }

        [Fact]
        public void ProcessFile_EnglishLanguageUnderMinWordCount_ReturnsFailure()
        {
            // Assemble
            SetupSettings(new AnalysisSettings { MinWordCount = 10, LanguageThreshold = 5 });
            SetupFile("The quick brown fox jumps over the lazy dog");
            SetupLanguage(true);
            var test = new FileProcessor(_settings.Object, _languageParser.Object);
            // Act
            // Assert
            Should.Throw<InvalidLengthException>(() => test.ProcessFile(_file.Object));
        }

        [Fact]
        public void ProcessFile_HasTwoSequences_ReturnsTwoSequences()
        {
            // Assemble
            SetupSettings(new AnalysisSettings { MinWordCount = 8, LanguageThreshold = 5 });
            SetupFile("The quick brown fox. The quick brown fox.");
            SetupLanguage(true);
            var test = new FileProcessor(_settings.Object, _languageParser.Object);
            // Act
            var result = test.ProcessFile(_file.Object);
            // Assert
            result.ShouldBeOfType<ReportModel>();
            result.IsEnglishDocument.ShouldBeTrue();
            result.HasRequiredWordCount.ShouldBeTrue();
            result.SequenceOccurances.ShouldBe(1);
        }

        [Fact]
        public void ProcessFile_ValidFileWithNonAplhaNumericCharacters_ReturnsSuccess()
        {
            // Assemble
            SetupSettings(new AnalysisSettings { MinWordCount = 10, LanguageThreshold = 5 });
            SetupFile("\"The food was off!\", exclaimed the Woman to the chef. \"It was dreadful\"");
            SetupLanguage(true);
            var test = new FileProcessor(_settings.Object, _languageParser.Object);
            // Act
            var result = test.ProcessFile(_file.Object);
            // Assert
            result.ShouldBeOfType<ReportModel>();
            result.IsEnglishDocument.ShouldBeTrue();
            result.HasRequiredWordCount.ShouldBeTrue();
        }

        private void SetupLanguage(bool isEnglish)
        {
            _languageParser = new Mock<ILanguageParser>();
            _languageParser
                .Setup(x => x.Parse(It.IsAny<string>()))
                .Returns(isEnglish);
        }

        private void SetupFile(string content)
        {
            _file = new Mock<IFormFile>();
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            _file.Setup(_ => _.OpenReadStream()).Returns(ms);
            _file.Setup(_ => _.FileName).Returns(fileName);
            _file.Setup(_ => _.Length).Returns(ms.Length);
        }

        private void SetupSettings(AnalysisSettings settings)
        {
            _settings = new Mock<IOptions<AnalysisSettings>>();
            _settings.Setup(x => x.Value).Returns(settings);

        }
    }
}
