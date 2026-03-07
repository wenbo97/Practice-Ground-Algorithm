
using System.Diagnostics;
using System.IO.Pipes;

namespace AnonymousPipeServer;

public class Program
{
    public static void Main(string[] args)
    {
        StartAnonumousServer();
    }

    private static void StartAnonumousServer()
    {
        using var pipeServer = new AnonymousPipeServerStream(
            PipeDirection.Out,
            HandleInheritability.Inheritable);

        Console.WriteLine("[Server] Pipe created.");

        string pipeHanldeStr = pipeServer.GetClientHandleAsString();

        Console.WriteLine($"[Server] Hanlde ID is: {pipeHanldeStr}");

        var childProcess = new Process();

        childProcess.StartInfo.FileName = "AnonymousPipeClient.exe";
        childProcess.StartInfo.Arguments = pipeHanldeStr;
        childProcess.StartInfo.UseShellExecute = false;
        childProcess.StartInfo.CreateNoWindow = false;

        Console.WriteLine("[Server] Starting child process...");
        childProcess.Start();

        //pipeServer.DisposeLocalCopyOfClientHandle();

        using (var writer = new StreamWriter(pipeServer))
        {
            writer.AutoFlush = true;

            Console.WriteLine("[Server] sending message...");

            writer.WriteLine("[Server] Hello child process, this is server speaking.");
            
            writer.WriteLine("[Server] This is the second line sent from server side....");
            writer.Close();
            childProcess.WaitForExit();
        }

        Console.WriteLine("[Server] Done.");
    }
}
