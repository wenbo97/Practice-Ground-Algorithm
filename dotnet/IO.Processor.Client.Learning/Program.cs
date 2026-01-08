using System.IO.Pipes;
using System.Text;

namespace IO.Processor.Client.Learning;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("[Client] Start...");
        using (var client = new NamedPipeClientStream(".", "DemoChannel", PipeDirection.InOut, PipeOptions.Asynchronous))
        {
            Console.WriteLine("[Client] Connecting to server...");
            try
            {
                await client.ConnectAsync(30 *1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Client] Could not connect to server. Is it running?");
                Console.WriteLine("Error msg: {0}", ex.Message);
                return;
            }

            //Console.WriteLine("[Client] Performing handshake...");
            //int serverSignal = client.ReadByte();
            //client.WriteByte(2);
            //await client.FlushAsync();
            //Console.WriteLine($"[Client] Handshake complete (Server Signal: {serverSignal})");

            var noBomUtf8 = new UTF8Encoding(false);
            using var reader = new StreamReader(client, noBomUtf8, false, 1024, leaveOpen: true);
            using StreamWriter writer = new StreamWriter(client, noBomUtf8, 1024, leaveOpen: true)
            {
                AutoFlush = true
            };
            var listeningTask = Task.Run(async () =>
            {

                while (client.IsConnected)
                {
                    try
                    {
                        var line = await reader.ReadLineAsync();
                        if (!string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine($"[Client] {line}");
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            });




            await writer.WriteLineAsync("echo hello");
            while (true)
            {
                Console.Write("> ");

                string? usrInput = Console.ReadLine();
                if (string.Equals(usrInput, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Shutdown client...");
                    break;
                }

                if (!string.IsNullOrEmpty(usrInput))
                {
                    try
                    {
                        await writer.WriteLineAsync(usrInput);
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("[Client] Server disconnected.");
                        break;
                    }
                }

            }

        }

        Console.WriteLine("[Client] Client shutdown.");
    }
}