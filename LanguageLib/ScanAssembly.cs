using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
namespace LanguageLib
{
   public class ScanAssembly
    {
        public static void Add(string file)
        {
          var asm= Assembly.LoadFile(file);
          var types=asm.DefinedTypes.Where(X=>X.is)
        }
    }
}
