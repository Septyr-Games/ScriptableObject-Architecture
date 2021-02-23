using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Septyr.ScriptableObjectArchitecture.Editor
{
    public interface IPropertyIterator
    {
        bool Next();
        void End();
    }

}