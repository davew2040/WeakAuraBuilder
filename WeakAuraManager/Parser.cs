using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Models;

namespace WeakAuraManager
{
    internal class Parser : IParser
    {
        public static class RowIndices
        {
            public const int ClassName = 0;
            public const int SpellName = 1;
            public const int SpellId = 2;
            public const int SpellTypeTag = 4;
            public const int SizeMultiplier = 5;
            public const int SpellExtraInfo = 6;
            public const int Priority = 7;
            public const int ShowInRaid = 8;
        }

        private static class SpellTags
        {
            public const string Buff = nameof(Buff);
            public const string Debuff = nameof(Debuff);
            public const string Timer = nameof(Timer);
        }

        private readonly IConfiguration _configuration;
        private readonly int _defaultSize;

        public Parser(IConfiguration configuration)
        {
            _configuration = configuration;
            _defaultSize = _configuration.GetValue<int>(ConfigKeys.DefaultSize);
        }

        public async Task<IEnumerable<BaseSpell>> Parse()
        {
            var service = new SheetsService(new BaseClientService.Initializer
            {
                ApplicationName = "Discovery Sample",
                ApiKey = _configuration.GetValue<string>(ConfigKeys.GoogleApiKey),
            });

            var request = service.Spreadsheets.Values.Get(_configuration.GetValue<string>(ConfigKeys.SpreadsheetId), "A2:I");
            var response = await request.ExecuteAsync();

            var results = new List<BaseSpell>();

            foreach (var rowValues in response.Values)
            {
                if (!IsValidRow(rowValues))
                { 
                    continue;
                }

                if ((rowValues[Parser.RowIndices.SpellTypeTag] as string) == SpellTags.Buff)
                {
                    var buff = new BuffSpell(_defaultSize);
                    buff.ParseFromRowValues(rowValues);
                    results.Add(buff);
                }
                else if ((rowValues[Parser.RowIndices.SpellTypeTag] as string) == SpellTags.Debuff)
                {
                    var debuff = new DebuffSpell(_defaultSize);
                    debuff.ParseFromRowValues(rowValues);
                    results.Add(debuff);
                }
                else if ((rowValues[Parser.RowIndices.SpellTypeTag] as string) == SpellTags.Timer)
                {
                    var timer = new TriggerTimedSpell(_defaultSize);
                    timer.ParseFromRowValues(rowValues);
                    results.Add(timer);
                }
                else
                {
                    Console.WriteLine($"Unhandled spell type '{rowValues[Parser.RowIndices.SpellTypeTag] as string}'");
                }
            }

            return results;
        }

        public static string? GetCellValue(IList<object> rowValues, int index)
        {
            if (rowValues.Count <= index)
            {
                return null;
            }

            return rowValues[index] as string;
        }

        private bool IsValidRow(IList<object> rowValues)
        {
            return rowValues.Count >= 5 && !string.IsNullOrWhiteSpace(rowValues.ElementAt(RowIndices.SpellName) as string);
        }
    }
}
