using System.Collections.Generic;

namespace GUIClient.Models;

public class OperationError
{
    
    public string Title { get; set; }
    public int Status { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}