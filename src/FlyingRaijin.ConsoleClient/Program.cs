using System;
using FlyingRaijin.Controller;

namespace FlyingRaijin.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(10000);
            TheController.Instance.Add(@"d:\ubuntu-20.04.2.0-desktop-amd64.iso.torrent");

            Console.ReadLine();
        }
    }
}