using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Configuration;
using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Constants;
using WeakAuraManager.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WeakAuraManager
{
    internal class FileGenerator : IFileGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IGroupBuilder _groupBuilder;

        private const int XBuffer = 8;
        private const int DebuffExtraBuffer = 18;
        private const int YBuffer = 14;

        private static BorderDetails BuffBorder = new BorderDetails
        {
            AuraColor = new AuraColor
            {
                Red = 0,
                Green = 1,
                Blue = 0,
                Alpha = 1
            },
            BorderSize = 2
        };

        private static BorderDetails DebuffBorder = new BorderDetails
        {
            AuraColor = new AuraColor
            {
                Red = 1,
                Green = 0,
                Blue = 0,
                Alpha = 1
            },
            BorderSize = 2
        };

        public FileGenerator(IConfiguration configuration, IGroupBuilder groupBuilder)
        {
            _configuration = configuration;
            _groupBuilder = groupBuilder;
        }

		public async Task<string> GenerateFileContents(string initialLuaContent, IEnumerable<BaseSpell> spells)
		{
			try
			{
				string groupName = GetGroupPrefix();

				Lua state = new Lua();

				state.DoString(initialLuaContent);

                var weakAurasTable = GetTableByPath(new List<string> { "WeakAurasSaved", "displays" }, state);

                var prefix = _configuration.GetValue<string>(ConfigKeys.GroupPrefix);
                var existingKeys = weakAurasTable.Keys.Cast<string>().Where(k => k.StartsWith(prefix));

                foreach (var key in existingKeys)
                {
                    weakAurasTable[key] = null;
                }

                Console.WriteLine($"Deleted existing {existingKeys.Count()} buffs starting with '{prefix}'");

				this.AddGroups(state, spells);

				AddLuaHelpers(state);

				var finalFileContent = Serialize(state, "WeakAurasSaved");

				return "WeakAurasSaved = " + finalFileContent;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		private void AddGroups(Lua state, IEnumerable<BaseSpell> allSpells)
		{
            int defaultSize = _configuration.GetValue<int>(ConfigKeys.DefaultSize);
            var groupPrefix = _configuration.GetValue<string>(ConfigKeys.GroupPrefix);

            // Buffs 

            AddGroup(
				state, 
				this.GetBuffs(allSpells, defaultSize), 
                new GroupBuilder.GroupBuilderParameters
                {
                    GroupName = $"{groupPrefix}/Buffs/Party",
                    SpellUnit = Units.Party,
                    XOffset = XBuffer,
                    YOffset = -10,
                    DefaultSize = defaultSize,
                    FrameType = GroupBuilder.FrameType.Party,
                    AnchorPoint = AnchorPoints.TopRight,
                    SelfAnchor = AnchorPoints.TopLeft,
                    BorderColor = BuffBorder.AuraColor,
                    BorderThickness = BuffBorder.BorderSize,
                    UseBorder = true
                }
			);

            AddGroup(
                state,
                this.GetBuffs(allSpells, defaultSize),
                new GroupBuilder.GroupBuilderParameters
                {
                    GroupName = $"{groupPrefix}/Buffs/ArenaFriendlies",
                    SpellUnit = Units.Party,
                    XOffset = XBuffer,
                    YOffset = -10,
                    DefaultSize = defaultSize,
                    FrameType = GroupBuilder.FrameType.ArenaFriendlies,
                    AnchorPoint = AnchorPoints.TopRight,
                    SelfAnchor = AnchorPoints.TopLeft,
                    BorderColor = BuffBorder.AuraColor,
                    BorderThickness = BuffBorder.BorderSize,
                    UseBorder = true
                }
            );

            AddGroup(
                state,
                this.GetBuffs(allSpells, defaultSize),
                new GroupBuilder.GroupBuilderParameters
                {
                    GroupName = $"{groupPrefix}/Buffs/ArenaEnemies",
                    SpellUnit = Units.Arena,
                    XOffset = XBuffer,
                    YOffset = -10,
                    DefaultSize = defaultSize,
                    FrameType = GroupBuilder.FrameType.ArenaEnemies,
                    AnchorPoint = AnchorPoints.TopRight,
                    SelfAnchor = AnchorPoints.TopLeft,
                    BorderColor = BuffBorder.AuraColor,
                    BorderThickness = BuffBorder.BorderSize,
                    UseBorder = true
                }
            );

            if (_configuration.GetValue<bool>(ConfigKeys.AddRaidBuffs))
            {
                int raidDefaultSize = _configuration.GetValue<int>(ConfigKeys.RaidDefaultSize);

                AddGroup(
                    state,
                    this.GetRaidBuffs(allSpells, raidDefaultSize),
                    new GroupBuilder.GroupBuilderParameters
                    {
                        GroupName = $"{groupPrefix}/Buffs/Raid",
                        SpellUnit = Units.Raid,
                        XOffset = -5,
                        YOffset = 5,
                        DefaultSize = raidDefaultSize,
                        FrameType = GroupBuilder.FrameType.Raid,
                        ShowText = false,
                        AnchorPoint = AnchorPoints.BottomRight,
                        SelfAnchor = AnchorPoints.BottomRight,
                        GrowDirection = GrowDirections.Left,
                        BorderColor = BuffBorder.AuraColor,
                        BorderThickness = BuffBorder.BorderSize,
                        UseBorder = true
                    }
                );
            }

            // Debuffs

            AddGroup( 
				state, 
				this.GetDebuffs(allSpells, defaultSize),
                new GroupBuilder.GroupBuilderParameters
                {
                    GroupName = $"{groupPrefix}/Debuffs/Party",
                    SpellUnit = Units.Party,
                    XOffset = XBuffer + DebuffExtraBuffer,
                    YOffset = -YBuffer-defaultSize,
                    DefaultSize = defaultSize,
                    FrameType = GroupBuilder.FrameType.Party,
                    AnchorPoint = AnchorPoints.TopRight,
                    SelfAnchor = AnchorPoints.TopLeft,
                    BorderColor = DebuffBorder.AuraColor,
                    BorderThickness = DebuffBorder.BorderSize,
                    UseBorder = true
                }
            );

            // target debuffs - fixme debug
            AddGroup(
                state,
                this.GetDebuffs(allSpells, defaultSize),
                new GroupBuilder.GroupBuilderParameters
                {
                    GroupName = $"{groupPrefix}/Debuffs/Target",
                    SpellUnit = Units.Target,
                    XOffset = XBuffer,
                    YOffset = -10 - YBuffer,
                    DefaultSize = defaultSize,
                    FrameType = GroupBuilder.FrameType.Party,
                    AnchorPoint = AnchorPoints.TopRight,
                    SelfAnchor = AnchorPoints.TopLeft,
                    BorderColor = DebuffBorder.AuraColor,
                    BorderThickness = DebuffBorder.BorderSize,
                    UseBorder = true
                }
            );

            AddGroup(
                state,
                this.GetDebuffs(allSpells, defaultSize),
                new GroupBuilder.GroupBuilderParameters
                {
                    GroupName = $"{groupPrefix}/Debuffs/ArenaFriendlies",
                    SpellUnit = Units.Party,
                    XOffset = XBuffer + DebuffExtraBuffer,
                    YOffset = -10 - YBuffer,
                    DefaultSize = defaultSize,
                    FrameType = GroupBuilder.FrameType.ArenaFriendlies,
                    AnchorPoint = AnchorPoints.TopRight,
                    SelfAnchor = AnchorPoints.TopLeft,
                    BorderColor = DebuffBorder.AuraColor,
                    BorderThickness = DebuffBorder.BorderSize,
                    UseBorder = true
                }
            );

            AddGroup(
                state,
                this.GetDebuffs(allSpells, defaultSize),
                new GroupBuilder.GroupBuilderParameters
                {
                    GroupName = $"{groupPrefix}/Debuffs/ArenaEnemies",
                    SpellUnit = Units.Arena,
                    XOffset = XBuffer,
                    YOffset = -10- YBuffer,
                    DefaultSize = defaultSize,
                    FrameType = GroupBuilder.FrameType.ArenaEnemies,
                    AnchorPoint = AnchorPoints.TopRight,
                    SelfAnchor = AnchorPoints.TopLeft,
                    BorderColor = DebuffBorder.AuraColor,
                    BorderThickness = DebuffBorder.BorderSize,
                    UseBorder = true
                }
            );
        }

        private IEnumerable<BaseSpell> GetDebuffs(IEnumerable<BaseSpell> all, int defaultSize)
        {
            var debuffs = all.Where(s => s.SpellType == SpellType.EnemyDebuff)
                .Concat(new List<BaseSpell> { new NpcDebuffSpell(defaultSize) });

            if (_configuration.GetValue<bool>(ConfigKeys.AddTestAnchor))
            {
                debuffs = debuffs.Concat(new List<BaseSpell> { new TestAnchorSpell(defaultSize) });
            }

            return debuffs;
        }

        private IEnumerable<BaseSpell> GetBuffs(IEnumerable<BaseSpell> all, int defaultSize)
        {
            var buffs = all.Where(s => s.SpellType == SpellType.SelfBuff || s.SpellType == SpellType.TriggerTimed);

            if (_configuration.GetValue<bool>(ConfigKeys.AddTestAnchor))
            {
                buffs = buffs.Concat(new List<BaseSpell> { new TestAnchorSpell(defaultSize) });
            }

            return buffs;
        }

        private IEnumerable<BaseSpell> GetRaidBuffs(IEnumerable<BaseSpell> all, int baseSize)
        {
            return this.GetBuffs(all, baseSize).Where(x => x.ShowInRaid);
        }


        private void AddGroup(Lua state, IEnumerable<BaseSpell> spells, GroupBuilder.GroupBuilderParameters groupParams)
		{
            var displaysTable = (state["WeakAurasSaved"] as LuaTable)["displays"] as LuaTable;

            if (displaysTable.Keys.Cast<string>().Any(k => k == groupParams.GroupName))
            {
                throw new Exception($"WeakAuras file already contains group name '{groupParams.GroupName}'");
            }

            var spellDefinitions = spells.OrderByDescending(s => s.Priority).Select(s => new { original = s, text = s.GetWeakaura(groupParams) });

            foreach (var spellDefinition in spellDefinitions)
            {
                InsertLuaText(spellDefinition.text, state, $"WeakAurasSaved.displays[\"{spellDefinition.original.GetWeakauraName(groupParams.GroupName)}\"]");
            }

            var groupText = _groupBuilder.GetGroupText(spellDefinitions.Select(s => s.original), groupParams);

            InsertLuaText(groupText, state, $"WeakAurasSaved.displays[\"{groupParams.GroupName}\"]");
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

        private string GetGroupPrefix() => _configuration.GetValue<string>(ConfigKeys.GroupPrefix);

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

        private class BorderDetails
        {
            public int BorderSize { get; set; }
            public AuraColor AuraColor { get; set; }
        }
    }
}
