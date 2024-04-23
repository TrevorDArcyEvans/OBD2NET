namespace obd2NET;

using System.IO.Ports;

public sealed class ObdSerialPort : SerialPort, IObdSerialPort
{
  public ObdSerialPort(string comPort) :
    base(comPort)
  {
  }

  public ObdSerialPort()
  {
  }
}
