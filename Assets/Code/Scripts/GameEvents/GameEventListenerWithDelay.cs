using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Coots.GameEventSystem
{
    /// <summary>
    /// Extention for the game event listener that allows for a delay.
    /// </summary>
    public class GameEventListenerWithDelay : GameEventListener
    {
        [SerializeField] float _delay = 1f;
        [SerializeField] UnityEvent[] _delayedUnityEvents;

        void Awake()
        {
            foreach (GameEvent gameEvent in _gameEvents)
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
        public override void RaiseEvent()
        {
            foreach (UnityEvent unityEvent in _unityEvents)
            {
                unityEvent.Invoke();
            }
            StartCoroutine(RunDelayedEvents());
        }

        private IEnumerator RunDelayedEvents()
        {
            yield return new WaitForSeconds(_delay);
            foreach (UnityEvent delayedEvent in _delayedUnityEvents)
            {
                delayedEvent.Invoke();
            }
        }
    }
}