using Intelligent_Editing.Configuration;
using Intelligent_Editing.Models;
using Intelligent_Editing.Parsers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Intelligent_Editing.Exceptions;

namespace Intelligent_Editing.Processors
{
    public class FileProcessor : IFileProcessor
    {
        private AnalysisSettings _analysisSettings;
        private ILanguageParser _languageParser;

        public FileProcessor(
            IOptions<AnalysisSettings> analysisSettings,
            ILanguageParser languageParser
            )
        {
            _analysisSettings = analysisSettings.Value;
            _languageParser = languageParser;
        }

        public ReportModel ProcessFile( IFormFile file )
        {
            bool isValidLanguage = false;
            bool hasRequiredWordCount = false;
            List<string> index = new List<string>();

            if (file.Length > 0)
            {
                using (var rs = file.OpenReadStream())
                {
                    using(var sr = new StreamReader(rs))
                    {                  
                        
                        StringBuilder word = new StringBuilder();
                        int i = 0;
                        while((i = sr.Read()) != -1)
                        {
                            char character = Convert.ToChar(i);
                            if(character != ' ')
                            {
                                word.Append(character);
                            }
                            else
                            {
                                isValidLanguage = isValidLanguage ? isValidLanguage : _languageParser.Parse(Sanitise(word.ToString()));

                                if (!isValidLanguage && (index.Count >= _analysisSettings.LanguageThreshold))
                                {
                                    throw new InvalidLanguageException("Language requirements have not been met.");
                                }

                                index.Add(word.ToString());
                                hasRequiredWordCount = index.Count >= _analysisSettings.MinWordCount;
                                word.Clear();
                            }
                        }

                        if(index.Count < _analysisSettings.MinWordCount)
                        {
                            throw new InvalidLengthException(
                                $"The document's word count of {index.Count} does not meet the expected {_analysisSettings.MinWordCount} words.");
                        }
                    }
                }
            }

            return new ReportModel
            {
                IsEnglishDocument = isValidLanguage,
                HasRequiredWordCount = hasRequiredWordCount,
                WordCount = index.Count,
                SequenceOccurances = 0
            };
        }

        private string Sanitise( string word)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(word, "");
        }
    }
}
