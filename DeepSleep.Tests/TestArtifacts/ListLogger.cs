namespace DeepSleep.Tests.TestArtifacts
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;

    public class ListLogger : ILogger
    {
        private List<string> logs = new List<string>();

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            logs.Add(state.ToString());
        }
    }
}
