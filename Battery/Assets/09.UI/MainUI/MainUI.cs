using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum InGameStateEnum
{
    Base,
    BatteryMove,
    BatterySelect
}

public class MainUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    public Grid grid;
    public BatteryPanel batteryPanel;

    private InGameStateMachine stateMachine;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        stateMachine = new InGameStateMachine();
        Debug.Log(Type.GetType("InGameBaseState"));
        
        foreach (InGameStateEnum stateEnum in Enum.GetValues(typeof(InGameStateEnum)))
        {
            string typeName = stateEnum.ToString(); // Idle, Run, Fall

            try
            {
                Type t = Type.GetType($"InGame{typeName}State");
                InGameState state = Activator.CreateInstance(t, this, stateMachine) as InGameState;
                stateMachine.AddState(stateEnum, state);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e} is loading error check Message");
                Debug.Log(e.Message);
            }
        }
    }

    private void Start()
    {
        stateMachine.Initialize(InGameStateEnum.Base);
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;

        grid = root.Q<Grid>("Grid");
        batteryPanel = root.Q<BatteryPanel>("BatteryPanel");
        batteryPanel.Initialize(this);
        batteryPanel.CreateBattery(new[]{1, 2, 3});
    }
}