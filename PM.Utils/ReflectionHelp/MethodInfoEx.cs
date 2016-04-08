using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PM.Utils.ReflectionHelp
{
    public class MethodInfoEx
    {
        public System.Reflection.MethodInfo MethodInfo { get; set; }
        public ParameterInfo[] ParameterInfos { get; set; }
    }
}
