using System;
using EventSystem;
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
    
    public VisualElement Root;
    public Grid grid;
    public BatteryPanel batteryPanel;
    public VisualElement edgeContainer;

    private InGameStateMachine stateMachine;

    public GameEventChannelSO eventChannel;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        stateMachine = new InGameStateMachine();
        
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
        Root = _uiDocument.rootVisualElement;

        var corePanel = Root.Q("CorePanel");
        grid = new Grid(this);
        corePanel.Add(grid);
        
        batteryPanel = new BatteryPanel(this);
        batteryPanel.CreateBattery(new[]{1, 2, 3});
        Root.Add(batteryPanel);
        
        edgeContainer = Root.Q("EdgeContainer");
    }
}