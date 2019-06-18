using System;
using Language.Convert;
using Language.Util;
using Newtonsoft.Json;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.d(JsonConvert.SerializeObject (args), ConsoleColor.DarkBlue);
            Log.d($"执行目录: {System.IO.Directory.GetCurrentDirectory()}", ConsoleColor.DarkBlue);

            // if (args.Length == 0)
            // {
            //     args = new string [] { 
            //         "-src","C:\\Users\\tsuix\\Documents\\GitHub\\languageExport\\excel", 
            //         // "-out","C:\\Users\\tsuix\\Documents\\GitHub\\languageExport\\output", 
            //         "-out", "C:\\Users\\tsuix\\Documents\\GitHub\\GuessSong\\Assets\\Scripts\\Song\\Language\\Configs"
            //         };
            // }

            if(args.Length > 0)
            {
                AppArgs appArgs = new AppArgs(args);
                ConvertCtl ctl = new ConvertCtl(appArgs, new ConvertLanguageHandle());
                ctl.Start();
            }
            else
            {
                Log.d (AppArgs.ArgTips(), ConsoleColor.DarkGreen);
            }
        }
    }
}
