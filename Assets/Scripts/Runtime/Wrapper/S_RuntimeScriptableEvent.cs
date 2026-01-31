using System;
using UnityEngine;

namespace BT.ScriptablesObject
{
    public class RuntimeScriptableEvent : ScriptableObject
    {
        public event Action action;

        public void Call() => action?.Invoke();

        private void InvokeOnValueChanged()
        {
            action?.Invoke();
        }
    }

    public class RuntimeScriptableEvent<T> : ScriptableObject
    {
        public event Action<T> action;

        public void Call(T a) => action?.Invoke(a);

        private void InvokeOnValueChanged(T a)
        {
            action?.Invoke(a);
        }
    }

    public class RuntimeScriptableEvent<T1, T2> : ScriptableObject
    {
        public event Action<T1, T2> action;

        public void Call(T1 a1, T2 a2) => action?.Invoke(a1, a2);

        private void InvokeOnValueChanged(T1 a1, T2 a2)
        {
            action?.Invoke(a1, a2);
        }
    }

    public class RuntimeScriptableEvent<T1, T2, T3> : ScriptableObject
    {
        public event Action<T1, T2, T3> action;

        public void Call(T1 a1, T2 a2, T3 a3) => action?.Invoke(a1, a2, a3);

        private void InvokeOnValueChanged(T1 a1, T2 a2, T3 a3)
        {
            action?.Invoke(a1, a2, a3);
        }
    }
}