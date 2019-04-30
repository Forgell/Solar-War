using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace Server
{
    class Program
    {
		// client related varibles
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 100;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
		// game related varibles
		private static int TOTAL_PLAYER_NUMBER = 0;
		//private static PlayerState[] player_state = new PlayerState[4];
		// need to create a game loop so I will use threadgin as a filler? good idea
		private static Thread game_loop_thread;
        public static string players_connected_as_string;
        private static Dictionary<Socket, int> playernums = new Dictionary<Socket, int>(4);

		//private List<string> actions;

        static void Main(string[] args)
        {
            Console.Title = "Server";
            SetupServer();
            Console.ReadLine(); // press endter to close Server
            CloseAllSockets();

        }

        public static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any , PORT));
			IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList,a => a.AddressFamily == AddressFamily.InterNetwork);
			Console.WriteLine(ipv4Addresses[ipv4Addresses.Length - 1]);
			// init
			//for (int i = 0; i < player_state.Length; i++)
			//{
			//	player_state[i] = PlayerState.NOT_CONNECTED;
			//}
            serverSocket.Listen(0);
            serverSocket.BeginAccept( AcceptCallback, null);
			game_loop_thread = new Thread(new ThreadStart(game_loop));
            Console.WriteLine("Server setup complete");
            players_connected_as_string = "";
        }


        public static void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            serverSocket.Close();
			game_loop_thread.Abort();
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }


			if (clientSockets.Count != 4)
			{
				clientSockets.Add(socket);
				socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
				Console.WriteLine("Client connected, waiting for request...");
				//clientSockets[clientSockets.Count - 1].Send(Encoding.ASCII.GetBytes("You are player: " + (++TOTAL_PLAYER_NUMBER)));
				//player_state[clientSockets.Count - 1] = PlayerState.ASSIGNING_PLAYER_NUMBER;
				serverSocket.BeginAccept(AcceptCallback, null);
				if (clientSockets.Count == 4)
				{
					for(int i = 0; i < 4; i++)
					{
						clientSockets[i].Send(Encoding.ASCII.GetBytes("Game Start!"));
					}
					game_loop_thread.Start();
				}
			}
			else
			{
				socket.Send(Encoding.ASCII.GetBytes("player list is full"));
			}
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;
			
            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                players_connected_as_string = players_connected_as_string.Remove(players_connected_as_string.IndexOf("" + playernums[current]), 1);
                playernums.Remove(current);
                Console.WriteLine("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                clientSockets.Remove(current);
				TOTAL_PLAYER_NUMBER--;
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            if (!text.Equals("buffer"))
            {
                //Console.WriteLine("Received Text: " + text);
            }

            if (text.Equals(""))
            {
                return;
            }
            
            if (text.ToLower() == "exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing.
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                Console.WriteLine("Client disconnected");
				TOTAL_PLAYER_NUMBER--;
                return;
            }
			else if (text.ToLower().Contains("exit"))
			{
                playernums.Remove(current);
				current.Shutdown(SocketShutdown.Both);
				current.Close();
				clientSockets.Remove(current);
				Console.WriteLine("Client " + text.ToCharArray()[4] + " disconnected");
				TOTAL_PLAYER_NUMBER--;
                //Console.WriteLine(players_connected_as_string.IndexOf(text.ToCharArray()[4]));
                //Console.WriteLine(text.ToCharArray()[4]);
				players_connected_as_string = players_connected_as_string.Remove(players_connected_as_string.IndexOf(text.ToCharArray()[4]), 1);
				return;
			}
            else
            {
                Console.WriteLine("recieved: " + text);
                //byte[] data = Encoding.ASCII.GetBytes(text); // sends the data back at them at current time is just sends buffer back and forth
                //current.Send(data);
                if (text.Contains("take"))
                {
                    int num = text[text.Length - 1] - '0';
                    if (!players_connected_as_string.Contains("" + num))
                    {
                        players_connected_as_string += num;
                        current.Send(Encoding.ASCII.GetBytes("You are player: " + num));
                        Console.WriteLine("sent: " + "You are player: " + num);
                        playernums.Add(current, num);
                    }else
                    {
                        current.Send(Encoding.ASCII.GetBytes("taken"));
                        Console.WriteLine("sent: taken");
                    }
                    if (playernums.Count == 4)
                    {
                        if (clientSockets.Count == 4)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                clientSockets[i].Send(Encoding.ASCII.GetBytes("Game Start!"));
                            }
                            game_loop_thread.Start();
                        }
                    }
                }
                //Console.WriteLine("Warning Sent");
            }
            //buffer messages
			//foreach(Socket client in clientSockets){
			//	client.Send(new byte[100]);
			//}
            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

		public static void game_loop()
		{
			while (true) {
				Stopwatch watch = new Stopwatch();
				watch.Start();
				
				foreach (Socket socket in clientSockets)
				{
					

				}
				watch.Stop();
				int time = (int)(((1.0 / 60.0) - (watch.ElapsedMilliseconds / Math.Pow(10, 3))) * 1000);
				Thread.Sleep( time ); // should wait a 60th of a seciund
			}
		}
    }
}
