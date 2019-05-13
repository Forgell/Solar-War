using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client_Solar_War
{
    class Networking
    {
        public static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static readonly int PORT = 100;

		public string getMessage() {
			byte[] server_message_as_bytes = new byte[100];
			ClientSocket.Receive(server_message_as_bytes); //(server_message_as_bytes, 0, server_message_as_bytes.Length, SocketFlags.None);
			server_message_as_bytes = server_message_as_bytes.Where(val => val != 0).ToArray();
			string server_message_as_string = Encoding.ASCII.GetString(server_message_as_bytes);
			return server_message_as_string;
		}


        public int recieveServerMessage(Label player_number_label)
        {
            byte[] server_message_as_bytes = new byte[100];
            ClientSocket.Receive(server_message_as_bytes); //(server_message_as_bytes, 0, server_message_as_bytes.Length, SocketFlags.None);
            server_message_as_bytes = server_message_as_bytes.Where(val => val != 0).ToArray();
            string server_message_as_string = Encoding.ASCII.GetString(server_message_as_bytes);
            if (!server_message_as_string.Equals(""))
            {
                //Console.WriteLine(server_message_as_string);
                if (server_message_as_string.Contains("You are player: "))
                {
                    int player_number = player_number = server_message_as_string[server_message_as_string.Length - 1] - '0';
                    player_number_label.updateText("You are player: " + player_number);
                    return player_number;
                }

                ClientSocket.Send(Encoding.ASCII.GetBytes("buffer"));
            }
            return 0;
        }

		public byte[] GetMap()
		{
			byte[] temp = new byte[100];
			try
			{
				ClientSocket.Receive(temp);
			}catch (Exception e) { Console.WriteLine(e.Message); }
			
			return temp;
		}

        public void closeStream()
        {
            if (ClientSocket.IsBound)
            {
                byte[] exit_message_as_bytes = Encoding.ASCII.GetBytes("exit");
                ClientSocket.Send(exit_message_as_bytes, 0, exit_message_as_bytes.Length, SocketFlags.None);
                ClientSocket.Close();
            }
            //state = State.CLOSING;
        }
		public void closeStream(int num)
		{
			if (ClientSocket.IsBound)
			{
				byte[] exit_message_as_bytes = Encoding.ASCII.GetBytes("exit" + num);
				ClientSocket.Send(exit_message_as_bytes, 0, exit_message_as_bytes.Length, SocketFlags.None);
				ClientSocket.Close();
			}
			//state = State.CLOSING;
		}
		public void send(String str)
		{
			byte[] message_as_bytes = Encoding.ASCII.GetBytes(str);
			ClientSocket.Send(message_as_bytes, 0, message_as_bytes.Length, SocketFlags.None);
		}
        /// <summary>
		/// is run only when the enter key is pressed and attempts to connect to the ip that is inputed by th custom text box
		/// </summary>
		public State attemptConnectionToServer(TextBox text_box)
        {
            if(text_box.Text.Equals(""))
            {
                return State.NULL;
            }
            try
            {
				string str = text_box.Text.Substring("Type in server ip: ".Length);
                string[] parts_of_ip_address = str.Split('.');
                byte[] ip_adress_as_byte_array = new byte[parts_of_ip_address.Length];
                for (int i = 0; i < parts_of_ip_address.Length; i++)
                {
                    ip_adress_as_byte_array[i] = Byte.Parse(parts_of_ip_address[i]);
                }
                try
                {
					IAsyncResult result = ClientSocket.BeginConnect(new IPAddress(ip_adress_as_byte_array), PORT, null, null);

					bool success = result.AsyncWaitHandle.WaitOne(2000, true);
					//ClientSocket.Connect(new IPAddress(ip_adress_as_byte_array) , PORT);

					if (ClientSocket.Connected)
					{
						//ClientSocket.EndConnect(result);
						//ClientSocket.Connect(new IPEndPoint(new IPAddress(ip_adress_as_byte_array), PORT));
						return State.WAITING_FOR_ALL_PLAYERS;
					}
					else
					{
						// NOTE, MUST CLOSE THE SOCKET

						ClientSocket.Close();
						ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
						throw new ApplicationException("Failed to connect server.");
						//return State.CONNECTING;
					}
					


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

            return State.NULL;

        }


        /// <summary>
		/// handels  the input from the KeyboardHelper class
		/// </summary>
		/// <param name="console"></param>
		public State getConnectingInput(KeyboardState console, KeyboardState old, TextBox text_box)
        {
            char input_as_char = KeyboardHelper.getIPInput(console, old);
            switch ((int)input_as_char)
            {
                case 000: break; // nothing is pressed
                case 004: // is transmitted if enter key is pressed
                    return attemptConnectionToServer(text_box);
                default:
                    text_box.updateText(input_as_char);
                    break;


            }
            return State.NULL;

        }
    }
}
