using System;

namespace GUIClient.Configuration;

public class ServerConfiguration
{
    public string Url { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public DateTime Timeout { get; set; }
}