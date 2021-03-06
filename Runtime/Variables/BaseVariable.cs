﻿using UnityEngine;
using UnityEngine.Events;

namespace Septyr.ScriptableObjectArchitecture
{
    public abstract class BaseVariable : GameEventBase
    {
        public abstract bool ReadOnly { get; }
        public abstract bool IsClamped { get; }
        public abstract bool Clampable { get; }
        public abstract System.Type Type { get; }
        public abstract object BaseValue { get; set; }
#if UNITY_EDITOR
        public abstract void ResetValue();
#endif
    }
    public abstract class BaseVariable<T> : BaseVariable
    {
        public virtual T Value
        {
            get => _value;
            set
            {
                _value = SetValue(value);
                Raise();
            }
        }
        public virtual T MinClampValue => Clampable ? _minClampedValue : default;
        public virtual T MaxClampValue => Clampable ? _maxClampedValue : default;

        public override bool ReadOnly => _readOnly;
        public override bool Clampable => false;
        public override bool IsClamped => _isClamped && !_readOnly;
        public override System.Type Type => typeof(T);
        public override object BaseValue
        {
            get => _value;
            set
            {
                _value = SetValue((T)value);
                Raise();
            }
        }

        [SerializeField]
        private bool _readOnly = false;
        [SerializeField]
        private bool _raiseWarning = true;
        [SerializeField]
        protected T _value = default;
        [SerializeField]
        protected bool _isClamped = false;
        [SerializeField]
        protected T _minClampedValue = default;
        [SerializeField]
        protected T _maxClampedValue = default;

        public virtual T SetValue(BaseVariable<T> value) => SetValue(value.Value);
        public virtual T SetValue(T value)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return _value;
            }
            else if (Clampable && IsClamped)
                return ClampValue(value);

            return value;
        }
        protected virtual T ClampValue(T value) => value;
        private void RaiseReadonlyWarning()
        {
            if (!_readOnly || !_raiseWarning)
                return;

            Debug.LogWarning("Tried to set value on " + name + ", but value is readonly!", this);
        }

#if UNITY_EDITOR
        public override void ResetValue() => _value = default;
#endif

        public override string ToString() => _value == null ? "null" : _value.ToString();
        public static implicit operator T(BaseVariable<T> variable) => variable.Value;
    }
    public abstract class BaseVariable<T, TEvent> : BaseVariable<T> where TEvent : UnityEvent<T>
    {
        [SerializeField]
        private TEvent _event = default;

        public override T SetValue(T value)
        {
            T oldValue = _value;
            T newValue = base.SetValue(value);

            if (!newValue.Equals(oldValue))
                _event?.Invoke(newValue);
            
            return newValue;
        }
        public void AddListener(UnityAction<T> callback) => _event.AddListener(callback);
        public void RemoveListener(UnityAction<T> callback) => _event.RemoveListener(callback);
        public override void RemoveAll()
        {
            base.RemoveAll();
            _event.RemoveAllListeners();
        }
    }
}