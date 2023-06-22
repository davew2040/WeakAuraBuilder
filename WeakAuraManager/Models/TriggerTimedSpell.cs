using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakAuraManager.Models
{
    internal class TriggerTimedSpell : SpellModel
    {
        private static string TimedDefinitionText = @$"{{
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
						[""spellId""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellId)}"",
						[""use_totemName""] = true,
						[""genericShowOn""] = ""showOnCooldown"",
						[""unit""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellUnit)}"",
						[""use_unit""] = true,
						[""use_delay""] = false,
						[""use_track""] = true,
						[""use_genericShowOn""] = true,
						[""use_totemType""] = false,
						[""debuffType""] = ""HELPFUL"",
						[""type""] = ""event"",
						[""use_remaining""] = false,
						[""use_absorbHealMode""] = true,
						[""subeventSuffix""] = ""_CAST_START"",
						[""duration""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellDelay)}"",
						[""use_spellId""] = true,
						[""event""] = ""Spell Cast Succeeded"",
						[""totemName""] = """",
						[""realSpellName""] = 0,
						[""use_spellName""] = true,
						[""spellIds""] = {{
						}},
						[""names""] = {{
						}},
						[""subeventPrefix""] = ""SPELL"",
						[""spellName""] = 0,
						[""delay""] = 15,
						[""use_absorbMode""] = true,
					}},
					[""untrigger""] = {{
					}},
				}}, -- [1]
				[""activeTriggerMode""] = -10,
			}},
			[""internalVersion""] = 65,
			[""keepAspectRatio""] = false,
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
					[""glow""] = false,
					[""useGlowColor""] = false,
					[""glowScale""] = 1,
					[""glowLength""] = 10,
					[""glowYOffset""] = 0,
					[""glowColor""] = {{
						1, -- [1]
						1, -- [2]
						1, -- [3]
						1, -- [4]
					}},
					[""glowType""] = ""buttonOverlay"",
					[""glowXOffset""] = 0,
					[""type""] = ""subglow"",
					[""glowThickness""] = 1,
					[""glowLines""] = 8,
					[""glowBorder""] = false,
				}}, -- [3]
			}},
			[""height""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""load""] = {{
				[""talent""] = {{
					[""multi""] = {{
					}},
				}},
				[""class""] = {{
					[""multi""] = {{
					}},
				}},
				[""use_class_and_spec""] = true,
				[""spec""] = {{
					[""multi""] = {{
					}},
				}},
				[""size""] = {{
					[""multi""] = {{
					}},
				}},
			}},
			[""regionType""] = ""icon"",
			[""displayIcon""] = """",
			[""cooldown""] = true,
			[""actions""] = {{
				[""start""] = {{
				}},
				[""init""] = {{
				}},
				[""finish""] = {{
				}},
			}},
			[""parent""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.GroupName)}"",
			[""authorOptions""] = {{
			}},
			[""frameStrata""] = 1,
			[""zoom""] = 0,
			[""config""] = {{
			}},
			[""selfPoint""] = ""CENTER"",
			[""id""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraName)}"",
			[""anchorFrameType""] = ""SCREEN"",
			[""useCooldownModRate""] = true,
			[""width""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""alpha""] = 1,
			[""inverse""] = false,
			[""cooldownTextDisabled""] = false,
			[""conditions""] = {{
			}},
			[""information""] = {{
			}},
			[""color""] = {{
				1, -- [1]
				1, -- [2]
				1, -- [3]
				1, -- [4]
			}},
		}}";

		public double TimerDuration { get; set; }

        public TriggerTimedSpell(int defaultSize) : base(defaultSize)
        {
            this.SpellType = SpellType.TriggerTimed;
        }

        public override void ParseFromRowValues(IList<object?> rowValues)
        {
            base.ParseFromRowValues(rowValues);

			var duration = Parser.GetCellValue(rowValues, Parser.RowIndices.SpellExtraInfo);

			if (duration == null)
			{
				throw new Exception($"Must provide spell duration for {nameof(TriggerTimedSpell)}.");
			}

			this.TimerDuration = double.Parse(duration);
        }

        public override string GetWeakaura(string groupName, string spellUnit)
        {
            var replaceTable = base.GetBaseReplacementTable(groupName, spellUnit);

			replaceTable.Add(TemplateHelpers.ReplacementTags.SpellDelay, this.TimerDuration.ToString());

            var replaced = TemplateHelpers.Replace(TimedDefinitionText, replaceTable);

			return replaced;
        }
    }
}
