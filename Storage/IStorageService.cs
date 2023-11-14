using Microsoft.AspNetCore.Components.Forms;

namespace Agape.Storage
{
    public interface IStorageService
    {
        Task<string> SaveAsync(Stream data, string objectName,  string contentType);

        Task<string> SaveInputFileAsync(IBrowserFile browserFile);

        Task EnsureCreated();
    }
}