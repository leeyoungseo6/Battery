using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Battery : VisualElement
{
    private MainUI _mainUI;

    private bool _isInserted;

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
        AddToClassList("battery");
        for (int i = 0; i < powerValue; i++)
        {
            var power = new VisualElement();
            power.AddToClassList("power");
            Add(power);
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
        
        this.CaptureMouse();
        
        Vector2 offset = evt.mousePosition - _mainUI.batteryPanel.worldBound.position - _startPos;

        style.position = Position.Absolute;
        style.left = offset.x;
        style.top = offset.y;
        style.width = new Length(layout.width, LengthUnit.Pixel);
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

    public void EndDrag()
    {
        this.ReleaseMouse();
        
        style.position = Position.Relative;
        style.left = StyleKeyword.Null;
        style.top = StyleKeyword.Null;
        style.width = StyleKeyword.Null;
    }
}