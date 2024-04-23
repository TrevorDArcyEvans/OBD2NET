namespace obd2NET;

public interface IObdSerialPort
{
  bool IsOpen { get;  }
  void Open();
  void Close();
  void Write(string data);
  int Read(byte[] buffer, int offset, int count);
}
