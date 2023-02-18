using UnityEngine;
using UnityEngine.Events;

namespace Coots.GameEventSystem
{
    /// <summary>
    /// Listener for game events.
    /// Drag the GameEvent's scriptable object into the inspector to subscribe.
    /// Any number of game events can trigger any number of unity events.
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] protected GameEvent[] _gameEvents;
        [SerializeField] protected UnityEvent[] _unityEvents;

        void Awake()
        {
            foreach(GameEvent gameEvent in _gameEvents)
            {
                gameEvent.Register(this);
            }
        }
        void OnDestroy()
        {
            foreach (GameEvent gameEvent in _gameEvents)
            {
                gameEvent.Deregister(this);
            }
        }

        public virtual void RaiseEvent()
        {
            foreach (UnityEvent unityEvent in _unityEvents)
            {
                unityEvent.Invoke();
            }
        }
    }
}