using UnityEngine.UIElements;

[UxmlElement]
public partial class Node : VisualElement
{
    public int x, y;
    
    public Node()
    {
        AddToClassList("node");
    }
    
    public Node(string name, int x, int y, bool isCore)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        AddToClassList("node-bg");
        var child = new VisualElement();
        child.AddToClassList("node");
        Add(child);
        
    }
}