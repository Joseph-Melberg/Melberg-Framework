using System;

namespace Demo.MySql.Infrastructure;
public class HeartbeatModel
{
    public string name { get; set; }
    public string mac { get; set; }
    public DateTime timestamp { get; set; }
    public bool online { get; set; }
    public bool announced { get; set; }
}