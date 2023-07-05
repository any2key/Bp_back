using Bp_tcp_server.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bp_tcp_server.Utils
{
    internal class FileLogger:IBpLogger
    {
        private readonly NLog.Logger tracelogger;
        private readonly NLog.Logger errorLogger;
        private readonly IBpConfiguration config;
        public FileLogger(IBpConfiguration config)
        {
            this.config=config;
            tracelogger = NLog.LogManager.GetLogger("traceLogger");
            errorLogger = NLog.LogManager.GetLogger("errorLogger");
        }

        public void Error(string error)
        {
            errorLogger.Error($"{config.Port}: {error}");
        }

        public void Error(Exception ex)
        {
            errorLogger.Error($"{config.Port}: {ex.Message}");
        }

        public void Log(string message)
        {
            tracelogger.Trace($"{config.Port}: {message}");
        }
    }
}
