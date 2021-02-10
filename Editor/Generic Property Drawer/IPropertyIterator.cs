using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Septyr.ScriptableObjectArchitecture.Editor
{
    public interface IPropertyIterator
    {
        bool Next();
        void End();
    }

}