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
    
    private Dictionary<Direction, Edge> _edges;
    private Edge _currentEdge, _dummyEdge;

    private bool _isAlign, _isCross;
    private Direction _prevDir;
    
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
        _edges = new();
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

    public void CreateEdge(MouseDownEvent evt)
    {
        //Vector2 offset = evt.mousePosition - _mainUI.grid.worldBound.position - evt.localMousePosition;
        _currentEdge = new Edge(_mainUI, this);
    }

    public void Connect(Battery battery)
    {
        var otherEdges = battery.GetEdges();
        
        Vector2 myPos = worldBound.position;
        Vector2 otherPos = battery.worldBound.position;
        Vector2 dir = otherPos - myPos;

        CalculateDirection(dir, out var eDir, out var angle);
        ValidateDirection(eDir, otherEdges);

        _prevDir = _currentEdge.direction = eDir;
        _currentEdge.style.rotate = new StyleRotate(new Rotate(new Angle(angle)));
    }

    private void CalculateDirection(Vector2 dir, out Direction eDir, out float angle)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0)
            {
                angle = 0;
                eDir = Direction.Right;
            }
            else
            {
                angle = 180;
                eDir = Direction.Left;
            }
        }
        else
        {
            if (dir.y > 0)
            {
                angle = 90;
                eDir = Direction.Down;
            }
            else
            {
                angle = -90;
                eDir = Direction.Up;
            }
        }
    }

    private void ValidateDirection(Direction eDir, Dictionary<Direction, Edge> otherEdges)
    {
        if (_prevDir != eDir && _dummyEdge != null)
        {
            if (_isAlign)
            {
                _isAlign = false;
                _mainUI.edgeContainer.Add(_dummyEdge);
                _edges.Add(_dummyEdge.direction, _dummyEdge);
                _currentEdge.RemoveFromEdgeClassList("rejected");
            }

            if (_isCross)
            {
                _isCross = false;
                _mainUI.edgeContainer.Add(_dummyEdge);
                otherEdges.Add(_dummyEdge.direction, _dummyEdge);
            }

            _dummyEdge = null;
        }
        
        if (_edges.ContainsKey(eDir))
        {
            _isAlign = true;
            _dummyEdge = _edges[eDir];
            _mainUI.edgeContainer.Remove(_dummyEdge);
            _edges.Remove(_dummyEdge.direction);
            _currentEdge.AddToEdgeClassList("rejected");
            return;
        }

        Direction opposite = eDir.Opposite();
        if (otherEdges.ContainsKey(opposite))
        {
            _isCross = true;
            _dummyEdge = otherEdges[opposite];
            _mainUI.edgeContainer.Remove(_dummyEdge);
            otherEdges.Remove(_dummyEdge.direction);
        }
    }

    public void EndConnection()
    {
        if (_isAlign)
        {
            _isAlign = false;
            _currentEdge.RemoveFromEdgeClassList("rejected");
            _mainUI.edgeContainer.Remove(_currentEdge);
            _dummyEdge = null;
        }
        else if (_isCross)
        {
            _isCross = false;
            _edges.Add(_currentEdge.direction, _currentEdge);
            _dummyEdge = null;
        }
        else if (_currentEdge != null)
        {
            _edges.Add(_currentEdge.direction, _currentEdge);
        }
        
        _currentEdge = null;
    }

    public Dictionary<Direction, Edge> GetEdges() => _edges;
}