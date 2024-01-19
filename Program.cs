using System;
using System.Diagnostics;
using System.IO;

namespace ffxivboot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Search FINAL FANTASY XIV");

            // Default path
            string defaultPath = @"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn\boot";
            string configFile = "config.txt";
            string launcherPath;

            if (File.Exists(configFile))
            {
                launcherPath = File.ReadAllText(configFile);
            }
            else
            {
                launcherPath = defaultPath;
            }

            if (!IsValidPath(launcherPath))
            {
                Console.WriteLine($"Invalid path or ffxivboot.exe not found at the default path: {launcherPath}.");

                launcherPath = PromptUserForPath();

                if (!IsValidPath(launcherPath))
                {
                    Console.WriteLine("Invalid path. Exiting program.");
                    Console.WriteLine("Press any key to close...");
                    Console.ReadKey();
                    return;
                }

                File.WriteAllText(configFile, launcherPath);
            }

            try
            {
                string fullPath = Path.Combine(launcherPath, "ffxivboot.exe");
                Console.WriteLine($"Launching: {fullPath}");

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = fullPath,
                    WorkingDirectory = launcherPath,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    Console.WriteLine(process.ExitCode == 0 ? "ffxivboot found" : "Error launching ffxivboot");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }

        static bool IsValidPath(string path)
        {
            return File.Exists(Path.Combine(path, "ffxivboot.exe"));
        }

        static string PromptUserForPath()
        {
            Console.WriteLine("Please enter the path to the folder containing FFXIV launcher (ffxivboot.exe):");
            return Console.ReadLine();
        }
    }
}
