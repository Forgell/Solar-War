using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ip , 8080);
            TcpClient client = default(TcpClient);

            try
            {
                server.Start();
                Console.WriteLine("Server started");
                //Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine("server failed \n"+ e.Message);
                Console.Read();
            }

            while (true)
            {
                client = server.AcceptTcpClient();

                byte[] recieveBuffer = new byte[100];
                NetworkStream stream = client.GetStream();

                stream.Read(recieveBuffer , 0 , recieveBuffer.Length);
                string msg = Encoding.ASCII.GetString(recieveBuffer, 0, recieveBuffer.Length);
                Console.WriteLine(msg);
                
            }
        }
    }
}
