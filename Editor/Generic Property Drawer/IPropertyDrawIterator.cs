using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Septyr.ScriptableObjectArchitecture.Editor
{
    public interface IPropertyDrawIterator : IPropertyIterator
    {
        void Draw();
    } 
}
