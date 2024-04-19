namespace obd2net.app;

using obd2NET;

public sealed class Program : IDisposable
{
  private readonly CancellationTokenSource _cts = new();

  public static void Main(string[] args)
  {
    using var prgm = new Program();
    prgm.Run(args);
  }

  private void Run(string[] args)
  {
    var comPort = (args.Length == 1) ? args[0] : "COM5";

    Console.WriteLine($"Connecting to USB OBD on: {comPort}");

    IOBDConnection conn = new SerialConnection(comPort);

    // install ctrl-c handler to shut down worker
    Console.CancelKeyPress += ConsoleOnCancelKeyPress;
    Console.WriteLine("Press ctrl-c to exit");

    while (!_cts.IsCancellationRequested)
    {
      // get the current (real-time) speed of the vehicle given in km/h:
      var speed = Vehicle.Speed(conn);
      
      Console.WriteLine($"  Speed: {speed} km/hr");
      
      Thread.Sleep(TimeSpan.FromSeconds(1));
    }

    Console.WriteLine($"  Cancellation requested  [{Thread.CurrentThread.ManagedThreadId}]...");
  }

  private void ConsoleOnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
  {
    e.Cancel = true;
    _cts.Cancel();

    Console.WriteLine("Console cancelled");
  }

  public void Dispose()
  {
    _cts.Dispose();

    Console.WriteLine("Disposed");
  }
}
