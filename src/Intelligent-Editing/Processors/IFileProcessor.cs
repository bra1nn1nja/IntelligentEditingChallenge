using Intelligent_Editing.Models;
using Microsoft.AspNetCore.Http;

namespace Intelligent_Editing.Processors
{
    public interface IFileProcessor
    {
        ReportModel ProcessFile(IFormFile file);
    }
}
