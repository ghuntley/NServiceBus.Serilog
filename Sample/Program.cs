﻿using System;
using NServiceBus;
using NServiceBus.Installation.Environments;
using NServiceBus.Serilog;
using Serilog;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            Configure.GetEndpointNameAction = () => "NServiceBusSerilogSample";

            //Setup Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logFile.txt")
                .CreateLogger();

            //Set NServiceBus to log to Serilog
            SerilogConfigurator.Configure();

            //Start using NServiceBus
            Configure.Serialization.Json();
            Configure.With()
                .DefaultBuilder()
                .InMemorySagaPersister()
                .UseInMemoryTimeoutPersister()
                .InMemorySubscriptionStorage()
                .UnicastBus()
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());
            Console.ReadLine();
        }
    }
}
