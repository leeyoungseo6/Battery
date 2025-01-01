using EventSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameBatterySelectState : InGameState
{
    private Battery _currentBattery;
    
    public InGameBatterySelectState(MainUI mainUI, InGameStateMachine stateMachine) : base(mainUI, stateMachine)
    {
        mainUI.eventChannel.AddListener<SelectBatteryEvent>(OnSelectBattery);
    }

    public override void Enter()
    {
        _mainUI.grid.batteries.ForEach(b => b.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent));
        _mainUI.grid.batteries.ForEach(b => b.RegisterCallback<MouseUpEvent>(OnMouseUpEvent));
    }

    public override void Exit()
    {
        _mainUI.grid.batteries.ForEach(b => b.UnregisterCallback<MouseMoveEvent>(OnMouseMoveEvent));
        _mainUI.grid.batteries.ForEach(b => b.UnregisterCallback<MouseUpEvent>(OnMouseUpEvent));
    }

    private void OnSelectBattery(SelectBatteryEvent evt)
    {
        _currentBattery = evt.target;
    }

    private void OnMouseMoveEvent(MouseMoveEvent evt)
    {
        Node node = _mainUI.grid.GetNode(evt.mousePosition);
        if (node.HasBattery(out var target))
        {
            if (_currentBattery == target) return;

            _currentBattery.Connect(target);
        }
    }

    private void OnMouseUpEvent(MouseUpEvent evt)
    {
        _currentBattery.EndConnection();
        
        _stateMachine.ChangeState(InGameStateEnum.Base);
    }
}