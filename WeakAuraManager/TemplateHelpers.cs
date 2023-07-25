using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Constants;
using WeakAuraManager.Models;
using static WeakAuraManager.GroupBuilder;

namespace WeakAuraManager
{
    public static class TemplateHelpers
    {
		private static string LoadingPartyEverywhereTemplate = $@"{{
				[""difficulty""] = {{
				}},
				[""ingroup""] = {{
					[""single""] = ""group"",
				}},
				[""use_never""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.LoadNever)},
				[""talent""] = {{
					[""multi""] = {{
					}},
				}},
				[""use_ingroup""] = true,
				[""spec""] = {{
					[""multi""] = {{
					}},
				}},
				[""class""] = {{
					[""multi""] = {{
					}},
				}},
				[""size""] = {{
					[""multi""] = {{
					}},
				}},
			}}";

        private static string LoadingRaidEverywhereTemplate = $@"{{
				[""difficulty""] = {{
				}},
				[""ingroup""] = {{
					[""single""] = ""raid"",
				}},
				[""use_never""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.LoadNever)},
				[""talent""] = {{
					[""multi""] = {{
					}},
				}},
				[""use_ingroup""] = true,
				[""spec""] = {{
					[""multi""] = {{
					}},
				}},
				[""class""] = {{
					[""multi""] = {{
					}},
				}},
				[""size""] = {{
					[""multi""] = {{
						[""scenario""] = true,
						[""ten""] = true,
						[""twentyfive""] = true,
						[""fortyman""] = true,
						[""ratedpvp""] = true,
						[""party""] = true,
						[""flexible""] = true,
						[""pvp""] = true,
						[""twenty""] = true,
						[""none""] = true,
					}},
				}},
			}}";

        private static string LoadingArenaTemplate = $@"{{
				[""use_size""] = false,
				[""use_never""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.LoadNever)},
				[""use_race""] = false,
				[""talent""] = {{
					[""multi""] = {{
					}},
				}},
				[""instance_type""] = {{
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
						[""arena""] = true,
						[""ratedarena""] = true,
					}},
				}},
			}}";

		public static string TextTemplate = $@"{{
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
			[""text_visible""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.TextEnabled)},
		}}";

		private static string BorderTemplate = $@"{{
			[""border_offset""] = 0,
			[""type""] = ""subborder"",
			[""border_color""] = {{
				{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderRed)}, -- [1]
				{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderGreen)}, -- [2]
				{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderBlue)}, -- [3]
				{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderAlpha)}, -- [4]
			}},
			[""border_visible""] = true,
			[""border_edge""] = ""Square Full White"",
			[""border_size""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderSize)},
		}}";

		private static string SubregionsTemplate = $@"{{
				{{
					[""type""] = ""subbackground"",
				}}, -- [1]
				{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.TextDefinition)}
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
				}},
				{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderDefinition)}
			}}";

		public static class ReplacementTags
        {
            public const string SpellUnit = "SPELL_UNIT";
            public const string SpellId = "SPELL_ID";
            public const string GroupName = "GROUP_NAME";
            public const string WeakAuraName = "WEAKAURA_NAME";
            public const string WeakAuraSize = "WEAKAURA_WIDTH";
            public const string GroupWeakAuras = "GROUP_WEAKAURAS";
            public const string SpellTimer = "SPELL_TIMER";
            public const string XOffset = "X_OFFSET";
            public const string YOffset = "Y_OFFSET";
            public const string LoadConstraints = "LOAD_CONSTRAINTS";
            public const string AnchorPoint = "ANCHOR_POINT";
            public const string SelfPoint = "SELF_POINT";
            public const string TextDefinition = "TEXT_DEFINITION";
			public const string BorderDefinition = "BORDER_DEFINITION";
            public const string SubregionsDefinition = "SUBREGIONS_DEFINITION";
            public const string GrowDirection = "GROW_DIRECTION";
            public const string TextEnabled = "TEXT_ENABLED";
            public const string BorderRed = "BORDER_RED";
            public const string BorderGreen = "BORDER_GREEN";
            public const string BorderBlue = "BORDER_BLUE";
            public const string BorderAlpha = "BORDER_ALPHA";
            public const string BorderSize = "BORDER_SIZE";
            public const string LoadNever = "LOAD_NEVER";
        }

        public static string MakeTag(string tag) => "{{" + tag + "}}";

        public static string Replace(string source, Dictionary<string, string> replaceTable)
        {
            var tagTable = replaceTable.ToDictionary(kvp => MakeTag(kvp.Key), kvp => kvp.Value);

            return source.FormatFromDictionary(tagTable);
        }
        
        public static string GetLoaderText(GroupBuilder.GroupBuilderParameters groupParams, BaseSpell spell)
        {
			var text = groupParams.FrameType switch
			{
				FrameType.Party => LoadingPartyEverywhereTemplate,
				FrameType.Raid => LoadingRaidEverywhereTemplate,
                FrameType.ArenaFriendlies => LoadingArenaTemplate,
                FrameType.ArenaEnemies => LoadingArenaTemplate,
				_ => throw new ArgumentException($"Unrecognized frame type {groupParams.FrameType}")
			};

			var loadNever = false;

			if (spell.LoadNever.HasValue)
			{
				loadNever = spell.LoadNever.Value;
			}
			else
			{
				loadNever = groupParams.LoadNever;
			}

            var replacementTable = new Dictionary<string, string>()
            {
                { TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.LoadNever), loadNever ? "true" : "false" }
            };

			var replaced = text.FormatFromDictionary(replacementTable);

            return replaced;
        }

        public static string GetBorderText(GroupBuilder.GroupBuilderParameters groupParams)
        {
			if (!groupParams.UseBorder)
			{
				return string.Empty;
			}
			var replacementTable = new Dictionary<string, string>()
			{
				{ TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderRed), groupParams.BorderColor.Red.ToString() },
				{ TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderGreen), groupParams.BorderColor.Green.ToString() },
				{ TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderBlue), groupParams.BorderColor.Blue.ToString() },
                { TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderAlpha), groupParams.BorderColor.Alpha.ToString() },
                { TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderSize), groupParams.BorderThickness.ToString() }
            };

			var replaced = BorderTemplate.FormatFromDictionary(replacementTable) + ",";

            return replaced;
        }

        private static string GetTextLua(bool textEnabled)
		{
            var replacementTable = new Dictionary<string, string>()
            {
                { TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.TextEnabled), textEnabled ? "true" : "false" }
            };

			return TextTemplate.FormatFromDictionary(replacementTable) + ",";
        }

        public static string GetSubregionText(GroupBuilder.GroupBuilderParameters groupParams)
        {
            var replacementTable = new Dictionary<string, string>()
            {
                { TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.TextDefinition), GetTextLua(groupParams.ShowText) },
                { TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.BorderDefinition), GetBorderText(groupParams) },
            };

            return SubregionsTemplate.FormatFromDictionary(replacementTable);
        }
    }
}
