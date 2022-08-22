using Intelligent_Editing.Models;
using Microsoft.AspNetCore.Http;
using Intelligent_Editing.Processors;

namespace Intelligent_Editing.Facades
{
    public class UploadFacade : IUploadFacade
    {
        private IFileProcessor _fileProcessor;
        public UploadFacade( IFileProcessor fileProcessor )
        {
            _fileProcessor = fileProcessor;
        }

        public ReportModel ProcessFile(IFormFile file)
        {
            return _fileProcessor.ProcessFile(file);
        }
    }
}
