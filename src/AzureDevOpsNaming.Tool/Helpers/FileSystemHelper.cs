﻿using AzureNaming.Tool.Models;
using AzureNaming.Tool.Services;
using System.Text.Json;

namespace AzureNaming.Tool.Helpers
{
    public class FileSystemHelper
    {
        private static IAdminLogService _adminLogService;

        public FileSystemHelper(IAdminLogService adminLogService)
        {
            _adminLogService = adminLogService;
        }

        public static async Task<string> ReadFile(string fileName, string folderName = "settings/")
        {
            await CheckFile(folderName + fileName);
            string data = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName + fileName));
            return data;
        }

        public static async Task WriteFile(string fileName, string content, string folderName = "settings/")
        {
            await CheckFile(folderName + fileName);
            int retries = 0;
            while (retries < 10)
            {
                try
                {
                    using FileStream fstr = File.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName + fileName), FileMode.Truncate, FileAccess.Write);
                    StreamWriter sw = new(fstr);
                    sw.Write(content);
                    sw.Flush();
                    sw.Dispose();
                    return;
                }
                catch (Exception)
                {
                    Thread.Sleep(50);
                    retries++;
                }
            }
        }

        public static async Task CheckFile(string fileName)
        {
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
            {
                var file = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                file.Close();

                for (int numTries = 0; numTries < 10; numTries++)
                {
                    try
                    {
                        await File.WriteAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName), "[]");
                        return;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        public static async Task<object> WriteConfiguation(object configdata, string configFileName)
        {
            try
            {

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                await FileSystemHelper.WriteFile(configFileName, JsonSerializer.Serialize(configdata, options));
                return "Config updated.";
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return ex;
            }
        }

        public static bool ResetConfiguration(string filename)
        {
            bool result = false;
            try
            {
                // Get all the files in the repository folder
                DirectoryInfo dirRepository = new("repository");
                foreach (FileInfo file in dirRepository.GetFiles())
                {
                    if (file.Name == filename)
                    {
                        // Copy the repository file to the settings folder
                        file.CopyTo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings/" + file.Name), true);
                        result = true;
                        // Clear any cached data

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _adminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
            }
            return result;
        }

        public static async Task MigrateDataToFile(string sourcefileName, string sourcefolderName, string destinationfilename, string destinationfolderName, bool delete)
        {
            // Get the source data
            string data = await ReadFile(sourcefileName, sourcefolderName);

            // Write the destination data
            await WriteFile(destinationfilename, data, destinationfolderName);

            // Check if the source file should be removed (In repository and settings folders)
            if (delete)
            {
                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "repository/" + sourcefileName));
                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings/" + sourcefileName));
            }
        }
    }
}
