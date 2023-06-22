using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeakAuraManager.Models;

namespace WeakAuraManager
{
    internal interface IParser
    {
        Task<IEnumerable<SpellModel>> Parse();
    }
}
