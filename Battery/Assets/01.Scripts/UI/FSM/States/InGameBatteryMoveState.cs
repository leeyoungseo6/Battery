using UnityEngine.UIElements;

public class InGameBatteryMoveState : InGameState
{
    public InGameBatteryMoveState(MainUI mainUI, InGameStateMachine stateMachine) : base(mainUI, stateMachine)
    {
    }

    public override void Enter()
    {
        _mainUI.grid.batteries.ForEach(b => b.RegisterCallback<MouseMoveEvent>(OnBatteryMoved));
        _mainUI.grid.batteries.ForEach(b => b.RegisterCallback<MouseUpEvent>(OnBatteryReleased));
        _mainUI.batteryPanel.batteries.ForEach(b => b.RegisterCallback<MouseMoveEvent>(OnBatteryMoved));
        _mainUI.batteryPanel.batteries.ForEach(b => b.RegisterCallback<MouseUpEvent>(OnBatteryReleased));
    }

    public override void Exit()
    {
        _mainUI.grid.batteries.ForEach(b => b.UnregisterCallback<MouseMoveEvent>(OnBatteryMoved));
        _mainUI.grid.batteries.ForEach(b => b.UnregisterCallback<MouseUpEvent>(OnBatteryReleased));
        _mainUI.batteryPanel.batteries.ForEach(b => b.UnregisterCallback<MouseMoveEvent>(OnBatteryMoved));
        _mainUI.batteryPanel.batteries.ForEach(b => b.UnregisterCallback<MouseUpEvent>(OnBatteryReleased));
    }

    private void OnBatteryMoved(MouseMoveEvent evt)
    {
        if (evt.target is Battery { IsDragging: true } battery)
            battery.Move(evt.localMousePosition);
    }

    private void OnBatteryReleased(MouseUpEvent evt)
    {
        if (evt.target is Battery { IsDragging: true } battery)
            battery.EndDrag(evt.mousePosition);
        
        _stateMachine.ChangeState(InGameStateEnum.Base);
    }
}