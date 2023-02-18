using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Coots.GameEventSystem
{
    /// <summary>
    /// Extention for the game event listener that only allows one trigger.
    /// </summary>
    public class GameEventListenerSingleUse : GameEventListener
    {
        private bool HasBeenRaised = false;
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
            if (HasBeenRaised)
            {
                return;
            }
            HasBeenRaised = true;
            foreach (UnityEvent unityEvent in _unityEvents)
            {
                unityEvent.Invoke();
            }
        }
    }
}