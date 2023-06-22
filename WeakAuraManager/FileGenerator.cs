using Microsoft.Extensions.Configuration;
using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WeakAuraManager
{
    internal class FileGenerator : IFileGenerator
    {
        private readonly IConfiguration _configuration;

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
			[""selfPoint""] = ""TOPLEFT"",
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
			[""anchorPoint""] = ""TOPRIGHT"",
			[""backdropColor""] = {{
				1, -- [1]
				1, -- [2]
				1, -- [3]
				0.5, -- [4]
			}},
			[""borderInset""] = 1,
			[""animate""] = true,
			[""grow""] = ""RIGHT"",
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
			[""id""] = ""Party Buffs"",
			[""anchorFrameParent""] = false,
			[""frameStrata""] = 1,
			[""anchorFrameType""] = ""SELECTFRAME"",
			[""sort""] = ""none"",
			[""uid""] = ""lFeUDmPkvab"",
			[""borderSize""] = 2,
			[""xOffset""] = 10,
			[""conditions""] = {{
			}},
			[""information""] = {{
			}},
			[""yOffset""] = 0
		}}";

        public FileGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

		public async Task<string> GenerateFileContents(string initialLuaContent, IEnumerable<SpellModel> spells)
		{
			try
			{
				// TODO - add another unit types?
				var spellUnit = "party";

				string groupName = GetGroupName();

				Lua state = new Lua();

				state.DoString(initialLuaContent);

				this.AddGroups(state, spells, spellUnit);

				AddLuaHelpers(state);

				var finalFileContent = Serialize(state, "WeakAurasSaved");

				return "WeakAurasSaved = " + finalFileContent;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		private void AddGroups(Lua state, IEnumerable<SpellModel> spells, string spellUnit)
		{
			AddGroup(
				_configuration.GetValue<string>(ConfigKeys.GroupName) + " - Buffs", 
				state, 
				spells.Where(s => s.SpellType == SpellType.SelfBuff || s.SpellType == SpellType.TriggerTimed), 
				spellUnit
			);

			AddGroup(
				_configuration.GetValue<string>(ConfigKeys.GroupName) + " - Debuffs", 
				state, 
				spells.Where(s => s.SpellType == SpellType.EnemyDebuff), 
				spellUnit
			);
        }

		private void AddGroup(string groupName, Lua state, IEnumerable<SpellModel> spells, string spellUnit)
		{
            var displaysTable = (state["WeakAurasSaved"] as LuaTable)["displays"] as LuaTable;

            if (displaysTable.Keys.Cast<string>().Any(k => k == groupName))
            {
                throw new Exception($"WeakAuras file already contains group name '{groupName}'");
            }

            var spellDefinitions = spells.OrderByDescending(s => s.Size).Select(s => new { original = s, text = s.GetWeakaura(groupName, spellUnit) });

            foreach (var spellDefinition in spellDefinitions)
            {
                InsertLuaText(spellDefinition.text, state, $"WeakAurasSaved.displays[\"{spellDefinition.original.GetWeakauraName(groupName)}\"]");
            }

            var groupText = GetGroupText(spellDefinitions.Select(s => s.original), groupName, spellUnit);

            InsertLuaText(groupText, state, $"WeakAurasSaved.displays[\"{groupName}\"]");
        }

        private string GetGroupText(IEnumerable<SpellModel> spells, string groupName, string spellUnit)
		{
			var tabTable = new Dictionary<string, string>()
			{
				{ TemplateHelpers.ReplacementTags.GroupWeakAuras, string.Join(",\n", spells.Select(s => $"\"{s.GetWeakauraName(groupName)}\"")) },
				{ TemplateHelpers.ReplacementTags.GroupName, groupName },
				{ TemplateHelpers.ReplacementTags.SpellUnit, spellUnit },
			};

			return TemplateHelpers.Replace(GroupTemplate, tabTable);
		}

		private void InsertLuaText(string luaText, Lua state, string outputPath)
		{
            var luaEncoding = new Lua();

            luaEncoding.DoString("TempAura = " + luaText);

            AddLuaHelpers(luaEncoding);

			var commandText = $"{outputPath} = {Serialize(luaEncoding, "TempAura")}";

            state.DoString(commandText);
        }

        private void Traverse(LuaTable luaTable, string location)
        {
            foreach (var key in luaTable.Keys)
            {
                if (key is string && ((string)key).Contains("All - PvP Friendly Buffs"))
                {
                    var value = luaTable[key];
                }

                if (luaTable[key] is LuaTable)
                {
                    Traverse(luaTable[key] as LuaTable, $"{location}/{key}");
                }
            }
        }

        private string GetGroupName() => _configuration.GetValue<string>(ConfigKeys.GroupName);

        private Dictionary<object, object> ToTable(LuaTable luaTable)
        {
            return luaTable.Keys.Cast<object>().ToDictionary(k => k, k =>
			{
				if (luaTable[k] is LuaTable)
				{
					return ToTable(luaTable[k] as LuaTable);
				}
				else
				{
					return luaTable[k];
				}
			});
        }

        private (IEnumerable<string> missing, IEnumerable<string> added) GetKeyDiff(IEnumerable<string> one, IEnumerable<string> two)
        {
            return (
                one.Where(a => !two.Contains(a)),
                two.Where(a => !one.Contains(a))
            );
        }

        private LuaTable GetTableByPath(IEnumerable<string> path, Lua table)
        {
			LuaTable next = table[path.ElementAt(0)] as LuaTable;
            for (int i=1; i<path.Count(); i++)
            {
				var part = path.ElementAt(i);
                next = next[part] as LuaTable;
            }

            return next;
        }

		private string Serialize(Lua state, string sourceVariableName)
		{
            var serialized = state.DoString($"return serialize({sourceVariableName})")[0] as string;

            var removeReturn = serialized.Substring("return ".Length);

            return removeReturn;
        }

        private void AddLuaHelpers(Lua luaState)
        {
//            luaState.DoString(@"function table.show(t, name, indent)
//   local cart     -- a container
//   local autoref  -- for self references

//   --[[ counts the number of elements in a table
//   local function tablecount(t)
//      local n = 0
//      for _, _ in pairs(t) do n = n+1 end
//      return n
//   end
//   ]]
//   -- (RiciLake) returns true if the table is empty
//   local function isemptytable(t) return next(t) == nil end

//   local function basicSerialize (o)
//      local so = tostring(o)
//      if type(o) == ""function"" then
//         local info = debug.getinfo(o, ""S"")
//         -- info.name is nil because o is not a calling level
//         if info.what == ""C"" then
//            return string.format(""%q"", so .. "", C function"")
//         else 
//            -- the information is defined through lines
//            return string.format(""%q"", so .. "", defined in ("" ..
//                info.linedefined .. ""-"" .. info.lastlinedefined ..
//                "")"" .. info.source)
//         end
//      elseif type(o) == ""number"" or type(o) == ""boolean"" then
//         return so
//      else
//         return string.format(""%q"", so)
//      end
//   end

//   local function addtocart (value, name, indent, saved, field)
//      indent = indent or """"
//      saved = saved or {}
//      field = field or name

//      cart = cart .. indent .. field

//      if type(value) ~= ""table"" then
//         cart = cart .. "" = "" .. basicSerialize(value) .. "";\n""
//      else
//         if saved[value] then
//            cart = cart .. "" = {}; -- "" .. saved[value] 
//                        .. "" (self reference)\n""
//            autoref = autoref ..  name .. "" = "" .. saved[value] .. "";\n""
//         else
//            saved[value] = name
//            --if tablecount(value) == 0 then
//            if isemptytable(value) then
//               cart = cart .. "" = {};\n""
//            else
//               cart = cart .. "" = {\n""
//               for k, v in pairs(value) do
//                  k = basicSerialize(k)
//                  local fname = string.format(""%s[%s]"", name, k)
//                  field = string.format(""[%s]"", k)
//                  -- three spaces between levels
//                  addtocart(v, fname, indent .. ""   "", saved, field)
//               end
//               cart = cart .. indent .. ""};\n""
//            end
//         end
//      end
//   end

//   name = name or ""__unnamed__""
//   if type(t) ~= ""table"" then
//      return name .. "" = "" .. basicSerialize(t)
//   end
//   cart, autoref = """", """"
//   addtocart(t, name, indent)
//   return cart .. autoref
//end");

            luaState.DoString(@"local no_identity = { number=1, boolean=1, string=1, ['nil']=1 }

function serialize (x)
   
   local gensym_max =  0  -- index of the gensym() symbol generator
   local seen_once  = { } -- element->true set of elements seen exactly once in the table
   local multiple   = { } -- element->varname set of elements seen more than once
   local nested     = { } -- transient, set of elements currently being traversed
   local nest_points = { }
   local nest_patches = { }
   
   local function gensym()
      gensym_max = gensym_max + 1 ;  return gensym_max
   end
   
   -----------------------------------------------------------------------------
   -- nest_points are places where a table appears within itself, directly or not.
   -- for instance, all of these chunks create nest points in table x:
   -- ""x = { }; x[x] = 1"", ""x = { }; x[1] = x"", ""x = { }; x[1] = { y = { x } }"".
   -- To handle those, two tables are created by mark_nest_point:
   -- * nest_points [parent] associates all keys and values in table parent which
   --   create a nest_point with boolean `true'
   -- * nest_patches contain a list of { parent, key, value } tuples creating
   --   a nest point. They're all dumped after all the other table operations
   --   have been performed.
   --
   -- mark_nest_point (p, k, v) fills tables nest_points and nest_patches with
   -- informations required to remember that key/value (k,v) create a nest point
   -- in table parent. It also marks `parent' as occuring multiple times, since
   -- several references to it will be required in order to patch the nest
   -- points.
   -----------------------------------------------------------------------------
   local function mark_nest_point (parent, k, v)
      local nk, nv = nested[k], nested[v]
      assert (not nk or seen_once[k] or multiple[k])
      assert (not nv or seen_once[v] or multiple[v])
      local mode = (nk and nv and ""kv"") or (nk and ""k"") or (""v"")
      local parent_np = nest_points [parent]
      local pair = { k, v }
      if not parent_np then parent_np = { }; nest_points [parent] = parent_np end
      parent_np [k], parent_np [v] = nk, nv
      table.insert (nest_patches, { parent, k, v })
      seen_once [parent], multiple [parent]  = nil, true
   end
   
   -----------------------------------------------------------------------------
   -- First pass, list the tables and functions which appear more than once in x
   -----------------------------------------------------------------------------
   local function mark_multiple_occurences (x)
      if no_identity [type(x)] then return end
      if     seen_once [x]     then seen_once [x], multiple [x] = nil, true
      elseif multiple  [x]     then -- pass
      else   seen_once [x] = true end
      
      if type (x) == 'table' then
         nested [x] = true
         for k, v in pairs (x) do
            if nested[k] or nested[v] then mark_nest_point (x, k, v) else
               mark_multiple_occurences (k)
               mark_multiple_occurences (v)
            end
         end
         nested [x] = nil
      end
   end

   local dumped    = { } -- multiply occuring values already dumped in localdefs
   local localdefs = { } -- already dumped local definitions as source code lines


   -- mutually recursive functions:
   local dump_val, dump_or_ref_val

   --------------------------------------------------------------------
   -- if x occurs multiple times, dump the local var rather than the
   -- value. If it's the first time it's dumped, also dump the content
   -- in localdefs.
   --------------------------------------------------------------------            
   function dump_or_ref_val (x)
      if nested[x] then return 'false' end -- placeholder for recursive reference
      if not multiple[x] then return dump_val (x) end
      local var = dumped [x]
      if var then return ""_["" .. var .. ""]"" end -- already referenced
      local val = dump_val(x) -- first occurence, create and register reference
      var = gensym()
      table.insert(localdefs, ""_[""..var..""]=""..val)
      dumped [x] = var
      return ""_["" .. var .. ""]""
   end

   -----------------------------------------------------------------------------
   -- Second pass, dump the object; subparts occuring multiple times are dumped
   -- in local variables which can be referenced multiple times;
   -- care is taken to dump locla vars in asensible order.
   -----------------------------------------------------------------------------
   function dump_val(x)
      local  t = type(x)
      if     x==nil        then return 'nil'
      elseif t==""number""   then return tostring(x)
      elseif t==""string""   then return string.format(""%q"", x)
      elseif t==""boolean""  then return x and ""true"" or ""false""
      elseif t==""function"" then
         return string.format (""loadstring(%q,'@serialized')"", string.dump (x))
      elseif t==""table"" then

         local acc        = { }
         local idx_dumped = { }
         local np         = nest_points [x]
         for i, v in ipairs(x) do
            if np and np[v] then
               table.insert (acc, 'false') -- placeholder
            else
               table.insert (acc, dump_or_ref_val(v))
            end
            idx_dumped[i] = true
         end
         for k, v in pairs(x) do
            if np and (np[k] or np[v]) then
               --check_multiple(k); check_multiple(v) -- force dumps in localdefs
            elseif not idx_dumped[k] then
               table.insert (acc, ""["" .. dump_or_ref_val(k) .. ""] = "" .. dump_or_ref_val(v))
            end
         end
         return ""{ ""..table.concat(acc,"", "").."" }""
      else
         error (""Can't serialize data of type ""..t)
      end
   end
          
   local function dump_nest_patches()
      for _, entry in ipairs(nest_patches) do
         local p, k, v = unpack (entry)
         assert (multiple[p])
         local set = dump_or_ref_val (p) .. ""["" .. dump_or_ref_val (k) .. ""] = "" .. 
            dump_or_ref_val (v) .. "" -- rec ""
         table.insert (localdefs, set)
      end
   end
   
   mark_multiple_occurences (x)
   local toplevel = dump_or_ref_val (x)
   dump_nest_patches()

   if next (localdefs) then
      return ""local _={ }\n"" ..
         table.concat (localdefs, ""\n"") .. 
         ""\nreturn "" .. toplevel
   else
      return ""return "" .. toplevel
   end
end");
		}
    }
}
