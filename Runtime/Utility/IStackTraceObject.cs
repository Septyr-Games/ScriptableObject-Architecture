using System.Collections.Generic;

namespace Septyr.ScriptableObjectArchitecture
{
    public interface IStackTraceObject
    {
        List<StackTraceEntry> StackTraces { get; }

        void AddStackTrace();
        void AddStackTrace(object value);
    } 
}