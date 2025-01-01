using UnityEngine;
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
            _stateMachine.ChangeState(InGameStateEnum.BatterySelect);
        else
        {
            Debug.Log((evt.target as Battery).name);
            _stateMachine.ChangeState(InGameStateEnum.BatteryMove);
        }
    }

    private void OnPanelBatteryClicked(MouseDownEvent evt)
    {
        (evt.target as Battery).StartDrag(evt);
        _stateMachine.ChangeState(InGameStateEnum.BatteryMove);
    }
}