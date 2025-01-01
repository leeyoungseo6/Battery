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
    
    public BatteryPanel(MainUI mainUI)
    {
        _mainUI = mainUI;
        AddToClassList("battery-panel");
        batteries = new();
    }

    public void CreateBattery(int[] batteryPowers)
    {
        for (int i = 0; i < batteryPowers.Length; i++)
        {
            Battery battery = new Battery(batteryPowers[i]);
            battery.Initialize(_mainUI);
            battery.name = $"Battery{i + 1}";
            Add(battery);
            
            batteries.Add(battery);
        }
    }

    public void AddBattery(Battery battery)
    {
        Add(battery);
        batteries.Add(battery);
    }

    public void RemoveBattery(Battery battery)
    {
        foreach (var item in batteries)
        {
            if (item == battery)
            {
                batteries.Remove(battery);
                break;
            }
        }
    }
}