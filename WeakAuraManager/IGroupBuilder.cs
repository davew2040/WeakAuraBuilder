using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Models;

namespace WeakAuraManager
{
    public interface IGroupBuilder
    {
        string GetGroupText(IEnumerable<BaseSpell> spells, GroupBuilder.GroupBuilderParameters groupParams);
    }
}
