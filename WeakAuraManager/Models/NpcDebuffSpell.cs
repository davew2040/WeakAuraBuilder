using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager;

namespace WeakAuraManager.Models
{
    public class NpcDebuffSpell : BaseSpell
    {
        private static string TextTemplate = $@"{{
			[""iconSource""] = -1,
			[""parent""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.GroupName)}"",
			[""yOffset""] = 0,
			[""anchorPoint""] = ""CENTER"",
			[""cooldownSwipe""] = true,
			[""cooldownEdge""] = false,
			[""icon""] = true,
			[""triggers""] = {{
				{{
					[""trigger""] = {{
						[""showClones""] = true,
						[""type""] = ""aura2"",
						[""subeventSuffix""] = ""_CAST_START"",
						[""event""] = ""Health"",
						[""unit""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellUnit)}"",
						[""names""] = {{
						}},
						[""spellIds""] = {{
						}},
						[""useName""] = false,
						[""useExactSpellId""] = false,
						[""use_castByPlayer""] = false,
						[""subeventPrefix""] = ""SPELL"",
						[""debuffType""] = ""HARMFUL"",
					}},
					[""untrigger""] = {{
					}},
				}}, -- [1]
				[""activeTriggerMode""] = -10,
			}},
			[""internalVersion""] = 65,
			[""keepAspectRatio""] = false,
			[""selfPoint""] = ""CENTER"",
			[""desaturate""] = false,
			[""subRegions""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SubregionsDefinition)},
			[""height""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""load""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.LoadConstraints)},
			[""authorMode""] = true,
			[""regionType""] = ""icon"",
			[""cooldown""] = true,
			[""animation""] = {{
				[""start""] = {{
					[""duration_type""] = ""seconds"",
					[""easeStrength""] = 3,
					[""type""] = ""none"",
					[""easeType""] = ""none"",
				}},
				[""main""] = {{
					[""duration_type""] = ""seconds"",
					[""easeStrength""] = 3,
					[""type""] = ""none"",
					[""easeType""] = ""none"",
				}},
				[""finish""] = {{
					[""duration_type""] = ""seconds"",
					[""easeStrength""] = 3,
					[""type""] = ""none"",
					[""easeType""] = ""none"",
				}},
			}},
			[""xOffset""] = 0,
			[""actions""] = {{
				[""start""] = {{
				}},
				[""finish""] = {{
				}},
				[""init""] = {{
				}},
			}},
			[""alpha""] = 1,
			[""cooldownTextDisabled""] = false,
			[""color""] = {{
				1, -- [1]
				1, -- [2]
				1, -- [3]
				1, -- [4]
			}},
			[""id""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraName)}"",
			[""width""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""frameStrata""] = 1,
			[""anchorFrameType""] = ""SCREEN"",
			[""useCooldownModRate""] = true,
			[""config""] = {{
				[""option""] = false,
			}},
			[""inverse""] = false,
			[""zoom""] = 0,
			[""conditions""] = {{
			}},
			[""information""] = {{
			}},
			[""authorOptions""] = {{
				{{
					[""type""] = ""toggle"",
					[""useDesc""] = false,
					[""key""] = ""option"",
					[""default""] = false,
					[""name""] = ""Option 1"",
					[""width""] = 1,
				}}, -- [1]
			}},
		}}";

		public NpcDebuffSpell(int defaultSize): base(defaultSize)
		{
            this.SpellType = SpellType.EnemyDebuff;
			this.SpellName = "NPC Debuffs";
        }

        public override string GetWeakaura(GroupBuilder.GroupBuilderParameters groupParams)
        {
			var replaceTable = base.GetBaseReplacementTable(groupParams);
			var replaced = TemplateHelpers.Replace(TextTemplate, replaceTable);

			return replaced;
        }
    }
}
