using UnityEngine;

namespace Com.Septyr.ScriptableObjectArchitecture
{
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "AudioClip Event Listener")]
    public sealed class AudioClipGameEventListener : BaseGameEventListener<AudioClip, AudioClipGameEvent, AudioClipUnityEvent>
    {

    }
}
