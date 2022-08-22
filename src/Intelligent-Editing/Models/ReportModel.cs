using System;

namespace Intelligent_Editing.Models
{
    public class ReportModel
    {
        public bool IsEnglishDocument { get; set; }
        public bool HasRequiredWordCount { get; set; }
        public Int64 WordCount { get; set; }
        public Int64 SequenceOccurances { get ; set; }
    }
}
