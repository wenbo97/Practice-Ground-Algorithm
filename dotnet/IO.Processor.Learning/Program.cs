using System.Diagnostics;

namespace IO.Processor.Learning;

public class Program
{

    public static async Task RunShell()
    {
        Process session = new Process();
        
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/k rem SESSION_ID:9981_DATABASE_WORKER";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.CreateNoWindow = true;
        
        session.StartInfo = startInfo;

        session.OutputDataReceived += (sender, eventArg) =>
        {
            if (!string.IsNullOrEmpty(eventArg.Data))
            {
                Console.WriteLine(eventArg.Data);
            }
        };
        
        session.ErrorDataReceived += (sender, eventArg) =>
        {
            if (!string.IsNullOrEmpty(eventArg.Data))
            {
                Console.WriteLine(eventArg.Data);
            }
        };

        session.Start();
        
        session.BeginOutputReadLine();
        session.BeginErrorReadLine();

        StreamWriter inputWriter = session.StandardInput;
        await session.StandardInput.WriteLineAsync("title MyCriticalTask");
        
        Console.WriteLine("CMD Session Started. Type commands (e.g., 'dir', 'ipconfig'). Type 'exit' to quit.");

        while (true)
        {
            string userCommand = Console.ReadLine();

            if (userCommand == "exit")
            {
                await inputWriter.WriteLineAsync("exit");
                break;
            }

            await inputWriter.WriteLineAsync(userCommand);
        }

        await session.WaitForExitAsync();
        Console.WriteLine("CMD session End.");
    }

    public static void RunUACShell()
    {
        var info = new ProcessStartInfo("cmd.exe");
        info.UseShellExecute = true;
        info.Verb = "runas"; // Triggers UAC prompt
        Process.Start(info);
    }
    
    public static void ShowVerbs(string fileName)
    {
        ProcessStartInfo info = new ProcessStartInfo(fileName);
        
        // UseShellExecute must be true to inspect Verbs
        // But we don't start it yet, just inspecting.
        
        // Note: You usually need to instantiate the Process object to read .StartInfo.Verbs
        // correctly after associating a file.
        Process p = new Process();
        p.StartInfo = info;

        Console.WriteLine($"Verbs available for '{fileName}':");
        
        // .Verbs property returns an array of strings
        foreach (string verb in p.StartInfo.Verbs)
        {
            Console.WriteLine($" - {verb}");
        }
    }
    
    public static async Task Main(string[] args)
    {
        // RunUACShell();
        ShowVerbs("D:\\A_Trainings\\Practice-Ground-Algorithm\\docs\\My Basic of CS\\IO & Networking\\Standard IO Operation\\standard-io-operation.md");
    }
}