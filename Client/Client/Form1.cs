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
        public Form1()
        {
            InitializeComponent();
        }

        

        private void Submit_Click(object sender, EventArgs e)
        {
			int port = 8080;
			try
			{
				IPAddress ip_adress = new IPAddress(new byte[] { 10, 0 , 0 ,3});
				IPEndPoint end = new IPEndPoint(ip_adress , port);
				TcpClient client = new TcpClient(end);
				int bytecount = Encoding.ASCII.GetByteCount(message.Text);

				byte[] sendData = new byte[bytecount];
				sendData = Encoding.ASCII.GetBytes(message.Text);
				NetworkStream stream = client.GetStream();
				stream.Write(sendData, 0, sendData.Length);
				stream.Close();
				client.Close();
			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
				
			}

        }
    }
}
