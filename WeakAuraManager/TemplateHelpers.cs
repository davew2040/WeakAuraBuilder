using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakAuraManager
{
    public static class TemplateHelpers
    {
        public static class ReplacementTags
        {
            public const string SpellUnit = "SPELL_UNIT";
            public const string SpellId = "SPELL_ID";
            public const string GroupName = "GROUP_NAME";
            public const string WeakAuraName = "WEAKAURA_NAME";
            public const string WeakAuraSize = "WEAKAURA_WIDTH";
            public const string GroupWeakAuras = "GROUP_WEAKAURAS";
            public const string SpellDelay = "SPELL_DELAY";
        }

        public static string MakeTag(string tag) => "{{" + tag + "}}";

        public static string Replace(string source, Dictionary<string, string> replaceTable)
        {
            var tagTable = replaceTable.ToDictionary(kvp => MakeTag(kvp.Key), kvp => kvp.Value);

            return source.FormatFromDictionary(tagTable);
        }
    }
}
