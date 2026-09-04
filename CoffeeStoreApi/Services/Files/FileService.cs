using System;

namespace CoffeeStoreApi.Services.Files
{
    public class FileService : IFileService
    {
        private readonly List<string> _allowedExtensions =
            new() { ".jpeg", ".jpg", ".png" };

        private const long MaxAllowedFileSize = 1 * 1024 * 1024;

        public async Task<byte[]?> GetFileBytesAsync(IFormFile? file)
        {
            if (file == null)
                return null;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid image extension.");

            if (file.Length > MaxAllowedFileSize)
                throw new ArgumentException("Image size exceeds 1MB.");

            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }
    }
}

