using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketAsyncEventArgsClient
{
    class Program
    {
        Socket socket;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Listen();
            program.SendMessage();
            while (Console.Read() > 0)
            {
                program.SendMessage();
            }
        }

        private void Listen()
        {
            
            //获取框内Ip地址
            IPAddress localaddress = IPAddress.Parse("127.0.0.1");

            //创建端点
            IPEndPoint endPoint = new IPEndPoint(localaddress, int.Parse("5730"));

            //Ip4地址，流方式，Tcp协议
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            //连接端点
            socket.Connect(endPoint);

            Thread thread = new Thread(() => Receive());
            thread.Start();


            if (socket != null)
            {
                Console.WriteLine("连接成功");
            }
        }

        private void SendMessage()
        {
            string str = "Hello World";
            int packageType = 1;
            int packageLen = str.Length + 8;
            byte[] bType = System.BitConverter.GetBytes(packageType);
            byte[] bLen = System.BitConverter.GetBytes(packageLen);
            //将数据放入发送buffer
            List<byte> allList = new List<byte>();
            allList.AddRange(bType);
            allList.AddRange(bLen);
            allList.AddRange(System.Text.Encoding.Default.GetBytes(str));
            byte[] sendByte = allList.ToArray();

            socket.Send(sendByte);
        }

        private void Receive()
        {
            Console.WriteLine("开始接收");
            while (true)
            {
                Console.WriteLine("开始展示");
                Byte[] buffer = new Byte[1024];
                Byte[] buffer2 = new Byte[1024];
                int length = socket.Receive(buffer, 1024, SocketFlags.None);
                Array.Copy(buffer, 8, buffer2, 0, buffer.Count() - 8);
                String xiaoxi = Encoding.Default.GetString(buffer2);
                Console.WriteLine(xiaoxi);

            }
            socket.Close();
        }
    }

}
