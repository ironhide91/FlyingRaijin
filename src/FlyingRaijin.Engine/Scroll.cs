using Serilog;

namespace FlyingRaijin
{
    public static class Scroll
    {
        internal static readonly ILogger Logger;

        static Scroll()
        {
            Logger =
                new LoggerConfiguration()
                    .WriteTo
                    .File(
                        path: "logs\\myapp.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}")
                    .Enrich.FromLogContext()
                    .MinimumLevel.Debug()                    
                    .CreateLogger();
        }
    }
}