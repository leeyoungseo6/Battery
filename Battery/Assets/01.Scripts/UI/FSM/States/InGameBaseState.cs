using EventSystem;
using UnityEngine.UIElements;

public class InGameBaseState : InGameState
{
    public InGameBaseState(MainUI mainUI, InGameStateMachine stateMachine) : base(mainUI, stateMachine)
    {
    }
    
    public override void Enter()
    {
        _mainUI.grid.batteries.ForEach(b => b.RegisterCallback<MouseDownEvent>(OnGridBatteryClicked));
        _mainUI.batteryPanel.batteries.ForEach(b => b.RegisterCallback<MouseDownEvent>(OnPanelBatteryClicked));
    }

    public override void Exit()
    {
        _mainUI.grid.batteries.ForEach(b => b.UnregisterCallback<MouseDownEvent>(OnGridBatteryClicked));
        _mainUI.batteryPanel.batteries.ForEach(b => b.UnregisterCallback<MouseDownEvent>(OnPanelBatteryClicked));
    }

    private void OnGridBatteryClicked(MouseDownEvent evt)
    {
        if (evt.clickCount == 2)
        {
            evt.StopPropagation();
            Battery battery = evt.target as Battery;
            UIEvents.SelectBatteryEvent.target = battery;
            _mainUI.eventChannel.RaiseEvent(UIEvents.SelectBatteryEvent);
            battery.CreateEdge(evt);
            _stateMachine.ChangeState(InGameStateEnum.BatterySelect);
        }
        else
        {
            (evt.target as Battery).StartDrag(evt);
            _stateMachine.ChangeState(InGameStateEnum.BatteryMove);
        }
    }

    private void OnPanelBatteryClicked(MouseDownEvent evt)
    {
        if (evt.target is Battery battery)
        {
            battery.StartDrag(evt);
            _stateMachine.ChangeState(InGameStateEnum.BatteryMove);
        }
    }
}