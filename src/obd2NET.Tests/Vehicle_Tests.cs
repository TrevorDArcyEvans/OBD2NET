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

  [Test]
  public void Engine_temperature_returns_expected([Values(-40, 0, 30, 95, 215)] int exp)
  {
    var expStr = (exp + 40).ToString("x2");
    var dataStr = $"\n01 05 {expStr} \r\n>";
    var data = new List<byte>(Encoding.Default.GetBytes(dataStr));

    _port.Setup(x => x.Read(It.IsAny<byte[]>(), 0, 1024))
      .Callback<byte[], int, int>((buffer, offset, count) => { data.CopyTo(buffer, 0); });

    var res = Vehicle.EngineTemperature(_serConn);

    res.Should().Be(exp);
  }

  [Test]
  public void Throttle_position_returns_expected([Values(40u, 0u, 30u, 95u, 100u)] uint exp)
  {
    var expStr = ((int)Math.Round(exp / 100d * 255d)).ToString("x2");
    var dataStr = $"\n01 11 {expStr} \r\n>";
    var data = new List<byte>(Encoding.Default.GetBytes(dataStr));

    _port.Setup(x => x.Read(It.IsAny<byte[]>(), 0, 1024))
      .Callback<byte[], int, int>((buffer, offset, count) => { data.CopyTo(buffer, 0); });

    var res = Vehicle.ThrottlePosition(_serConn);

    res.Should().Be(exp);
  }

  [Test]
  public void Calculated_engine_load_value_returns_expected([Values(40u, 0u, 30u, 95u, 100u)] uint exp)
  {
    var expStr = ((int)Math.Round(exp / 100d * 255d)).ToString("x2");
    var dataStr = $"\n01 04 {expStr} \r\n>";
    var data = new List<byte>(Encoding.Default.GetBytes(dataStr));

    _port.Setup(x => x.Read(It.IsAny<byte[]>(), 0, 1024))
      .Callback<byte[], int, int>((buffer, offset, count) => { data.CopyTo(buffer, 0); });

    var res = Vehicle.CalculatedEngineLoadValue(_serConn);

    res.Should().Be(exp);
  }

  [Test]
  public void Fuel_pressure_returns_expected([Values(40u, 0u, 199u, 195u, 765u)] uint exp)
  {
    var expStr = ((int)Math.Round(exp / 3d)).ToString("x2");
    var dataStr = $"\n01 0a {expStr} \r\n>";
    var data = new List<byte>(Encoding.Default.GetBytes(dataStr));

    _port.Setup(x => x.Read(It.IsAny<byte[]>(), 0, 1024))
      .Callback<byte[], int, int>((buffer, offset, count) => { data.CopyTo(buffer, 0); });

    var res = Vehicle.FuelPressure(_serConn);

    // since dividing by 3, not all values in range [0-765 kPa] are possible
    // so have to allow some leeway
    res.Should().BeCloseTo(exp, 1);
  }

  [Test]
  public void Malfunction_indicator_lamp_returns_expected([Values(true, false)] bool exp)
  {
    var dataA = (exp ? 128 : 127).ToString("x2");
    var dataB = 112.ToString("x2");
    var dataC = 157.ToString("x2");
    var dataD = 32.ToString("x2");
    var dataStr = $"\n01 01 {dataA} {dataB} {dataC} {dataD} \r\n>";
    var data = new List<byte>(Encoding.Default.GetBytes(dataStr));

    _port.Setup(x => x.Read(It.IsAny<byte[]>(), 0, 1024))
      .Callback<byte[], int, int>((buffer, offset, count) => { data.CopyTo(buffer, 0); });

    var res = Vehicle.MalfunctionIndicatorLamp(_serConn);

    res.Should().Be(exp);
  }
}
