using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakAuraManager
{
    internal class FileBackerUpper : IFileBackerUpper
    {
        private readonly IConfiguration _configuration;

        public FileBackerUpper(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task Backup()
        {
            var configPath = _configuration.GetValue<string>(ConfigKeys.WeakAurasConfigPath);
            var backupPath = _configuration.GetValue<string>(ConfigKeys.BackupPath);

            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            var backupFilename = Path.Combine(backupPath, $"WeakAuras.lua.{Utilities.FormatFilenameDate(DateTime.Now)}.bak");

            File.Copy(configPath, backupFilename);

            Console.WriteLine($"Backed up '{configPath}' to '{backupFilename}'.");
        }
    }
}
