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
        (evt.target as Battery).Move(evt.localMousePosition);
    }

    private void OnBatteryReleased(MouseUpEvent evt)
    {
        (evt.target as Battery).EndDrag();
    }
}