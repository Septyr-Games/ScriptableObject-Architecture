using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Septyr.ScriptableObjectArchitecture.Editor
{
    public interface IPropertyDrawIterator : IPropertyIterator
    {
        void Draw();
    } 
}
