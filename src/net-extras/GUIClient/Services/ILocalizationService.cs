using System.Resources;
using Microsoft.Extensions.Localization;

namespace GUIClient.Services;

public interface ILocalizationService
{
    IStringLocalizer GetLocalizer();

    ResourceManager GetResourceManager();
}