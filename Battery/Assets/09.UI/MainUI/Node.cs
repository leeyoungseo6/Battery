using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Node : VisualElement
{
    public int x, y;

    private MainUI _mainUI;
    
    private Battery _currentBattery;
    private Dictionary<Direction, Edge> _edges;
    private Edge _currentEdge, _dummyEdge;
    private Node _dummyNode;

    private bool _isAlign, _isCross;
    private Direction _prevDir;
    
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
        _edges = new();
        
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

    public bool HasBattery() => _currentBattery != null;
    
    public void CreateEdge()
    {
        _currentEdge = new Edge(_mainUI, this);
    }

    public void Connect(Node otherNode)
    {
        if (_currentEdge != null)
        {
            if (this != otherNode)
                _currentEdge.visible = true;
            else
            {
                _currentEdge.visible = false;
                return;
            }
        }
        
        Vector2 myPos = worldBound.position;
        Vector2 otherPos = otherNode.worldBound.position;
        Vector2 dir = otherPos - myPos;

        CalculateDirection(dir, out var eDir, out var angle);
        ValidateDirection(eDir, otherNode);

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

    private void ValidateDirection(Direction eDir, Node otherNode)
    {
        var otherEdges = otherNode.GetEdges();
        
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
                _dummyNode.AddEdge(_dummyEdge);
                _dummyNode = null;
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
            _dummyNode = otherNode;
            _dummyEdge = otherEdges[opposite];
            _mainUI.edgeContainer.Remove(_dummyEdge);
            otherNode.RemoveEdge(opposite);
            otherEdges.Values.Print();
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
            if (_currentEdge.visible == false)
                _mainUI.edgeContainer.Remove(_currentEdge);
            else
                _edges.Add(_currentEdge.direction, _currentEdge);
        }
        
        _currentEdge = null;
    }

    private Dictionary<Direction, Edge> GetEdges() => _edges;
    private void AddEdge(Edge edge) => _edges.Add(edge.direction, edge);
    private void RemoveEdge(Direction dir) => _edges.Remove(dir);
}