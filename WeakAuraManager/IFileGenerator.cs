using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Models;

namespace WeakAuraManager
{
    internal interface IFileGenerator
    {
        Task<string> GenerateFileContents(string luaContent, IEnumerable<BaseSpell> spells);
    }
}
