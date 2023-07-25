using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakAuraManager
{
    public static class ConfigKeys
    {
        public const string WeakAurasConfigPath = nameof(WeakAurasConfigPath);
        public const string BackupPath = nameof(BackupPath);
        public const string GoogleApiKey = nameof(GoogleApiKey);
        public const string SpreadsheetId = nameof(SpreadsheetId);
        public const string GroupPrefix = nameof(GroupPrefix);
        public const string DefaultSize = nameof(DefaultSize);
        public const string RaidDefaultSize = nameof(RaidDefaultSize);
        public const string AddRaidBuffs = nameof(AddRaidBuffs);
        public const string AddTestAnchor = nameof(AddTestAnchor);
    }
}