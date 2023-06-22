using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakAuraManager.Models
{
    public enum SpellType
    {
        SelfBuff,
        EnemyDebuff,
        TriggerTimed
    }

    public abstract class SpellModel
    {
        private readonly int _defaultSize;

        public SpellModel(int defaultSize)
        {
            _defaultSize = defaultSize;
        }

        public string SpellName { get; set; }
        public int SpellId { get; set; }
        public int Size { get; set; }
        public SpellType SpellType { get; set; }

        public virtual void ParseFromRowValues(IList<object?> rowValues)
        {
            this.SpellName = rowValues[Parser.RowIndices.SpellName] as string;
            this.SpellId = int.Parse(rowValues[Parser.RowIndices.SpellId] as string ?? "0");

#pragma warning disable CS8604 // Possible null reference argument.
            var sizeValue = Parser.GetCellValue(rowValues, Parser.RowIndices.Size);

            this.Size = !string.IsNullOrWhiteSpace(sizeValue)
                ? int.Parse(sizeValue)
                : _defaultSize;
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public abstract string GetWeakaura(string groupName, string spellUnit);

        protected Dictionary<string, string> GetBaseReplacementTable(string groupName, string spellUnit)
        {
            return new Dictionary<string, string>()
            {
                { TemplateHelpers.ReplacementTags.SpellId, this.SpellId.ToString() },
                { TemplateHelpers.ReplacementTags.WeakAuraName, this.GetWeakauraName(groupName) },
                { TemplateHelpers.ReplacementTags.WeakAuraSize, this.Size.ToString() },
                { TemplateHelpers.ReplacementTags.GroupName, groupName },
                { TemplateHelpers.ReplacementTags.SpellUnit, spellUnit },
            };
        }

        public string GetWeakauraName(string groupName) => $"{groupName} - {this.SpellName}";
    }
}
