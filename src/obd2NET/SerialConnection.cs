﻿namespace obd2NET;

using System;
using System.Threading;

/// <summary>
/// Represents the serial connection to the vehicle's interface
/// </summary>
public class SerialConnection : IOBDConnection
{
  /// <summary>
  /// Port used for communicating with the vehicle's interface
  /// </summary>
  public IObdSerialPort Port { get; set; }

  public SerialConnection() :
    this(new ObdSerialPort())
  {
  }

  public SerialConnection(string comPort) :
    this(new ObdSerialPort(comPort))
  {
  }

  public SerialConnection(IObdSerialPort port)
  {
    Port = port;
    Open();
  }

  ~SerialConnection()
  {
    Close();
  }

  /// <summary>
  /// Opens the connection to the interface if no connection was established yet
  /// </summary>
  public void Open()
  {
    if (!Port.IsOpen)
    {
      Port.Open();
    }
  }

  /// <summary>
  /// Closes the connection to the interface if the connection is established
  /// </summary>
  public void Close()
  {
    if (Port.IsOpen)
    {
      Port.Close();
    }
  }

  /// <summary>
  /// Queries data from the vehicle by sending a specific mode and PID
  /// </summary>
  /// <param name="parameterMode"> <c>Vehicle.Mode</c> used </param>
  /// <param name="parameterID"> <c>Vehicle.PID</c> indicating the information to query </param>
  /// <returns> <c>ControllerResponse</c> object holding the returned data from the controller unit </returns>
  /// <remarks> Blocking until a complete answer has been received </remarks>
  public ControllerResponse Query(Vehicle.Mode parameterMode, Vehicle.PID parameterID)
  {
    Port.Write(Convert.ToUInt32(parameterMode).ToString("X2") + Convert.ToUInt32(parameterID).ToString("X2") + "\r");
    Thread.Sleep(100);

    var fullResponse = "";
    while (!fullResponse.Contains(">"))
    {
      var readBuffer = new byte[1024];
      Port.Read(readBuffer, 0, 1024);
      fullResponse = System.Text.Encoding.Default.GetString(readBuffer);
    }

    return new ControllerResponse(fullResponse, parameterMode, parameterID);
  }

  /// <summary>
  /// Queries data from the vehicle by sending a specific mode
  /// </summary>
  /// <param name="parameterMode"> <c>Vehicle.Mode</c> used </param>
  /// <returns> <c>ControllerResponse</c> object holding the returned data from the controller unit </returns>
  /// <remarks> Blocking until a complete answer has been received </remarks>
  public ControllerResponse Query(Vehicle.Mode parameterMode)
  {
    Port.Write(Convert.ToUInt32(parameterMode).ToString("X2") + "\r");
    Thread.Sleep(100);

    var fullResponse = "";
    while (!fullResponse.Contains(">"))
    {
      var readBuffer = new byte[1024];
      Port.Read(readBuffer, 0, 1024);
      fullResponse = System.Text.Encoding.Default.GetString(readBuffer);
    }

    return new ControllerResponse(fullResponse, parameterMode);
  }
}