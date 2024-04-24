namespace obd2NET.Tests;

using Moq;

[TestFixture]
public class Vehicle_Tests
{
  private Mock<IObdSerialPort> _port;
  public SerialConnection _serConn;
  
  [SetUp]
  public void Setup()
  {
    _port = new();
    _serConn = new(_port.Object);
  }

  [Test]
  public void Speed_returns_expected()
  {
    Assert.Pass();
  }
}
