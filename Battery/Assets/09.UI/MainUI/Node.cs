using System.Collections.Generic;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Node : VisualElement
{
    public int x, y;

    private MainUI _mainUI;
    
    private Battery _currentBattery;
    private Dictionary<Direction, Edge> _edges;
    
    public Node()
    {
        AddToClassList("node");
    }
    
    public Node(MainUI mainUI, string name, int x, int y)
    {
        _mainUI = mainUI;
        this.name = name;
        this.x = x;
        this.y = y;
        AddToClassList("node-bg");
        var child = new VisualElement();
        child.AddToClassList("node");
        Add(child);
    }

    public void EquipBattery(Battery battery)
    {
        Add(battery);
        _currentBattery = battery;
    }

    public void UnEquipBattery()
    {
        _currentBattery = null;
    }

    public bool HasBattery(out Battery battery)
    {
        battery = _currentBattery;
        return _currentBattery != null;
    }
}