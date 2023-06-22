using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager;

namespace WeakAuraManager.Models
{
    public class DebuffSpell : SpellModel
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
						[""useExactSpellId""] = true,
						[""subeventPrefix""] = ""SPELL"",
						[""auraspellids""] = {{
							""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellId)}"", -- [1]
						}},
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
			[""subRegions""] = {{
				{{
					[""type""] = ""subbackground"",
				}}, -- [1]
				{{
					[""text_shadowXOffset""] = 0,
					[""text_text_format_s_format""] = ""none"",
					[""text_text""] = ""%s"",
					[""text_shadowColor""] = {{
						0, -- [1]
						0, -- [2]
						0, -- [3]
						1, -- [4]
					}},
					[""text_selfPoint""] = ""AUTO"",
					[""text_automaticWidth""] = ""Auto"",
					[""text_fixedWidth""] = 64,
					[""anchorYOffset""] = 0,
					[""text_justify""] = ""CENTER"",
					[""rotateText""] = ""NONE"",
					[""type""] = ""subtext"",
					[""text_color""] = {{
						1, -- [1]
						1, -- [2]
						1, -- [3]
						1, -- [4]
					}},
					[""text_font""] = ""Friz Quadrata TT"",
					[""text_shadowYOffset""] = 0,
					[""text_wordWrap""] = ""WordWrap"",
					[""text_fontType""] = ""OUTLINE"",
					[""text_anchorPoint""] = ""INNER_BOTTOMRIGHT"",
					[""text_fontSize""] = 12,
					[""anchorXOffset""] = 0,
					[""text_visible""] = true,
				}}, -- [2]
				{{
					[""glowFrequency""] = 0.25,
					[""type""] = ""subglow"",
					[""glowXOffset""] = 0,
					[""glowType""] = ""buttonOverlay"",
					[""glowLength""] = 10,
					[""glowYOffset""] = 0,
					[""glowColor""] = {{
						1, -- [1]
						1, -- [2]
						1, -- [3]
						1, -- [4]
					}},
					[""glow""] = false,
					[""glowThickness""] = 1,
					[""useGlowColor""] = false,
					[""glowScale""] = 1,
					[""glowLines""] = 8,
					[""glowBorder""] = false,
				}}, -- [3]
			}},
			[""height""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""load""] = {{
				[""use_never""] = false,
				[""talent""] = {{
					[""multi""] = {{
					}},
				}},
				[""class""] = {{
					[""multi""] = {{
					}},
				}},
				[""spec""] = {{
					[""multi""] = {{
					}},
				}},
				[""size""] = {{
					[""multi""] = {{
					}},
				}},
			}},
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

		public DebuffSpell(int defaultSize): base(defaultSize)
		{
            this.SpellType = SpellType.EnemyDebuff;
        }

        public override string GetWeakaura(string groupName, string spellUnit)
        {
			var replaceTable = base.GetBaseReplacementTable(groupName, spellUnit);
			var replaced = TemplateHelpers.Replace(TextTemplate, replaceTable);

			return replaced;
        }
    }
}
