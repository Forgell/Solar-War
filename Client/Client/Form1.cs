using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;


namespace Client
{
    public partial class Form1 : Form
    {

        private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private int port = 100;

        public Form1()
        {
            InitializeComponent();
        }

        

        private void Submit_Click(object sender, EventArgs e)
        {
            /*IPAddress ip = new IPAddress(0x1c41880a);
            IPEndPoint end = new IPEndPoint(IPAddress.Any, 5001);
            TcpClient client = new TcpClient(end);
            int bytecount = Encoding.ASCII.GetByteCount(message.Text);

            byte[] sendData = new byte[bytecount];
            sendData = Encoding.ASCII.GetBytes(message.Text);
            NetworkStream stream = client.GetStream();
            stream.Write(sendData , 0 , sendData.Length);
            stream.Close();
            client.Close();
            */
            byte[] buffer = Encoding.ASCII.GetBytes(message.Text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string[] parts = adressTextBox.Text.Split('.');
            byte[] array = new byte[parts.Length];
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = Byte.Parse(parts[i]);
            }
            ClientSocket.Connect(new IPAddress(array), 100);
			port++;
        }
    }
}
