using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FileStorage.Data.Models;
using FileStorage.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace FileStorage.Domain.Utilities
{
    /// <summary>
    /// Class with extension that allows FormFile processing
    /// </summary>
    public static class FormFileExtensions
    {
        public static async Task<byte[]> ProcessFormFileAsync(this IFormFile formFile,
                                                              long sizeLimit)
        {
            var nameForDisplay = WebUtility.HtmlEncode(formFile.FileName);

            if (formFile.Length == 0)
                throw new StorageItemEmptyException($"File: {nameForDisplay} is empty from FormFile.");

            if (formFile.Length > sizeLimit)
                throw new StorageItemExceedLimitException(
                    $"File: {nameForDisplay} exceeds {sizeLimit / 1048576:N1} MB.");

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length == 0)
                        throw new StorageItemEmptyException($"File: {nameForDisplay} is empty from MemoryStream.");
                    else
                        return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static StorageItem CreateFileItemFormFile(this IFormFile formFile,
                                                 StorageItem primaryFolder,
                                                 User user)
        {
            var fileItem = new StorageItem
            {
                TrustedName = StorageItemsHelpers.GetStorageItemTrustedName(),
                DisplayName = WebUtility.HtmlEncode(formFile.FileName),
                Extension = Path.GetExtension(formFile.FileName),
                CreatedOn = DateTime.Now,
                IsFolder = false,
                IsPrimaryFolder = false,
                IsRecycled = false,
                IsPublic = false,
                Size = formFile.Length,
                UserId = user.Id,
                ParentFolder = primaryFolder
            };
            fileItem.RelativePath = StorageItemsHelpers.GetFileRelativePath(fileItem);

            return fileItem;
        }
    }
}
