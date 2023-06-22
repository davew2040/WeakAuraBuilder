using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Models;

namespace WeakAuraManager
{
    internal class WeakAuraManagerRunner : IWeakAuraManagerRunner
    {
        private readonly IConfiguration _configuration;
        private readonly IFileGenerator _fileGenerator;
        private readonly IParser _parser;
        private readonly IFileBackerUpper _fileBackerUpper;

        public WeakAuraManagerRunner(IConfiguration config, IFileGenerator fileGenerator, IParser parser, IFileBackerUpper fileBackerUpper)
        {
            _configuration = config;
            _fileGenerator = fileGenerator;
            _parser = parser;
            _fileBackerUpper = fileBackerUpper;
        }

        public async Task Run()
        {
            await _fileBackerUpper.Backup();

            var spells = await _parser.Parse();

            var configPath = _configuration.GetValue<string>(ConfigKeys.WeakAurasConfigPath);

            var fileContents = await _fileGenerator.GenerateFileContents(File.ReadAllText(configPath), spells);

            File.Delete(configPath);

            await File.WriteAllTextAsync(configPath, fileContents);
        }
    }
}
