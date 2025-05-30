using System;
using UnityEngine;

namespace HairvestMoon.Utility
{
    // Custom event object for broadcasting bool values between decoupled systems
    // Register/Unregister listeners and call Raise(bool)


    [CreateAssetMenu(menuName = "Events/BoolEvent")]
    public class BoolEvent : ScriptableObject
    {
        private Action<bool> _listeners;

        public void Raise(bool value) => _listeners?.Invoke(value);
        public void Register(Action<bool> callback) => _listeners += callback;
        public void Unregister(Action<bool> callback) => _listeners -= callback;
    }
}