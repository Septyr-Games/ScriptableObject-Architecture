using UnityEngine;
using UnityEngine.Events;

namespace Com.Septyr.ScriptableObjectArchitecture
{
    [System.Serializable]
    public sealed class GameObjectUnityEvent : UnityEvent<GameObject>
    {
    } 
}