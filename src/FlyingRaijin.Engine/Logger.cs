using Serilog;
using System;

namespace FlyingRaijin.Bencode.Read
{
    public static class Scroll
    {
        private static ILogger logger;

        public static void SetLogger(ILogger _logger)
        {
            logger = _logger.ForContext(typeof(Parser)) ?? throw new ArgumentNullException(nameof(_logger));
        }
    }
}
