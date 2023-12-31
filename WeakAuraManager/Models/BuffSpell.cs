﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager;

namespace WeakAuraManager.Models
{
    public class BuffSpell : BaseSpell
    {
        private static string TextTemplate = $@"{{
			[""iconSource""] = -1,
			[""authorOptions""] = {{
				{{
					[""type""] = ""toggle"",
					[""useDesc""] = false,
					[""key""] = ""option"",
					[""name""] = ""Option 1"",
					[""default""] = false,
					[""width""] = 1,
				}}, -- [1]
			}},
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
						[""auraspellids""] = {{
							""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellId)}"", -- [1]
						}},
						[""event""] = ""Health"",
						[""subeventPrefix""] = ""SPELL"",
						[""useExactSpellId""] = true,
						[""spellIds""] = {{
						}},
						[""subeventSuffix""] = ""_CAST_START"",
						[""useName""] = false,
						[""names""] = {{
						}},
						[""unit""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellUnit)}"",
						[""debuffType""] = ""HELPFUL"",
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
			[""xOffset""] = 0,
			[""parent""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.GroupName)}"",
			[""actions""] = {{
				[""start""] = {{
				}},
				[""init""] = {{
				}},
				[""finish""] = {{
				}},
			}},
			[""color""] = {{
				1, -- [1]
				1, -- [2]
				1, -- [3]
				1, -- [4]
			}},
			[""cooldownTextDisabled""] = false,
			[""useCooldownModRate""] = true,
			[""config""] = {{
				[""option""] = false,
			}},
			[""id""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraName)}"",
			[""anchorFrameType""] = ""SCREEN"",
			[""alpha""] = 1,
			[""width""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""frameStrata""] = 1,
			[""inverse""] = false,
			[""zoom""] = 0,
			[""conditions""] = {{
			}},
			[""information""] = {{
			}},
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
		}}";

		public BuffSpell(int defaultSize): base(defaultSize)
		{
            this.SpellType = SpellType.SelfBuff;
        }

        public override string GetWeakaura(GroupBuilder.GroupBuilderParameters groupParams)
        {
			var replaceTable = base.GetBaseReplacementTable(groupParams);
			var replaced = TemplateHelpers.Replace(TextTemplate, replaceTable);

			return replaced;
        }
    }
}
