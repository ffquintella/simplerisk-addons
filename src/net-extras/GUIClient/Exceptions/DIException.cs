using System;

namespace ClientServices.Exceptions;

public class DIException: Exception
{
    public DIException(string message) : base(message)
    {
    }
}