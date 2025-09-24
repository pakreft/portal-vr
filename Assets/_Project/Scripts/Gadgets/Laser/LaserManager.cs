using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PortalVR.Gadgets.Laser
{
    public class LaserManager : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnAllReceiversHit;

        [SerializeField]
        private UnityEvent OnReceiversExit;

        private readonly List<LaserEmitter> _emitters = new();
        private readonly HashSet<Receiver> _receivers = new();
        private readonly HashSet<Trigger> _triggers = new();

        private readonly HashSet<IHittable> _currentHits = new();
        private readonly HashSet<IHittable> _previousHits = new();

        private bool _allReceiversHit;

        private void Update()
        {
            Step();
        }

        public void Register<T>(T item)
        {
            switch (item)
            {
                case LaserEmitter emitter:
                    _emitters.Add(emitter);
                    break;
                case Receiver receiver:
                    _receivers.Add(receiver);
                    break;
                case Trigger trigger:
                    _triggers.Add(trigger);
                    break;
            }
        }

        public void Unregister<T>(T item)
        {
            switch (item)
            {
                case LaserEmitter emitter:
                    _emitters.Remove(emitter);
                    break;
                case Receiver receiver:
                    _receivers.Remove(receiver);
                    break;
                case Trigger trigger:
                    _triggers.Remove(trigger);
                    break;
            }
        }

        private void Step()
        {
            _previousHits.Clear();
            _previousHits.UnionWith(_currentHits);
            _currentHits.Clear();

            FireLasers();
            FireEnterExitFuncs();
            HandleCompletion();
        }

        private void FireLasers()
        {
            foreach (var emitter in _emitters)
            {
                var hits = emitter.FireLaser();
                _currentHits.UnionWith(hits);
            }
        }

        private void FireEnterExitFuncs()
        {
            foreach (var hit in _currentHits)
            {
                if (!_previousHits.Contains(hit))
                    hit.LaserEnter(null);
            }

            foreach (var prev in _previousHits)
            {
                if (!_currentHits.Contains(prev))
                    prev.LaserExit(null);
            }
        }

        private void HandleCompletion()
        {
            var allNowHit = AreAllReceiversAndTriggersHit();
            if (allNowHit && !_allReceiversHit)
            {
                _allReceiversHit = true;
                OnAllReceiversHit?.Invoke();
            }
            else if (!allNowHit && _allReceiversHit)
            {
                _allReceiversHit = false;
                OnReceiversExit?.Invoke();
            }
        }

        private bool AreAllReceiversAndTriggersHit()
        {
            var required = new HashSet<IHittable>(_receivers);
            required.UnionWith(_triggers);
            return _currentHits.IsSupersetOf(required);
        }
    }
}
