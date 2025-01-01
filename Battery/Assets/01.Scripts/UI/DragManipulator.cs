using UnityEngine;
using UnityEngine.UIElements;

public class DragManipulator : MouseManipulator
{
    public delegate void MouseEvent(VisualElement target, Vector2 pos);

    private MouseEvent _onDragStart, _onDragEnd, _doubleClicked;
    private VisualElement _root;

    private Vector2 _startPos;
    private bool _isDragging;

    private float _clickWindow;

    public DragManipulator(VisualElement root, MouseEvent onDragStart, MouseEvent onDragEnd, MouseEvent doubleClicked)
    {
        _root = root;
        _onDragStart = onDragStart;
        _onDragEnd = onDragEnd;
        _doubleClicked = doubleClicked;
        
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }
    
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(HandleMouseDown);
        target.RegisterCallback<MouseMoveEvent>(HandleMouseMove);
        target.RegisterCallback<MouseUpEvent>(HandleMouseUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(HandleMouseDown);
        target.UnregisterCallback<MouseMoveEvent>(HandleMouseMove);
        target.UnregisterCallback<MouseUpEvent>(HandleMouseUp);
    }

    private void HandleMouseDown(MouseDownEvent evt)
    {
        if (CanStartManipulation(evt) == false) return;
        if (evt.clickCount == 2)
        {
            _doubleClicked?.Invoke(target, evt.mousePosition);
            return;
        }

        _startPos = evt.localMousePosition;
        
        //_root.Add(target);
        _isDragging = true;
        target.CaptureMouse();
        
        Vector2 offset = evt.mousePosition - _root.worldBound.position - _startPos;

        target.style.position = Position.Absolute;
        target.style.left = offset.x;
        target.style.top = offset.y;
        target.style.width = new Length(target.layout.width, LengthUnit.Pixel);
        
        _onDragStart?.Invoke(target, evt.mousePosition);
    }

    private void HandleMouseMove(MouseMoveEvent evt)
    {
        if (_isDragging == false || target.HasMouseCapture() == false) return;

        Vector2 diff = evt.localMousePosition - _startPos;
        float x = target.layout.x;
        float y = target.layout.y;

        target.style.left = x + diff.x;
        target.style.top = y + diff.y;
    }

    private void HandleMouseUp(MouseUpEvent evt)
    {
        if (CanStartManipulation(evt) == false) return;

        _isDragging = false;
        target.ReleaseMouse();
        
        target.style.position = Position.Relative;
        target.style.left = StyleKeyword.Null;
        target.style.top = StyleKeyword.Null;
        target.style.width = StyleKeyword.Null;
        
        _onDragEnd.Invoke(target, evt.mousePosition);
    }
}
