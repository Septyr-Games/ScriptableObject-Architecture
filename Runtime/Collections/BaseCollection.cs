using System.Collections;
using Type = System.Type;

namespace Septyr.ScriptableObjectArchitecture
{
    public abstract class BaseCollection : SOArchitectureBaseObject, IEnumerable
    {
        public object this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        public int Count => List.Count;

        public abstract bool ReadOnly { get; }
        public abstract IList List { get; }
        public abstract Type Type { get; }
        public abstract bool IsFixedSize { get; }
        public abstract int FixedSize { get; set; }

        IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();
        public bool Contains(object obj) => List.Contains(obj);
#if UNITY_EDITOR
        public abstract void Reset();
#endif
    }
}