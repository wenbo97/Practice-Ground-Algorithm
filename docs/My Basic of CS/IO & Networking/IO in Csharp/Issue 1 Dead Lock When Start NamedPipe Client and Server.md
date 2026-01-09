**Client Code*
```csharp
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

            var listeningTask = Task.Run(async () =>
            {
                using var reader = new StreamReader(client, noBomUtf8, false, 1024, leaveOpen: true);

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


            using StreamWriter writer = new StreamWriter(client, noBomUtf8, 1024, leaveOpen: true)
            {
                AutoFlush = true
            };

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
```

**Server Code**
```csharp
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;

namespace IO.Processor.Server.Learning;

public class Program
{

    private static StreamWriter pipelineWriter;

    public static async Task Main(string[] args)
    {
        Console.WriteLine("[Server] Starting server...");

        using (var server = new NamedPipeServerStream(
            "DemoChannel",
            PipeDirection.InOut, 1,
            PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous))
        {
            Console.WriteLine("[Server] Waiting for client connection...");

            // wait for a client to connect - blocking call
            await server.WaitForConnectionAsync();

            //Console.WriteLine("[Server] Client connected! Ready to receive messages.");
            //server.WriteByte(1);
            //await server.FlushAsync();
            //int clientSignal = server.ReadByte();
            //Console.WriteLine($"[Server] Handshake complete (Client Signal: {clientSignal})");

            var noBomUtf8 = new UTF8Encoding(false);
            pipelineWriter = new StreamWriter(server, noBomUtf8, 1024, leaveOpen: true) { AutoFlush = true };
            using (var session = CreateSession())
            {
                Console.WriteLine("[Server] Aim to initialize a new cmd session...");

                session.Start();
                session.BeginOutputReadLine();
                session.BeginErrorReadLine();
                StreamWriter cmdWriter = session.StandardInput;
                cmdWriter.AutoFlush = true;

                Console.WriteLine("[Server] CMD Session Started.");

                using (var reader = new StreamReader(server))
                {
                    while (server.IsConnected)
                    {
                        try
                        {
                            var line = await reader.ReadLineAsync();
                            if (!string.IsNullOrEmpty(line))
                            {
                                Console.WriteLine($"[Server] Received Command: {line}");
                                await cmdWriter.WriteLineAsync(line);
                            }
                        }
                        catch (IOException)
                        {
                            break;
                        }

                    }
                }
            }
        }
        Console.WriteLine("[Server] Client disconnected. Exiting.");
    }

    private static Process CreateSession()
    {
        Process cmdSession = new Process();

        cmdSession.StartInfo.FileName = "cmd.exe";
        cmdSession.StartInfo.Arguments = "/D /K";
        cmdSession.StartInfo.UseShellExecute = false;
        cmdSession.StartInfo.RedirectStandardError = true;
        cmdSession.StartInfo.RedirectStandardInput = true;
        cmdSession.StartInfo.RedirectStandardOutput = true;
        cmdSession.StartInfo.CreateNoWindow = true;

        cmdSession.OutputDataReceived += (s, d) => ForwardToPipe("OUT", d.Data);
        cmdSession.ErrorDataReceived += (s, d) => ForwardToPipe("ERR", d.Data);

        return cmdSession;
    }


    private static void ForwardToPipe(string type, string? data)
    {
        if (string.IsNullOrEmpty(data)) return;

        // debug
        Console.WriteLine($"[CMD->Pipe][{type}] {data}");

        // send back to Client
        try
        {
            if (pipelineWriter != null && pipelineWriter.BaseStream.CanWrite)
            {
                pipelineWriter.WriteLine($"[{type}] - {data}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error Writing to Pipe] {ex.Message}");
        }
    }
}
```

