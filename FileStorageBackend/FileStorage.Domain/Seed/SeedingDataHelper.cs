using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FileStorage.Domain.Seed
{
    /// <summary>
    /// Class for deserialize data for data seed from Json file.
    /// </summary>
    public static class SeedingDataHelper
    {
        public static List<T> SeedingDataFromJson<T>(string fileName)
        {
            List<T> data = new List<T>();

            string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(),
                                      "FileStorage.Data", 
                                      "Seeding", 
                                      fileName);

            if (File.Exists(filePath))
            {
                try
                {
                    string file = File.ReadAllText(filePath);
                    data = JsonSerializer.Deserialize<List<T>>(file);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
                throw new Exception("Files not found");

            return data;
        }
    }
}
