using System.Collections.Generic;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BatteryPanel : VisualElement
{
    private MainUI _mainUI;

    public List<Battery> batteries;
    
    public BatteryPanel()
    {
        AddToClassList("battery-panel");
        batteries = new();
    }

    public void Initialize(MainUI mainUI)
    {
        _mainUI = mainUI;
    }

    public void CreateBattery(int[] batteryPowers)
    {
        for (int i = 0; i < batteryPowers.Length; i++)
        {
            Battery battery = new Battery(batteryPowers[i]);
            battery.Initialize(_mainUI);
            Add(battery);
            
            batteries.Add(battery);
        }
    }
}