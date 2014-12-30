using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    static class Logger
    {
        private static List<string> history = new List<string>();
        public static void Log(string format, params object[] args)
        {
            history.Add(string.Format(format, args));
        }

        public static void drawLog(Camera root, int last = 5)
        {
            for (int i = 0; i < last && i < history.Count; ++i)
            {
                root.print(1, i, history[i]);
            }
        }
    }
}
