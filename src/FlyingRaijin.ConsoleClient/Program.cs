using FlyingRaijin.Controller;
using System;

namespace FlyingRaijin.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = @"D:\DEV\FlyingRaijin\bt-protocol\src\FlyingRaijin.Test\Artifacts\Torrents\Slackware64-14.1-install-dvd.torrent";

            System.Threading.Thread.Sleep(20000);

            TheController.Instance.Add(file);

            Console.ReadLine();
        }
    }
} 