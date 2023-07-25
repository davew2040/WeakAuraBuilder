using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager;

namespace WeakAuraManager.Models
{
    public class TestAnchorSpell : BaseSpell
    {
        private static string TextTemplate = $@"{{
			[""iconSource""] = -1,
			[""xOffset""] = 0,
			[""yOffset""] = 0,
			[""anchorPoint""] = ""CENTER"",
			[""cooldownSwipe""] = true,
			[""cooldownEdge""] = false,
			[""icon""] = true,
			[""triggers""] = {{
				{{
					[""trigger""] = {{
						[""use_absorbMode""] = true,
						[""unit""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellUnit)}"",
						[""debuffType""] = ""HARMFUL"",
						[""showClones""] = true,
						[""useName""] = false,
						[""use_health""] = false,
						[""auraspellids""] = {{
							""221562"", -- [1]
						}},
						[""subeventSuffix""] = ""_CAST_START"",
						[""event""] = ""Health"",
						[""names""] = {{
						}},
						[""use_unit""] = true,
						[""subeventPrefix""] = ""SPELL"",
						[""spellIds""] = {{
						}},
						[""useExactSpellId""] = true,
						[""health""] = ""0"",
						[""use_absorbHealMode""] = true,
						[""type""] = ""unit"",
						[""health_operator""] = "">"",
					}},
					[""untrigger""] = {{
					}},
				}}, -- [1]
				[""activeTriggerMode""] = -10,
			}},
			[""internalVersion""] = 66,
			[""keepAspectRatio""] = false,
			[""selfPoint""] = ""CENTER"",
			[""desaturate""] = false,
			[""subRegions""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SubregionsDefinition)},
			[""height""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""load""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.LoadConstraints)},
			[""authorMode""] = true,
			[""regionType""] = ""icon"",
			[""information""] = {{
			}},
			[""authorOptions""] = {{
				{{
					[""type""] = ""toggle"",
					[""default""] = false,
					[""name""] = ""Option 1"",
					[""useDesc""] = false,
					[""key""] = ""option"",
					[""width""] = 1,
				}}, -- [1]
			}},
			[""color""] = {{
				1, -- [1]
				1, -- [2]
				1, -- [3]
				1, -- [4]
			}},
			[""actions""] = {{
				[""start""] = {{
				}},
				[""finish""] = {{
				}},
				[""init""] = {{
				}},
			}},
			[""parent""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.GroupName)}"",
			[""cooldownTextDisabled""] = false,
			[""frameStrata""] = 1,
			[""config""] = {{
				[""option""] = false,
			}},
			[""id""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraName)}"",
			[""anchorFrameType""] = ""SCREEN"",
			[""alpha""] = 1,
			[""width""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""useCooldownModRate""] = true,
			[""uid""] = ""X)PYOk3(TU4"",
			[""inverse""] = false,
			[""zoom""] = 0,
			[""conditions""] = {{
			}},
			[""cooldown""] = true,
			[""animation""] = {{
				[""start""] = {{
					[""easeStrength""] = 3,
					[""type""] = ""none"",
					[""duration_type""] = ""seconds"",
					[""easeType""] = ""none"",
				}},
				[""main""] = {{
					[""easeStrength""] = 3,
					[""type""] = ""none"",
					[""duration_type""] = ""seconds"",
					[""easeType""] = ""none"",
				}},
				[""finish""] = {{
					[""easeStrength""] = 3,
					[""type""] = ""none"",
					[""duration_type""] = ""seconds"",
					[""easeType""] = ""none"",
				}},
			}},
		}}";

		public TestAnchorSpell(int defaultSize): base(defaultSize)
		{
            this.SpellType = SpellType.TestAnchor;
			this.SpellName = "Test Anchor";
        }

        public override string GetWeakaura(GroupBuilder.GroupBuilderParameters groupParams)
        {
			var replaceTable = base.GetBaseReplacementTable(groupParams);
			var replaced = TemplateHelpers.Replace(TextTemplate, replaceTable);

			return replaced;
        }
    }
}
