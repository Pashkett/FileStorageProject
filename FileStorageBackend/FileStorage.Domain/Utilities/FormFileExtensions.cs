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
    /// 
    /// </summary>
    public static class FormFileExtensions
    {
        public static async Task<byte[]> ProcessFormFileAsync(this IFormFile formFile,
                                                              long sizeLimit)
        {
            var nameForDisplay = WebUtility.HtmlEncode(formFile.FileName);

            if (formFile.Length == 0)
                throw new StorageItemEmptyException($"File: {nameForDisplay} is empty.");

            if (formFile.Length > sizeLimit)
                throw new StorageItemExceedLimitException($"File: {nameForDisplay} exceeds {sizeLimit / 1048576:N1} MB.");

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await formFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length == 0)
                        throw new StorageItemEmptyException($"File: {nameForDisplay} is empty.");
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
                CreatedOn = DateTime.Now,
                IsFolder = false,
                IsPrimaryFolder = false,
                IsRecycled = false,
                IsPublic = false,
                Size = formFile.Length,
                UserId = user.Id,
                ParentFolder = primaryFolder
            };

            fileItem.DisplayName = WebUtility.HtmlEncode(formFile.FileName);
            fileItem.TrustedName = StorageItemsHelpers.GetStorageItemTrustedName(fileItem);
            fileItem.Extension = Path.GetExtension(formFile.FileName);
            fileItem.RelativePath = StorageItemsHelpers.GetStorageItemRelativePath(fileItem);

            return fileItem;
        }
    }
}
