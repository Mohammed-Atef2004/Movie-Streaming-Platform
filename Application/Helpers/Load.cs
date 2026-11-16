using Microsoft.AspNetCore.Http;
using System.Security;
using System.Text.RegularExpressions;

namespace BLL.Helpers
{
    /// <summary>
    /// Hardened file storage helpers: validates content type/extension, enforces a size
    /// limit, generates collision-free random file names, and never lets the caller
    /// treat an exception message as a file name.
    /// </summary>
    public static class Load
    {
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp"
        };

        private static readonly HashSet<string> AllowedVideoExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".webm", ".mkv", ".mov"
        };

        private const long MaxImageSizeBytes = 5 * 1024 * 1024;   // 5 MB
        private const long MaxVideoSizeBytes = 512 * 1024 * 1024; // 512 MB

        /// <exception cref="InvalidDataException">When the file is missing, too large or of a disallowed type.</exception>
        public static string UploadImage(IFormFile? file, string folderName = "Images")
        {
            return UploadFile(file, folderName, AllowedImageExtensions, MaxImageSizeBytes);
        }

        /// <exception cref="InvalidDataException">When the file is missing, too large or of a disallowed type.</exception>
        public static string UploadVideo(IFormFile? file, string folderName = "Videos")
        {
            return UploadFile(file, folderName, AllowedVideoExtensions, MaxVideoSizeBytes);
        }

        private static string UploadFile(IFormFile? file, string folderName, HashSet<string> allowedExtensions, long maxSizeBytes)
        {
            if (file == null || file.Length == 0)
                throw new InvalidDataException("No file was provided.");

            if (file.Length > maxSizeBytes)
                throw new InvalidDataException($"File is too large. Maximum allowed size is {maxSizeBytes / (1024 * 1024)} MB.");

            // Never trust the client's path - only its (sanitized) extension matters.
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(extension) || !allowedExtensions.Contains(extension))
                throw new InvalidDataException($"File type '{extension}' is not allowed. Allowed types: {string.Join(", ", allowedExtensions)}.");

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            Directory.CreateDirectory(folderPath);

            // Random name: collision-free and strips any client-supplied path tricks.
            var fileName = $"{Guid.NewGuid()}{extension}";
            var finalPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        public static bool RemoveFile(string folderName, string? fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                    return false;

                // Path traversal guard: strip any directory components the caller may pass.
                var safeName = Path.GetFileName(fileName);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, safeName);

                // Defense in depth: the resolved path must stay inside wwwroot.
                var rootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName));
                var resolvedPath = Path.GetFullPath(fullPath);
                if (!resolvedPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
                    return false;

                if (File.Exists(resolvedPath))
                {
                    File.Delete(resolvedPath);
                    return true;
                }

                return false;
            }
            catch
            {
                // Deleting a leftover file must never crash the business operation.
                return false;
            }
        }

        /// <summary>Kept for backwards compatibility with older call sites.</summary>
        public static string UploadFile(string folderName, IFormFile file) => UploadImage(file, folderName);
    }
}
