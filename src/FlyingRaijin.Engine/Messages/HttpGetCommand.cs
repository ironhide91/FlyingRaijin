﻿namespace FlyingRaijin.Engine.Messages
{
    public class HttpGetCommand
    {
        public readonly string Url;

        public HttpGetCommand(string url)
        {
            Url = url;
        }
    }
}