using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeakAuraManager.Models
{
    internal class TriggerTimedSpell : BaseSpell
    {
		private static string TimedDefinitionText = $@"{{
			[""iconSource""] = -1,
			[""xOffset""] = 0,
			[""yOffset""] = 0,
			[""anchorPoint""] = ""RIGHT"",
			[""cooldownSwipe""] = true,
			[""cooldownEdge""] = false,
			[""triggers""] = {{
				{{
					[""trigger""] = {{
						[""use_castType""] = false,
						[""duration""] = ""5"",
						[""use_specific_sourceUnit""] = true,
						[""use_delay""] = false,
						[""spellName""] = 0,
						[""use_absorbHealMode""] = true,
						[""subeventSuffix""] = ""_CAST_SUCCESS"",
						[""event""] = ""Combat Log"",
						[""castType""] = ""cast"",
						[""use_spellId""] = true,
						[""use_sourceUnit""] = false,
						[""check""] = ""event"",
						[""use_track""] = true,
						[""use_absorbMode""] = true,
						[""genericShowOn""] = ""showOnCooldown"",
						[""subeventPrefix""] = ""SPELL"",
						[""custom_type""] = ""stateupdate"",
						[""delay""] = 15,
						[""names""] = {{
						}},
						[""use_cloneId""] = false,
						[""debuffType""] = ""HELPFUL"",
						[""use_stage""] = false,
						[""type""] = ""custom"",
						[""events""] = ""COMBAT_LOG_EVENT_UNFILTERED:SPELL_CAST_SUCCESS"",
						[""custom""] = ""\nfunction(allstates, event, _, subEvent, hideCaster, sourceGuid, sourceName, _, _, _, _, _, _, spellID, ...)\n\n    if sourceName and spellID == {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellId)}\n\n    then\n        local type = \""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellUnit)}\""\n        local key = sourceName .. \"":\"" .. spellID\n\n        if type == \""party\"" or type == \""raid\"" then\n\n            for i = 1, GetNumGroupMembers() do\n                local prefix = IsInRaid() and \""raid\"" or \""party\"" -- ternary operator equivalent\n                local unit = prefix .. i\n\n                local usePlayer = not IsInRaid() and sourceName == UnitName(\""player\"")\n\n                if usePlayer then -- Technically not accurate if same name across realms\n                    unit = \""player\""\n                end\n\n                local unitGuid = UnitGUID(unit)\n\n                if unitGuid == sourceGuid or usePlayer then\n                    local spellInfo = {{GetSpellInfo(spellID)}}\n\n                    allstates[key] = {{\n                        show = true,\n                        changed = true,\n                        progressType = \""timed\"",\n                        duration = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellTimer)},\n                        name = sourceName,\n                        icon = spellInfo[3],\n                        caster = sourceName,\n                        autoHide = true,\n                        unit = unit\n                    }}\n\n                    return true\n                end\n            end\n        elseif type == \""arena\"" then\n            for i = 1, 5 do\n                local unit = \""arena\"" .. i\n\n                local unitGuid = UnitGUID(unit)\n\n                if (unitGuid ~= nil) then\n                    if unitGuid == sourceGuid then\n                        local spellInfo = {{GetSpellInfo(spellID)}}\n    \n                        allstates[key] = {{\n                            show = true,\n                            changed = true,\n                            progressType = \""timed\"",\n                            duration = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SpellTimer)},\n                            name = sourceName,\n                            icon = spellInfo[3],\n                            caster = sourceName,\n                            autoHide = true,\n                            unit = unit\n                        }}\n    \n                        return true\n                    end\n                end\n            end\n        end\n    end\n\n    return false\nend"",				
						[""use_spellName""] = false,
						[""use_genericShowOn""] = true,
						[""custom_hide""] = ""timed"",
						[""use_unit""] = true,
						[""realSpellName""] = 0,
						[""spellIds""] = {{
						}},
						[""unit""] = ""player"",
						[""customTexture""] = """",
						[""stage_operator""] = ""=="",
					}},
					[""untrigger""] = {{
					}},
				}}, -- [1]
				[""disjunctive""] = ""any"",
				[""activeTriggerMode""] = -10,
			}},
			[""internalVersion""] = 66,
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
			[""subRegions""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.SubregionsDefinition)},
			[""height""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""load""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.LoadConstraints)},
			[""regionType""] = ""icon"",
			[""displayIcon""] = """",
			[""selfPoint""] = ""LEFT"",
			[""parent""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.GroupName)}"",
			[""cooldown""] = true,
			[""anchorFrameParent""] = true,
			[""authorOptions""] = {{
			}},
			[""zoom""] = 0,
			[""cooldownTextDisabled""] = false,
			[""config""] = {{
			}},
			[""alpha""] = 1,
			[""id""] = ""{TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraName)}"",
			[""anchorFrameType""] = ""SELECTFRAME"",
			[""frameStrata""] = 1,
			[""width""] = {TemplateHelpers.MakeTag(TemplateHelpers.ReplacementTags.WeakAuraSize)},
			[""useCooldownModRate""] = true,
			[""inverse""] = false,
			[""color""] = {{
				1, -- [1]
				1, -- [2]
				1, -- [3]
				1, -- [4]
			}},
			[""conditions""] = {{
			}},
			[""information""] = {{
			}},
			[""icon""] = true,
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

        public override string GetWeakaura(GroupBuilder.GroupBuilderParameters groupParams)
        {
            var replaceTable = base.GetBaseReplacementTable(groupParams);

			replaceTable.Add(TemplateHelpers.ReplacementTags.SpellTimer, this.TimerDuration.ToString());

            var replaced = TemplateHelpers.Replace(TimedDefinitionText, replaceTable);

			return replaced;
        }
    }
}
