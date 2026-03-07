
using System.IO.Pipes;

namespace AnonymousPipeClient;

public class Program
{
    public static void Main(string[] args)
    {
        StartAnonymousClient(args);
    }

    private static void StartAnonymousClient(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("[Client] Error: No Handle provided.");
            return;
        }

        string pipeHandleId = args[0];
        Console.WriteLine($"[Client] received handle id [{pipeHandleId}]");

        try
        {
            using var pipeClient = new AnonymousPipeClientStream(
                PipeDirection.In,
                pipeHandleId);

            using var reader = new StreamReader(pipeClient);

            string? message;

            while ((message = reader.ReadLine()) != null)
            {
                Console.WriteLine($"[Client] Message received from server: {message}");
            }

            Console.WriteLine("[Client] Server closed the pipe. exiting...");

            // Task.Delay(TimeSpan.FromSeconds(15));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Client] Error: {ex.Message}");
        }
    }
}
