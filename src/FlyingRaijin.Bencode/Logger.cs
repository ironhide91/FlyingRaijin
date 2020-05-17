using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FlyingRaijin.Bencode
{
    public static class MyLogger
    {
        //private static readonly Logger Instance =
        //    new LoggerConfiguration()
        //        .WriteTo.File("Bencode-.txt", rollingInterval: RollingInterval.Day)
        //        .CreateLogger();

        static MyLogger()
        {
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
        }

        public static ILogger GetLogger<T>([CallerMemberName] string memberName = "")
        {
            return
                new LoggerConfiguration()                        
                        .WriteTo.Debug()
                        //.WriteTo.File("D://Bencode-.txt", rollingInterval: RollingInterval.Day)
                        .Enrich.With(new InvocationContextEnricher(typeof(T).FullName, memberName))
                        .CreateLogger();
        }
    }

    public class InvocationContextEnricher : ILogEventEnricher
    {
        public InvocationContextEnricher(string typeName, string callerMemberName)
        {
            TypeName = typeName;
            CallerMemberName = callerMemberName;
        }

        public string TypeName { get; protected set; }
        public string CallerMemberName { get; protected set; }

        public static string TypeNamePropertyName { get; } = Constants.SourceContextPropertyName;
        public static string CallerMemberNamePropertyName { get; } = "CallerMemberName";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty(TypeNamePropertyName, new ScalarValue(TypeName)));
            logEvent.AddPropertyIfAbsent(new LogEventProperty(CallerMemberNamePropertyName, new ScalarValue(CallerMemberName)));
        }
    }
}