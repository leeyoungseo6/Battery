namespace EventSystem
{
    public static class UIEvents
    {
        public static SelectBatteryEvent SelectBatteryEvent = new();
    }

    public class SelectBatteryEvent : GameEvent
    {
        public Battery target;
    }
}