using System;
using System.Windows.Forms;
using System.Text;
using System.Net.Sockets;
using System.Threading;


namespace WindowsFormsApplication9
{
    public partial class Chat : Form{

        String hostname = "";
        String version = "0.05";
        String nombre;
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;

        public Chat()
        {
            InitializeComponent();
            button1.Text = "Enviar";
            button2.Text = "Conectar al servidor";
            button3.Text = "Confirmar datos";
            label1.Text = "Nombre";
            label2.Text = "IP";
            this.Text = "GregoryChat " + version;
            this.button1.Enabled = false;
            this.button2.Enabled = false;
            this.textBox2.Enabled = false;
        }

        private void getMessage()
        {
            while (true)
            {
                try
                {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[10025];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
                msg();
                }
                catch (Exception e)
                {
                }
            }
        }

        private void msg()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(msg));
            }
            else
            {
                textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + readData;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            escribir(textBox2.Text);
            this.textBox2.Text = "";
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (nombre == null || hostname == null)
            {
                MessageBox.Show("Debes introducir un nombre");
            }
            else
            {
                readData = "Conectado";
                msg();
                clientSocket.Connect(hostname, 8888);
                serverStream = clientSocket.GetStream();
                escribir(nombre);
                Thread ctThread = new Thread(getMessage);
                ctThread.Start();
                this.textBox2.Enabled=true;
                this.button1.Enabled=true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "")
            {
                nombre = textBox3.Text;
                hostname = textBox4.Text;
                this.button2.Enabled = true;
            }
            else
            {
                MessageBox.Show("Debes escribir tu nombre y la IP del servidor");
            }
        }

        private void escribir (string linea){
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(linea + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void cierre(object sender, FormClosingEventArgs e)
        {
            escribir("/desconectar");
        }

    }
}
