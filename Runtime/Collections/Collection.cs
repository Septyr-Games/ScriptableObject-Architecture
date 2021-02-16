using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Septyr.ScriptableObjectArchitecture
{
    public class Collection<T> : BaseCollection, IEnumerable<T>
    {
        public new T this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        [SerializeField]
        private bool _readOnly = false;
        [SerializeField]
        private bool _raiseWarning = true;
        [SerializeField]
        private List<T> _list = new List<T>();
        [SerializeField]
        private bool _isFixedSize = false;
        [SerializeField, Min(2)]
        private int _fixedSize = 2;

        public override bool ReadOnly => _readOnly;
        public override IList List => _list;
        public override Type Type => typeof(T);
        public override bool IsFixedSize => _isFixedSize;
        public override int FixedSize
        {
            get
            {
                return Math.Max(_fixedSize, 0);
            }
            set
            {
                while (_list.Count > value)
                {
                    _list.RemoveAt(_list.Count - 1);
                }
                for (int i = _list.Count - 1; i < value; i++)
                {
                    _list.Add(default(T));
                }
                _list.Capacity = value;
                _fixedSize = value;
            }
        }

        public void Add(T obj)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return;
            }
            if (_fixedSize > 0 && _list.Count >= _fixedSize)
                throw new ArgumentOutOfRangeException();

            _list.Add(obj);
            _list.Capacity = _fixedSize;
        }
        public void Remove(T obj)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return;
            }
            _list.Remove(obj);
            _list.Capacity = _fixedSize;
        }
        public void Clear()
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return;
            }
            _list.Clear();
        }
        public bool Contains(T value)
        {
            return _list.Contains(value);
        }
        public int IndexOf(T value)
        {
            return _list.IndexOf(value);
        }
        public void RemoveAt(int index)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return;
            }
            _list.RemoveAt(index);
        }
        public void Insert(int index, T value)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return;
            }
            if (_fixedSize > 0 && _list.Count >= _fixedSize)
                throw new ArgumentOutOfRangeException();

            _list.Insert(index, value);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        public override string ToString()
        {
            return "Collection<" + typeof(T) + ">(" + Count + ")";
        }
        public T[] ToArray()
        {
            return _list.ToArray();
        }
        private void RaiseReadonlyWarning()
        {
            if (!_raiseWarning)
                return;

            Debug.LogWarning("Tried to set value on " + name + ", but value is readonly!", this);
        }
#if UNITY_EDITOR
        public override void Reset()
        {
            _list.Clear();
            if (_isFixedSize)
                _list.Capacity = _fixedSize;
            else
            {
                _fixedSize = 0;
                _list.Capacity = 0;
            }
        }
#endif
    }
}
