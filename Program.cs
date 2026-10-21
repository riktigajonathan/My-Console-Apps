using System;
using System.IO;
using System.Diagnostics;

// 1. Define the root folder containing your projects
// (AppDomain.CurrentDomain.BaseDirectory gets the bin folder, so we navigate up to the solution/parent root)
string rootFolder = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName
                    ?? AppDomain.CurrentDomain.BaseDirectory;

Console.WriteLine($"Scanning root folder: {rootFolder}\n");

// 2. Find all .csproj files recursively (excluding the parent project itself if needed)
string parentProjectName = "YourParentProjectName"; // Replace with your parent project's name or folder
string[] csprojFiles = Directory.GetFiles(rootFolder, "*.csproj", SearchOption.AllDirectories);

foreach (string csprojPath in csprojFiles)
{
    string projectName = Path.GetFileNameWithoutExtension(csprojPath);
    string projectFolder = Path.GetDirectoryName(csprojPath)!;

    // Skip the parent project itself if it's located within the same directory tree
    if (projectName.Equals(parentProjectName, StringComparison.OrdinalIgnoreCase))
    {
        continue;
    }

    Console.WriteLine($"========================================");
    Console.WriteLine($"Processing Project: {projectName}");
    Console.WriteLine($"Path: {projectFolder}");
    Console.WriteLine($"========================================");

    // 3. Build the project using 'dotnet build'
    bool buildSuccess = RunCommand("dotnet", $"build \"{csprojPath}\" -c Release");

    if (buildSuccess)
    {
        Console.WriteLine($"[Success] Built {projectName}");

        // 4. Optionally run the project using 'dotnet run' (--no-build since we just built it)
        // Uncomment the line below if you want them to launch automatically:
        // RunCommandAsync("dotnet", $"run --project \"{csprojPath}\" --no-build -c Release");
    }
    else
    {
        Console.WriteLine($"[Error] Failed to build {projectName}");
    }

    Console.WriteLine();
}

Console.WriteLine("All projects processed.");

// --- Helper Method to Run CLI Commands Synchronously ---
bool RunCommand(string fileName, string arguments)
{
    var startInfo = new ProcessStartInfo
    {
        FileName = fileName,
        Arguments = arguments,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using var process = Process.Start(startInfo);
    if (process == null) return false;

    // Read output (optional: print to console for debugging)
    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();

    process.WaitForExit();

    if (!string.IsNullOrEmpty(error) && process.ExitCode != 0)
    {
        Console.WriteLine(error);
    }

    return process.ExitCode == 0;
}