using System;
using System.Globalization;
using System.Text;
using Avalonia.Data;
using Avalonia.Data.Converters;
using GUIClient.Exceptions;

namespace GUIClient.Converters;

public class StatusToColourConverter: IValueConverter
{
    public static readonly StatusToColourConverter Instance = new();
    
    public object? Convert( object? value, 
        Type targetType, 
        object? parameter, 
        CultureInfo culture)
    {
        if (value is null) return "";
        
        if (value is string sourceData && targetType.IsAssignableTo(typeof(string)))
        {
            if (sourceData.Length == 0) throw new InvalidStatusException("Invalid empty status to convert", sourceData);

            switch (sourceData)
            {
                case "New":
                    return "#40ff40";
                case "Management Review":
                    return "#ffb940";
                default:
                    return "#cee4eb";
                    //throw new InvalidStatusException("Unrecognized status", sourceData);
                    //break;
            }
        }
        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
    
    public object ConvertBack( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        throw new NotSupportedException();
    }
}