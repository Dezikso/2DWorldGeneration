using System.Collections.Generic;
using UnityEngine;

namespace Core.EventChannels
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        readonly private HashSet<EventListener<T>> listeners = new();

        [SerializeField] private bool _isDebug = false;
        [SerializeField][TextArea] private string _description;

        public void Raise(T value)
        {
            foreach (var listener in listeners)
            {
                if (_isDebug)
                    Debug.Log($"Raising event to listener: {listener.gameObject.name} with value: {value}");

                listener.Raise(value);
            }
        }

        internal void Register(EventListener<T> listener) => listeners.Add(listener);
        internal void Deregister(EventListener<T> listener) => listeners.Remove(listener);
    }


    public class Empty { }

    [CreateAssetMenu(menuName = "Events/EventChannel")]
    public class EventChannel : EventChannel<Empty> { }

}