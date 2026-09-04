using System;
namespace CoffeeStoreApi.Services.Files
{
	public interface IFileService
	{
        Task<byte[]?> GetFileBytesAsync(IFormFile? file);
    }
}

