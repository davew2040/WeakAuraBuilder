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
        TriggerTimed,
        TestAnchor
    }

    public abstract class BaseSpell
    {
        public const int DefaultPriority = 10;

        private readonly int _defaultSize;

        public BaseSpell(int defaultSize)
        {
            _defaultSize = defaultSize;
        }

        public string SpellName { get; set; }
        public bool ShowInRaid { get; set; } = false;
        public int SpellId { get; set; }
        public double SizeMultiplier { get; set; } = 1.0;
        public SpellType SpellType { get; set; }
        public int Priority { get; set; } = DefaultPriority;
        public bool? LoadNever { get; set;} = null;

        public virtual void ParseFromRowValues(IList<object?> rowValues)
        {
            this.SpellName = rowValues[Parser.RowIndices.SpellName] as string;
            this.SpellId = int.Parse(rowValues[Parser.RowIndices.SpellId] as string ?? "0");

#pragma warning disable CS8604 // Possible null reference argument.
            var sizeValue = Parser.GetCellValue(rowValues, Parser.RowIndices.SizeMultiplier);

            if (!string.IsNullOrWhiteSpace(sizeValue))
            {
                this.SizeMultiplier = double.Parse(sizeValue);
            }
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
            var priorityValue = Parser.GetCellValue(rowValues, Parser.RowIndices.Priority);

            if (!string.IsNullOrWhiteSpace(priorityValue))
            {
                this.Priority = int.Parse(priorityValue);
            }
#pragma warning restore CS8604 // Possible null reference argument.

#pragma warning disable CS8604 // Possible null reference argument.
            var showInRaid = Parser.GetCellValue(rowValues, Parser.RowIndices.ShowInRaid);

            if (!string.IsNullOrWhiteSpace(showInRaid))
            {
                this.ShowInRaid = true;
            }
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public abstract string GetWeakaura(GroupBuilder.GroupBuilderParameters groupParams);

        protected Dictionary<string, string> GetBaseReplacementTable(GroupBuilder.GroupBuilderParameters groupParams)
        {
            int size = (int)(groupParams.DefaultSize * this.SizeMultiplier);

            var replacementTable = new Dictionary<string, string>()
            {
                { TemplateHelpers.ReplacementTags.SpellId, this.SpellId.ToString() },
                { TemplateHelpers.ReplacementTags.WeakAuraName, this.GetWeakauraName(groupParams.GroupName) },
                { TemplateHelpers.ReplacementTags.WeakAuraSize, size.ToString() },
                { TemplateHelpers.ReplacementTags.GroupName, groupParams.GroupName },
                { TemplateHelpers.ReplacementTags.SpellUnit, groupParams.SpellUnit },
                { TemplateHelpers.ReplacementTags.LoadConstraints, TemplateHelpers.GetLoaderText(groupParams, this) },
                { TemplateHelpers.ReplacementTags.SubregionsDefinition, TemplateHelpers.GetSubregionText(groupParams) }
            };

            return replacementTable;
        }

        public string GetWeakauraName(string groupName) => $"{groupName} - {this.SpellName}";
    }
}
