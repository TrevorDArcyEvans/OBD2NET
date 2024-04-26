namespace obd2NET.Tests;

[TestFixture]
public sealed class Vehicle_Tests
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
  public void Speed_returns_expected([Values(50u, 150u, 30u, 221u)] uint exp)
  {
    var expStr = exp.ToString("x2");
    var dataStr = $"\n01 0d {expStr} \r\n>";
    var data = new List<byte>(Encoding.Default.GetBytes(dataStr));

    _port.Setup(x => x.Read(It.IsAny<byte[]>(), 0, 1024))
      .Callback<byte[], int, int>((buffer, offset, count) => { data.CopyTo(buffer, 0); });

    var res = Vehicle.Speed(_serConn);

    res.Should().Be(exp);
  }

  [Test]
  public void RPM_returns_expected([Values(1500u, 1000u, 3500u, 12100u)] uint exp)
  {
    var rpmA = exp / 64;
    var rpmB = exp * 4 - 256 * rpmA;
    var rpmAstr = rpmA.ToString("x2");
    var rpmBstr = rpmB.ToString("x2");
    var dataStr = $"\n01 0c {rpmAstr} {rpmBstr} \r\n>";
    var data = new List<byte>(Encoding.Default.GetBytes(dataStr));

    _port.Setup(x => x.Read(It.IsAny<byte[]>(), 0, 1024))
      .Callback<byte[], int, int>((buffer, offset, count) => { data.CopyTo(buffer, 0); });

    var res = Vehicle.RPM(_serConn);

    res.Should().Be(exp);
  }
}
