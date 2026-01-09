using System.Diagnostics;
using System.IO.Pipes;
using System.Text;

namespace IO.Processor.Server.Learning;

public class Program
{

    private static StreamWriter pipelineWriter;

    public static async Task Main(string[] args)
    {
        StartSimpleServer();
    }

    public static async Task StartSimpleServer()
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

                byte[] handShakeBuffer = new byte[] { 1 };

                await server.WriteAsync(handShakeBuffer, 0, 1);
                await server.FlushAsync();

                byte[] clientAck = new byte[1];

                int clientAckByte = await server.ReadAsync(clientAck, 0, 1);

                if (clientAckByte == 0 || clientAck[0] != 2)
                {
                    Console.Error.WriteLine("[Server] Handshake failed. Closing.");
                    return;
                }

                Console.WriteLine("[Server] Handshake success. Start processing.");

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