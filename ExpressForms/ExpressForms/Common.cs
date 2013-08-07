using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressForms
{
    public static class Common
    {
        public static string[] PropertiesToIgnore { get { return new[] { "EntityState", "EntityKey" }; } }
    }
}
