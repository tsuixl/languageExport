using System;

namespace Language.Util
{
    public static class Log
    {
        public static bool IsJenkins { get; set; }

        public static void d(object o)
        {
            _log(ConsoleColor.White, o);
        }


        public static void d(object o, ConsoleColor c)
        {
            _log(c, o);
        }


        public static void w(object o)
        {
            _log(ConsoleColor.Yellow, o);
        }


        public static void e(object o, bool kill = false)
        {
            _log(ConsoleColor.Red, o);
            if (kill)
                SystemUtil.Kill(o);
        }


        public static void _log(ConsoleColor c, object o)
        {
            Console.ForegroundColor = c;
            if (IsJenkins)
            {
                Console.WriteLine(string.Format(GetJenkinsColor(Console.ForegroundColor), o));
            }
            else
                Console.WriteLine(o);
            Console.ForegroundColor = ConsoleColor.White;
        }


        public static string GetJenkinsColor(ConsoleColor color)
        {
            int c = 37;
            if (color == ConsoleColor.Red)
            {
                c = 31;
            }
            else if (color == ConsoleColor.Yellow)
            {
                c = 33;
            }
            else if (color == ConsoleColor.Green)
            {
                c = 32;
            }
            else if (color == ConsoleColor.Blue)
            {
                c = 34;
            }
            else
            {
                c = 37;
            }

            return string.Format("\u001b[{0}m{{0}}\u001b[0m", c);
        }


    }
}

