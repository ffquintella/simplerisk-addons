namespace Model.Exceptions;

public class RestComunicationException: Exception
{
    
    public string RestExceptionMessage { get; set; }
    
    public RestComunicationException(string restException)
    {
        RestExceptionMessage = restException;
    }
}