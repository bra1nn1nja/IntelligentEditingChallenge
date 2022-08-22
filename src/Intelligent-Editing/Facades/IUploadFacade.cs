using Intelligent_Editing.Models;
using Microsoft.AspNetCore.Http;

namespace Intelligent_Editing.Facades
{
    public interface IUploadFacade
    {
        ReportModel ProcessFile(IFormFile file);
    }
}
