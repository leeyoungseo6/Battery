using UnityEngine;
using UnityEngine.UIElements;

public enum Direction
{
    Up, Down, Left, Right
}

[UxmlElement]
public partial class Edge : VisualElement
{
    private MainUI _mainUI;

    private VisualElement _edge;

    public Direction direction;
    public Node next;
    
    public Edge()
    {
        AddToClassList("pivot");
        var edge = new VisualElement();
        edge.AddToClassList("edge");
        Add(edge);
    }
    
    public Edge(MainUI mainUI, Node node)
    {
        _mainUI = mainUI;
        _mainUI.edgeContainer.Add(this);
        AddToClassList("pivot");
        _edge = new VisualElement();
        _edge.AddToClassList("edge");
        Add(_edge);

        Vector2 pos = node.layout.position;
        style.left = pos.x;
        style.top = pos.y;
        visible = false;
    }

    public void AddToEdgeClassList(string className)
    {
        _edge.AddToClassList(className);
    }
    
    public void RemoveFromEdgeClassList(string className)
    {
        _edge.RemoveFromClassList(className);
    }
}