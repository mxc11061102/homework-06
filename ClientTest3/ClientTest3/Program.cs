using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace ClientTest3
{
    class Program
    {
        static void Main()
        {
            ClientAPI.GNClient.SetIpAddr("127.0.0.1");//设置服务器IP地址，如果是本地就设置为127.0.0.1，否则设置为服务器真实地址，仅供测试使用
            ClientAPI.GNClient client = new ClientAPI.GNClient();
            client.SetUserInfo("a11", "a");//可以使用a1到a80之间的所有用户名登入，密码均为a
            if (client.Login()) Console.WriteLine("Welcome to the game");
            else Console.WriteLine("Failed to log in");
            double goldNumber=50;
            while (true)
            {
                string str = client.Receive();//获得服务器返回信息，如果是开始信息，那么提交数字
                string[] param = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (param[0] == "BEGN" && param[1] == "1")//是开始信息，并且游戏模式是提交一个数字
                {
                    if (goldNumber > 2)
                    {
                        client.Commit(goldNumber * 0.7);//提交数字
                        Console.WriteLine("Commit number {0} to server", goldNumber * 0.7);
                        Console.WriteLine(client.Receive());//获得提交信息，成功或者失败
                    }
                    if (goldNumber <= 2)
                    {
                        client.Commit(1.001);
                        Console.WriteLine("Commit number{0} and {1} to server", 1.001);
                        Console.WriteLine(client.Receive());
                    }
                }
                if (param[0] == "BEGN" && param[1] == "2")
                {
                    if (goldNumber >2)
                    {
                        client.Commit(goldNumber, goldNumber * 0.7);
                        Console.WriteLine("Commit number{0} and {1} to server", goldNumber, goldNumber * 0.7);
                        Console.WriteLine(client.Receive());
                    }
                    if (goldNumber <=2)
                    {
                        client.Commit(1.001 , 1.001);
                        Console.WriteLine("Commit number{0} and {1} to server", 1.001 ,1.001 );
                        Console.WriteLine(client.Receive());
                    }

                }
                do
                    str = client.Receive();//获得以RSLT为开头的字串，表示本轮信息
                while (!str.Contains("RSLT"));
                string[] info = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine(str);
                goldNumber = double.Parse(info[3]);//info的内容为：0 RSLT, 1，本轮得分。 2，当前总得分。 3，本局黄金数
            }
        }
    }
}
