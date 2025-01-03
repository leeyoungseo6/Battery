using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Battery : VisualElement
{
    private MainUI _mainUI;

    public int power;

    private bool _isInserted;
    
    private Node _currentNode;

    public bool IsDragging => _isDragging;
    private bool _isDragging;
    
    private Vector2 _startPos;
    
    public Battery()
    {
        AddToClassList("battery");
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            var power = new VisualElement();
            power.AddToClassList("power");
            Add(power);
        }
    }
    
    public Battery(int powerValue)
    {
        power = powerValue;
        AddToClassList("battery");

        for (int i = 0; i < powerValue; i++)
        {
            var powerVisual = new VisualElement();
            powerVisual.AddToClassList("power");
            powerVisual.pickingMode = PickingMode.Ignore;
            Add(powerVisual);
        }
    }

    public void Initialize(MainUI mainUI)
    {
        _mainUI = mainUI;
    }

    public void StartDrag(MouseDownEvent evt)
    {
        _isDragging = true;
        _startPos = evt.localMousePosition;
        _mainUI.Root.Add(this);
        
        this.CaptureMouse();

        Vector2 offset = evt.mousePosition - _startPos;

        style.position = Position.Absolute;
        style.left = offset.x;
        style.top = offset.y;
        style.width = new Length(layout.width, LengthUnit.Pixel);
        AddToClassList("dragging");
    }

    public void Move(Vector2 pos)
    {
        if (this.HasMouseCapture() == false) return;

        Vector2 diff = pos - _startPos;
        float x = layout.x;
        float y = layout.y;

        style.left = x + diff.x;
        style.top = y + diff.y;
    }

    public void EndDrag(Vector2 mousePos)
    {
        this.ReleaseMouse();

        _isDragging = false;
        style.position = Position.Relative;
        style.left = StyleKeyword.Null;
        style.top = StyleKeyword.Null;
        style.width = StyleKeyword.Null;
        RemoveFromClassList("dragging");

        Node node = _mainUI.grid.GetNode(mousePos);
        if (node == null)
        {
            if (_isInserted == false) return;
            
            _isInserted = false;
            _currentNode.UnEquipBattery();
            _mainUI.grid.RemoveBattery(this);
            _mainUI.batteryPanel.AddBattery(this);
        }
        else if (_currentNode != node)
        {
            _isInserted = true;
            _currentNode = node;
            _currentNode.EquipBattery(this);
            _mainUI.batteryPanel.RemoveBattery(this);
            _mainUI.grid.AddBattery(this);
        }
        else
        {
            _currentNode.Add(this);
        }
    }

    public void CreateEdge()
    {
        _currentNode.CreateEdge();
    }

    public void Connect(Node node)
    {
        _currentNode.Connect(node);
    }

    public void EndConnection()
    {
        _currentNode.EndConnection();
    }
}