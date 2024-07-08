using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static UdpClient udpClient;
    static bool isBroadcasting = false;
    static bool isListening = true;
    static int broadcastPort = 3000; // Port for broadcasting

    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting UDP Server (.NET Console App)");

        StartListening();

        while (true)
        {
            Console.WriteLine("Press 'S' to stop broadcasting, or 'Q' to quit.");
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.S)
            {
                StartListening();
            }
            else if (key == ConsoleKey.Q)
            {
                StopListening();
                break;
            }
        }
    }

    static async void StartBroadcasting()
    {
        if (!isBroadcasting)
        {
            isBroadcasting = true;
            udpClient = new UdpClient();
            string message = $"Service: example_service, Port: {broadcastPort}";
            byte[] data = Encoding.UTF8.GetBytes(message);

            try
            {
                while (isBroadcasting)
                {
                    await udpClient.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Broadcast, broadcastPort));
                    Console.WriteLine($"Broadcasting: {message}");
                    await Task.Delay(5000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while broadcasting: {e.Message}");
            }
            finally
            {
                udpClient.Close();
                Console.WriteLine("Stopped broadcasting.");
            }
        }
    }

    static void StopBroadcasting()
    {
        isBroadcasting = false;
        udpClient?.Close();
        Console.WriteLine("Stopping broadcasting...");
    }

    static async void StartListening()
    {
        using UdpClient listener = new UdpClient(broadcastPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, broadcastPort);

        try
        {
            while (isListening)
            {
                UdpReceiveResult result = await listener.ReceiveAsync();
                string message = Encoding.UTF8.GetString(result.Buffer);
                Console.WriteLine($"Received: {message}");
                if (message == "start")
                {
                    //StartBroadcasting();

                    udpClient = new UdpClient();
                    string replymessage = $"Ip : {IPAddress.Broadcast}";
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    try
                    {
                        await udpClient.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Broadcast, broadcastPort));
                        Console.WriteLine($"Broadcasting: {message}");
                        await Task.Delay(5000);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error while broadcasting: {e.Message}");
                    }
                    finally
                    {
                        udpClient.Close();
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while listening: {e.Message}");
        }
        finally
        {
            listener.Close();
            Console.WriteLine("Stopped listening.");
        }
    }

    static void StopListening()
    {
        isListening = false;
        udpClient?.Close();
        Console.WriteLine("Stopping listening...");
    }
}
