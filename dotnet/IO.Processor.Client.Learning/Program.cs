using System.IO.Pipes;

namespace ConsoleApp1;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("[Client] Start...");
        using (var client = new NamedPipeClientStream(".", "DemoChannel", PipeDirection.Out))
        {
            Console.WriteLine("[Client] Connecting to server...");
            try
            {
                await client.ConnectAsync(5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Client] Could not connect to server. Is it running?");
                Console.WriteLine("Error msg: {0}", ex.Message);
                return;
            }

            Console.WriteLine("[Client] Connected! Type something and hit Enter.");

            using (var writer = new StreamWriter(client))
            {
                writer.AutoFlush = true;
                while (true)
                {
                    Console.Write(">");
                    string? usrInput = Console.ReadLine();

                    if ("exit".Contains(usrInput, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Shutdown client...");
                        break;
                    }
                    
                    if (!string.IsNullOrEmpty(usrInput))
                    {
                        await writer.WriteLineAsync(usrInput);
                    }
                    else
                    {
                        Console.WriteLine("Empty command will not send to server.");
                    }
                }
            }
        }

        Console.WriteLine("[Client] Client shutdown.");
    }
}