using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace client1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
           await SendMessageAsync();
        }

        static UdpClient udpClient;
        static int broadcastPort = 3000; // Port for broadcasting

        private static async Task SendMessageAsync()
        {
            


            udpClient = new UdpClient();
            string message = $"start";
            byte[] data = Encoding.UTF8.GetBytes(message);

            try
            {
                
                    await udpClient.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Broadcast, broadcastPort));
                    Console.WriteLine($"start");
                    await Task.Delay(5000);
                
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
}
