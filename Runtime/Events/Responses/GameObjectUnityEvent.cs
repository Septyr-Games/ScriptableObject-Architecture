using UnityEngine;
using UnityEngine.Events;

namespace Septyr.ScriptableObjectArchitecture
{
    [System.Serializable]
    public sealed class GameObjectUnityEvent : UnityEvent<GameObject>
    {
    } 
}