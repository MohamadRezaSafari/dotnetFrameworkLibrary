using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Providers
{
    public class SynchronousClientSocket
    {
        private readonly static IPAddress IP = IPAddress.Parse("127.0.0.1");
        private const int port = 1994;


        public static void StartClient(string Tag, byte[] Data, string CloseTag)
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IP, port);
                Socket sender = new Socket(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    sender.Send(Encoding.UTF8.GetBytes(Tag));
                    sender.Send(Data);
                    sender.Send(Encoding.UTF8.GetBytes(CloseTag));

                    int bytesRec = sender.Receive(bytes);
                    var receive = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        private static byte[] AppendTwoByteArrays(byte[] arrayA, byte[] arrayB)
        {
            byte[] outputBytes = new byte[arrayA.Length + arrayB.Length];
            Buffer.BlockCopy(arrayA, 0, outputBytes, 0, arrayA.Length);
            Buffer.BlockCopy(arrayB, 0, outputBytes, arrayA.Length, arrayB.Length);
            return outputBytes;
        }
    }
}
