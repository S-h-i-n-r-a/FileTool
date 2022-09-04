using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: messy

namespace FileTool
{
    internal static class Config
    {
        private static string execDir;
        internal static bool failed;

        // Directories
        internal static string? srcDir;
        internal static string? tgtDir;

        // File Modifications
        internal static string? tgtFileT;
        internal static string? prefix;
        internal static string? suffix;

        // Repeat Operations
        internal static bool repeat;
        internal static int pauseMillis;

        static Config()
        {
            execDir = AppDomain.CurrentDomain.BaseDirectory;
            failed = false;
            
            if (!File.Exists(execDir + "\\FileTool.cfg")) { Console.WriteLine("ERROR: no config file found."); failed = true; return; }


            // Read cfg file
            StreamReader sr = new(execDir + "\\FileTool.cfg");

            sr.ReadLine();
            srcDir = sr.ReadLine();
            if (srcDir == null || srcDir == "\"\"") { Console.WriteLine("ERROR: source directory cannot be empty."); failed = true; return; }
            srcDir = srcDir.Replace("\"", "");

            sr.ReadLine(); sr.ReadLine();
            tgtDir = sr.ReadLine();
            if (tgtDir == null || tgtDir == "\"\"") { Console.WriteLine("ERROR: target directory cannot be empty."); failed = true; return; }
            tgtDir = tgtDir.Replace("\"", "");

            sr.ReadLine(); sr.ReadLine();
            tgtFileT = sr.ReadLine();
            if (tgtFileT != null) { tgtFileT = tgtFileT.Replace("\"", ""); }
            else { tgtFileT = ".*"; }

            sr.ReadLine(); sr.ReadLine();
            prefix = sr.ReadLine();
            if (prefix != null) { prefix = prefix.Replace("\"", ""); }

            sr.ReadLine(); sr.ReadLine();
            suffix = sr.ReadLine();
            if (suffix != null) { suffix = suffix.Replace("\"", ""); }

            sr.ReadLine(); sr.ReadLine();
            if (!bool.TryParse(sr.ReadLine(), out repeat)) 
            { Console.WriteLine("WARNING: Invalid value @ repeat, fallback to false."); repeat = false; }

            sr.ReadLine(); sr.ReadLine();
            if (!int.TryParse(sr.ReadLine(), out pauseMillis))
            { Console.WriteLine("WARNING: Invalid value @ interval, fallback to 10 minutes."); pauseMillis = 600000; }
            else { pauseMillis *= 1000; }
        }
    }
}
