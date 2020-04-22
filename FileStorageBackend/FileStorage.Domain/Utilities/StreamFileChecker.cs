using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace FileStorage.Domain.Utilities
{
    public static class StreamFileChecker
    {
        public static async Task<byte[]> ProcessStreamedFile(
            MultipartSection section,
            //ContentDispositionHeaderValue contentDisposition,
            /*ModelStateDictionary modelState,*/
            /*string[] permittedExtensions,*/
            long sizeLimit)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await section.Body.CopyToAsync(memoryStream);

                    if (memoryStream.Length == 0)
                    {
                        throw new InvalidDataException("The file is empty.");
                    }
                    else if (memoryStream.Length > sizeLimit)
                    {
                        var megabyteSizeLimit = sizeLimit / 1048576;
                        throw new InvalidDataException($"The file exceeds {megabyteSizeLimit:N1} MB.");
                    }
                    else
                    {
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
