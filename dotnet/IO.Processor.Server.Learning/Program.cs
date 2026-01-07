using System.IO.Pipes;

namespace IO.Processor.Server.Learning;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("[Server] Starting server...");

        using (var server = new NamedPipeServerStream("DemoChannel", PipeDirection.In))
        {
            Console.WriteLine("[Server] Waiting for client connection...");

            // wait for a client to connect - blocking call
            await server.WaitForConnectionAsync();

            Console.WriteLine("[Server] Client connected! Ready to receive messages.");

            using (var reader = new StreamReader(server))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine($"[Server] Received: {line}");
                    }
                }
            }
        }
        Console.WriteLine("[Server] Client disconnected. Exiting.");
    }
}