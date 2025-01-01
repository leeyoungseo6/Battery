public class InGameState
{
    protected MainUI _mainUI;
    protected InGameStateMachine _stateMachine;
    
    public InGameState(MainUI mainUI, InGameStateMachine stateMachine)
    {
        _mainUI = mainUI;
        _stateMachine = stateMachine;
    }
    
    public virtual void Enter(){}
    public virtual void Exit(){}
}