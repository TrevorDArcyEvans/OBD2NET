Introduction
===
Most vehicles nowadays are equipped with board computers permanently controlling the state of internal vehicle components, performing tests and storing current vehicle data and failures. These board computers are accessable through the OBD port commonly located at the driving cab in the near of the clutch.  obd2NET is a .NET libarary for querying these OBD systems. Real time data and diagnostic codes can be retrieved by performing simple serial requests. Basically, querying works just as simple: You cast the query mode (i.e. `0x01` for the current data) and specify a PID (parameter ID) such as `0x0D` for the vehicle speed to get your information back. See [OBD-II PIDs](http://en.wikipedia.org/wiki/OBD-II_PIDs) for available modes and PIDs.

However, when using this library, you don't need to worry on how to communicate with the vehicle, just plug in the OBD connector into your vehicle's OBD port and get the other end of the cable to communicate with your device (USB/serial interfaces).

Examples
===
Opening the connection is as easy as it gets:

    IOBDConnection conn = new SerialConnection("COM5"); // you may need to replace the COM port
    
Getting the current (real-time) speed of the vehicle given in km/h:

    uint speed = Vehicle.Speed(conn);
    
    
Obtaining available DTCs (Diagnostic Trouble Codes)


    var networkTroubleCodes = Vehicle.DiagnosticTroubleCodes(conn).Where(code => code.ErrorLocation == DiagnosticTroubleCode.Location.Network);

    foreach (var code in networkTroubleCodes)
        Console.WriteLine(code.TextRepresentation);
    
The given code will query all network trouble codes and print its text representation to the screen.
    
These are not all possibilities of the library, refer to the `Vehicle` class to get all implemented functions that query data. Please note that I only have tested these functions in my Audi A3 (2007) and can therefore not guarantee, that they will work for you.


Compatibility
===
Of course, not all vehicles around are implementing the ISO 9141 standard so this library might not work for your vehicle. Before OBD2, interfaces were manufacturer specific implemented so you'd rather look into your manufacturers implementation details and extend the library by yourself.

Troubleshooting
===

### [Why would ch341-uart be disconnected from ttyUSB?](https://stackoverflow.com/questions/70123431/why-would-ch341-uart-be-disconnected-from-ttyusb)

When connecting USB OBD device, `dmesg` gives output similar to:

```bash
[679413.370685] usb 1-2.4.3: USB disconnect, device number 10
[679417.176663] usb 1-2.4.3: new full-speed USB device number 11 using xhci_hcd
[679417.298905] usb 1-2.4.3: New USB device found, idVendor=1a86, idProduct=7523, bcdDevice= 2.64
[679417.298915] usb 1-2.4.3: New USB device strings: Mfr=0, Product=2, SerialNumber=0
[679417.298919] usb 1-2.4.3: Product: USB Serial
[679417.337967] ch341 1-2.4.3:1.0: ch341-uart converter detected
[679417.352141] usb 1-2.4.3: ch341-uart converter now attached to ttyUSB0
[679417.890411] input: BRLTTY 6.4 Linux Screen Driver Keyboard as /devices/virtual/input/input16
[679417.895881] usb 1-2.4.3: usbfs: interface 0 claimed by ch341 while 'brltty' sets config #1
[679417.899018] ch341-uart ttyUSB0: ch341-uart converter now disconnected from ttyUSB0
[679417.899038] ch341 1-2.4.3:1.0: device disconnected
```

The device should be connected to `ttyUSB0` but has been disconnected.  Per the referenced article:

```text
For Ubuntu 22.04 (Jammy Jellyfish), the simplest solution is to remove the package brltty via 
`sudo apt remove brltty`, since it''s unnecessary unless you''re using a braille e-reader.  
However, I am unsure if it could cause errors later on.
```

License
===
  
    The MIT License (MIT)
    
    Copyright (c) 2014 
    
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
