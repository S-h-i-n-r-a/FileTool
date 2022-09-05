namespace FileTool
{
    internal class Program
    {
        private const string authLogo =
            "██████████████████████████████████████████████████\n" +
            "██░░░░░░██░░██░░██░░░░░░██░░░░░░██░░░░░░██░░░░░░██\n" +
            "██░░██████░░██░░████░░████░░██░░██░░██░░██░░██░░██\n" +
            "██░░░░░░██░░░░░░████░░████░░██░░██░░░░████░░░░░░██\n" +
            "██████░░██░░██░░████░░████░░██░░██░░██░░██░░██░░██\n" +
            "██░░░░░░██░░██░░██░░░░░░██░░██░░██░░██░░██░░██░░██\n" +
            "██████████████████████████████████████████████████\n" +
            "██░░░░░░░░░░░ github.com/S-h-i-n-r-a ░░░░░░░░░░░██\n" +
            "██████████████████████████████████████████████████";

        static void Main(string[] args)
        {
            Console.WriteLine(authLogo + "\n\nFileTool starting..");
            
            // Calling Config.failed automatically loads the config
            if (Config.failed) { Console.WriteLine("Failed to read config, check the file and restart.\nPress enter to exit."); Console.ReadLine(); return; }

            Console.WriteLine("\nDone!\n");

            if (Config.repeat)
            {
                Console.WriteLine("Running in repeat mode..\nEnter \"exit\" command at any time to quit.");
                Thread thread = new(RepeatManipFiles);
                thread.Start();

                string? command = "";
                while (!command.Equals("exit")) { command = Console.ReadLine() ?? ""; }
                thread.Interrupt();
                while (thread.IsAlive) { /* Wait for thread to exit */ }
            }
            else
            {
                ManipulateFiles();
                Console.WriteLine("Done!");
            }

            Console.WriteLine("Exiting..");
        }


#pragma warning disable CS8604  // Checks are performed in Config, null values never reach this section
        private static void ManipulateFiles()
        {
            Console.WriteLine("\nLooking for files..\n");
            string[] files = Directory.GetFiles(Config.srcDir, Config.tgtFileT);
            if (files.Length == 0) { Console.WriteLine("No new files found.\n"); return; }
            for (int i = 0; i < files.Length; ++i) { Console.WriteLine(files[i]); }

            Console.WriteLine("\nApplying transformations..\n");
            if (!Directory.Exists(Config.tgtDir)) { Directory.CreateDirectory(Config.tgtDir); }
            for (int i = 0; i < files.Length; ++i)
            {
                string filename = files[i].Substring(files[i].LastIndexOf('\\') +1, files[i].LastIndexOf('.') - files[i].LastIndexOf('\\') -1);
                string filetype = files[i].Substring(files[i].LastIndexOf('.') +1);

                string transformedFile = $"{Config.tgtDir}\\{Config.prefix + filename + Config.suffix}.{filetype}";
                try 
                { 
                    File.Copy(files[i], transformedFile);
                    File.Delete(files[i]);
                    Console.WriteLine(transformedFile); 
                }
                catch (Exception e) { Console.WriteLine($"ERROR: {e}"); }
            }
            Console.WriteLine();
        }
#pragma warning restore CS8604

        private static void RepeatManipFiles()
        {
            ManipulateFiles();
            Console.WriteLine($"Waiting for {Config.pauseMillis / 1000} seconds..\n");
            try { Thread.Sleep(Config.pauseMillis); }
            catch { Console.WriteLine("INFO: Thread interrupted while in SLEEP state."); return; }
            RepeatManipFiles();
        }
    }
}