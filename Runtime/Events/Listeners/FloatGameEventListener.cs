using UnityEngine;

namespace Com.Septyr.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "float Event Listener")]
    public sealed class FloatGameEventListener : BaseGameEventListener<float, FloatGameEvent, FloatUnityEvent>
    {
    }
}