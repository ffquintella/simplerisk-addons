using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using GUIClient.Services;
using Microsoft.Extensions.Localization;

namespace GUIClient.Tools;

public class Locator: IStringLocalizer
{

    private CultureInfo? _culture;
    
    public Locator(CultureInfo? culture =  null)
    {
        //if (culture == null) culture = new CultureInfo("en-US");
        _culture = culture;
    }
    
    
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        throw new System.NotImplementedException();
    }

    public LocalizedString this[string name]  {
        get
        {
            
            ResourceManager rm = new ResourceManager("GUIClient.Resources.Localization",
                typeof(Locator).Assembly);
            
            string? str;
            
            if (_culture == null)
            {
                str = rm.GetString(name);
                if (str == null)
                {
                    str = "";
                }
                return new LocalizedString(name, str);
            }
            str = rm.GetString(name, _culture);
            if (str == null)
            {
                str = "";
            }
            return new LocalizedString(name, str);
        }
     
    }

    public LocalizedString this[string name, params object[] arguments] => this[name];
}