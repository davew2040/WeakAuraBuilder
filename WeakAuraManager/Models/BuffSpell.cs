using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager;

namespace WeakAuraManager.Models
{
    public class BuffSpell : SpellModel
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
					[""text_visible""] = true,
					[""text_anchorPoint""] = ""INNER_BOTTOMRIGHT"",
					[""text_fontSize""] = 12,
					[""anchorXOffset""] = 0,
					[""text_fontType""] = ""OUTLINE"",
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
					[""useGlowColor""] = false,
					[""glow""] = false,
					[""glowScale""] = 1,
					[""glowThickness""] = 1,
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

        public override string GetWeakaura(string groupName, string spellUnit)
        {
			var replaceTable = base.GetBaseReplacementTable(groupName, spellUnit);
			var replaced = TemplateHelpers.Replace(TextTemplate, replaceTable);

			return replaced;
        }
    }
}
