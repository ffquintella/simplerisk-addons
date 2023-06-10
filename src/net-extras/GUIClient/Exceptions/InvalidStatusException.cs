using System;

namespace ClientServices.Exceptions;

public class InvalidStatusException: Exception
{
    public String Status { get; set; }
    public InvalidStatusException(string message, string status) : base(message)
    {
        Status = status;
    }
}