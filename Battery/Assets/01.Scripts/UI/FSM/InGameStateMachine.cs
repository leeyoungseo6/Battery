using System.Collections.Generic;

public class InGameStateMachine
{
    public InGameState CurrentState { get; private set; }
    public Dictionary<InGameStateEnum, InGameState> stateDictionary = new();

    public void Initialize(InGameStateEnum startState)
    {
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(InGameStateEnum newState)
    {
        CurrentState.Exit();
        CurrentState = stateDictionary[newState];
        CurrentState.Enter();
    }
    
    public void AddState(InGameStateEnum stateEnum, InGameState playerState)
    {
        stateDictionary.Add(stateEnum, playerState);
    }
}