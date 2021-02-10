using UnityEngine;

namespace Com.Septyr.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "ushort Event Listener")]
    public sealed class UShortGameEventListener : BaseGameEventListener<ushort, UShortGameEvent, UShortUnityEvent>
    {
    }
}