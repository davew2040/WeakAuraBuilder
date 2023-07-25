using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Constants;
using WeakAuraManager.Models;

namespace WeakAuraManager
{
	public class GroupBuilder : IGroupBuilder
	{
		public enum FrameType
		{
			Party,
			ArenaFriendlies,
			ArenaEnemies,
			Raid
		}

        public enum GroupType
        {
			Undefined,
            Buffs,
			Debuffs
        }

        private static string GroupTemplate = @$"{{
			[""arcLength""] = 360,
			[""controlledChildren""] = {{
				{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.GroupWeakAuras)}
			}},
			[""borderBackdrop""] = ""Blizzard Tooltip"",
			[""authorOptions""] = {{
			}},
			[""groupIcon""] = 369278,
			[""gridType""] = ""RD"",
			[""fullCircle""] = true,
			[""useAnchorPerUnit""] = true,
			[""actions""] = {{
				[""start""] = {{
				}},
				[""init""] = {{
				}},
				[""finish""] = {{
				}},
			}},
			[""triggers""] = {{
				{{
					[""trigger""] = {{
						[""names""] = {{
						}},
						[""type""] = ""aura2"",
						[""spellIds""] = {{
						}},
						[""subeventSuffix""] = ""_CAST_START"",
						[""unit""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellUnit)}"",
						[""subeventPrefix""] = ""SPELL"",
						[""event""] = ""Health"",
						[""debuffType""] = ""HELPFUL"",
					}},
					[""untrigger""] = {{
					}},
				}}, -- [1]
			}},
			[""columnSpace""] = 1,
			[""internalVersion""] = 65,
			[""useLimit""] = true,
			[""align""] = ""LEFT"",
			[""config""] = {{
			}},
			[""borderColor""] = {{
				0, -- [1]
				0, -- [2]
				0, -- [3]
				1, -- [4]
			}},
			[""rotation""] = 0,
			[""space""] = 3,
			[""radius""] = 200,
			[""subRegions""] = {{
			}},
			[""stagger""] = 0,
			[""selfPoint""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SelfPoint)}"",
			[""load""] = {{
				[""size""] = {{
					[""multi""] = {{
					}},
				}},
				[""spec""] = {{
					[""multi""] = {{
					}},
				}},
				[""class""] = {{
					[""multi""] = {{
					}},
				}},
				[""talent""] = {{
					[""multi""] = {{
					}},
				}},
			}},
			[""anchorPoint""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.AnchorPoint)}"",
			[""backdropColor""] = {{
				1, -- [1]
				1, -- [2]
				1, -- [3]
				0.5, -- [4]
			}},
			[""borderInset""] = 1,
			[""animate""] = true,
			[""grow""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.GrowDirection)}"",
			[""scale""] = 1,
			[""centerType""] = ""LR"",
			[""border""] = false,
			[""anchorFrameFrame""] = ""PlayerFrame"",
			[""regionType""] = ""dynamicgroup"",
			[""hybridPosition""] = ""hybridFirst"",
			[""anchorPerUnit""] = ""UNITFRAME"",
			[""rowSpace""] = 1,
			[""gridWidth""] = 5,
			[""hybridSortMode""] = ""ascending"",
			[""constantFactor""] = ""RADIUS"",
			[""limit""] = 3,
			[""borderOffset""] = 4,
			[""animation""] = {{
				[""start""] = {{
					[""type""] = ""none"",
					[""easeStrength""] = 3,
					[""duration_type""] = ""seconds"",
					[""easeType""] = ""none"",
				}},
				[""main""] = {{
					[""type""] = ""none"",
					[""easeStrength""] = 3,
					[""duration_type""] = ""seconds"",
					[""easeType""] = ""none"",
				}},
				[""finish""] = {{
					[""type""] = ""none"",
					[""easeStrength""] = 3,
					[""duration_type""] = ""seconds"",
					[""easeType""] = ""none"",
				}},
			}},
			[""borderEdge""] = ""Square Full White"",
			[""anchorFrameParent""] = false,
			[""frameStrata""] = 1,
			[""anchorFrameType""] = ""SELECTFRAME"",
			[""sort""] = ""none"",
			[""borderSize""] = 2,
			[""xOffset""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.XOffset)},
			[""conditions""] = {{
			}},
			[""information""] = {{
			}},
			[""yOffset""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.YOffset)}
		}}";

		public string GetGroupText(IEnumerable<BaseSpell> spells, GroupBuilder.GroupBuilderParameters groupParams)
		{
			var tabTable = new Dictionary<string, string>()
			{
				{
					TemplateHelpers.ReplacementTags.GroupWeakAuras,
					string.Join(",\n", spells.Select(s => $"\"{s.GetWeakauraName(groupParams.GroupName)}\""))
				},
				{ TemplateHelpers.ReplacementTags.GroupName, groupParams.GroupName },
				{ TemplateHelpers.ReplacementTags.SpellUnit, groupParams.SpellUnit },
				{ TemplateHelpers.ReplacementTags.XOffset, groupParams.XOffset.ToString() },
				{ TemplateHelpers.ReplacementTags.YOffset, groupParams.YOffset.ToString() },
				{ TemplateHelpers.ReplacementTags.AnchorPoint, groupParams.AnchorPoint },
				{ TemplateHelpers.ReplacementTags.SelfPoint, groupParams.SelfAnchor },
				{ TemplateHelpers.ReplacementTags.GrowDirection, groupParams.GrowDirection },
			};

			return TemplateHelpers.Replace(GroupTemplate, tabTable);
		}

		public class GroupBuilderParameters
		{
			public string GroupName { get; set; }
			public string SpellUnit { get; set; }
			public int XOffset { get; set; }
			public int YOffset { get; set; }
			public string SelfAnchor { get; set; }
			public string AnchorPoint { get; set; }
			public int DefaultSize { get; set; }
			public bool ShowText { get; set; } = true;
			public FrameType FrameType { get; set; }
			public GroupType GroupType { get; set; } = GroupType.Undefined;
            public string GrowDirection { get; set; } = GrowDirections.Right;
			public AuraColor BorderColor { get; set; }
			public bool UseBorder { get; set; } = false;
			public int BorderThickness { get; set; } = 0;
            public bool LoadNever { get; set; } = false;
        }
	}
}
