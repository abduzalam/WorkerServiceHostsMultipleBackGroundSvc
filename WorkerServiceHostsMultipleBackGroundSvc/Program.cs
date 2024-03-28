using Serilog;
using WorkerServiceHostsMultipleBackGroundSvc;

internal class Program
{
    private static async Task Main(string[] args)
    {

        // do not specify logfile like below format, that will cause issues like windows service do not execute
        // whe we host it as a windows service

        //  .WriteTo.File("Logs/test.txt", rollingInterval: RollingInterval.Day)
        // also for windows service, keep all registration singleton
        // 
        var configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

        var logFilePath = configuration["ApplicationSettings:LogFile"];

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
        try
        {
            IHost host = Host.CreateDefaultBuilder(args)
           .UseWindowsService(options =>
           {
               options.ServiceName = "NSpec DataQueue Service";
           })
           .UseSerilog((hostingContext, services, loggerConfiguration) =>
           {
               loggerConfiguration
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .WriteTo.Debug();
           })
       .ConfigureServices((hostingContext, services) =>
       {
           var configuration = hostingContext.Configuration;



           services.AddSingleton<FirstWorker>();
           services.AddSingleton<SecondWorker>();
           services.AddSingleton<ThirdWorker>();
           services.AddSingleton<IProcessFactory, ProcessFactory>();
           services.AddHostedService<FirstWorkerService>(serviceProvider =>
           {
               var factory = serviceProvider.GetRequiredService<IProcessFactory>();

               var logger = serviceProvider.GetRequiredService<ILogger<FirstWorkerService>>();
               var process = factory.CreateProcess(ProcessType.First);

               return new FirstWorkerService(logger, process);
           });
           services.AddHostedService<SecondWorkerService>(serviceProvider =>
           {
               var factory = serviceProvider.GetRequiredService<IProcessFactory>();

               var logger = serviceProvider.GetRequiredService<ILogger<SecondWorkerService>>();
               var process = factory.CreateProcess(ProcessType.Second);
               return new SecondWorkerService(logger, process);
           });
           services.AddHostedService<ThirdWorkerService>(serviceProvider =>
           {
               var factory = serviceProvider.GetRequiredService<IProcessFactory>();

               var logger = serviceProvider.GetRequiredService<ILogger<ThirdWorkerService>>();
               var process = factory.CreateProcess(ProcessType.Third);

               return new ThirdWorkerService(logger, process);
           });

       })
        .Build();

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            // Handle exceptions from host configuration or during startup
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            // Ensure any buffered events are written to the sinks and logging system is properly shut down
            Log.CloseAndFlush();
        }
    }
}