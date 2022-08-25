using System;
using System.Collections.Generic;
using System.Linq;

namespace GUIClient.ViewModels;

public class DeviceViewModel: ViewModelBase
{
    public List<String> TestData { get; set; } = new List<string>();

    public DeviceViewModel()
    {
        TestData.Add("test1");
        TestData.Add("teste2");
    }
    
}