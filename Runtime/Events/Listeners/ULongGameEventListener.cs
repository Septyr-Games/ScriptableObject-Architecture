using UnityEngine;

namespace Com.Septyr.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "ulong Event Listener")]
    public sealed class ULongGameEventListener : BaseGameEventListener<ulong, ULongGameEvent, ULongUnityEvent>
    {
    }
}