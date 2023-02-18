using System.Collections.Generic;
using UnityEngine;

namespace Coots.GameEventSystem
{
    /// <summary>
    /// Game event using scriptable objects. This helps it work throughout multiple scenes.
    /// Allows you to trigger Unity Events in editor.
    /// Create a scriptable object in the inspector to create a new event.
    /// </summary>
    [CreateAssetMenu(menuName = "Game Event", fileName = "New Game Event")]
    public class GameEvent : ScriptableObject
    {
        HashSet<GameEventListener> _listeners = new HashSet<GameEventListener>();

        public virtual void Invoke()
        {
            foreach (var globalEventListener in _listeners)
                globalEventListener.RaiseEvent();
        }

        public void Register(GameEventListener gameEventListener) => _listeners.Add(gameEventListener);
        public void Deregister(GameEventListener gameEventListener) => _listeners.Remove(gameEventListener);
    }

}
