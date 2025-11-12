namespace BackgroundService;

public class BackgroundWorkerService : IHostedService, IDisposable
{
    private int _executionCount = 0;
    private readonly ILogger<BackgroundWorkerService> _logger;
    public readonly List<Timer> TimerList = [];

    public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger)
    {
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Timed Hosted] Service running.");
        return Task.CompletedTask;
    }
    
    public void StartNewTimer()
    {
        var timer = new Timer(DoWork, TimerList.Count + 1, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        TimerList.Add(timer);
        _logger.LogInformation("[Timed Hosted Service] has started a new timer. Timer Count: {TimerCount}", TimerList.Count);
    }
    
    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref _executionCount);
        _logger.LogInformation("[Timed Hosted Service] has incremented. counter: {counter}  new execution count: {CounterValue}", state, count);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[Timed Hosted Service] is stopping.");
        TimerList.ForEach(timer => timer.Change(Timeout.Infinite, Timeout.Infinite));
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        TimerList.ForEach(timer => timer.Dispose());
    }
}