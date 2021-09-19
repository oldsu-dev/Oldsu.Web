using System;
using Oldsu.Logging;
using Oldsu.Logging.Strategies;

namespace Oldsu.Web
{
    public class Global
    {
        public static LoggingManager LoggingManager = new LoggingManager(
            new MongoDbWriter(
                Environment.GetEnvironmentVariable("OLDSU_MONGO_DB_CONNECTION_STRING") ?? throw new NullReferenceException()));
    }
}