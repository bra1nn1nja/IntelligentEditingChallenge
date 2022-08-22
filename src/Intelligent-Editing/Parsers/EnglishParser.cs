using System.Collections.Generic;

namespace Intelligent_Editing.Parsers
{
    public class EnglishParser : ILanguageParser
    {
        private readonly HashSet<string> Articles = new HashSet<string>(
            new[] 
            { "a", "an", "the" });

        private readonly HashSet<string> Pronouns = new HashSet<string>(
            new[]
            {
                "i", "me", "mine", "myself", 
                "you", "your", "yours", "yourself", 
                "he", "him", "his", "himself",
                "she", "her", "hers", "herself",
                "it", "its", "itself",
                "we", "us", "our", "ours", "ourselves",
                "they", "them", "their", "theirs", "themselves" 
            });

        public bool Parse(string word)
        {
            return IsEnglishArticle(word) || IsEnglishPronoun(word);
        }

        private bool IsEnglishArticle(string word)
        {

            return Pronouns.Contains(word.ToLower());
        }

        private bool IsEnglishPronoun(string word)
        {
            return Articles.Contains(word.ToLower());
        }
    }
}
