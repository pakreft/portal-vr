using System.Collections.Generic;
using UnityEngine;

namespace PortalVR.Gadgets.Laser
{
    public class LaserManager : MonoBehaviour
    {
        [Header("Discovered Components")]
        [SerializeField]
        private List<LaserEmitter> _emitters = new();

        [SerializeField]
        private List<Trigger> _triggers = new();

        [SerializeField]
        private List<Receiver> _receivers = new();

        public IReadOnlyList<LaserEmitter> Emitters => _emitters;
        public IReadOnlyList<Trigger> Triggers => _triggers;
        public IReadOnlyList<Receiver> Receivers => _receivers;

        private readonly HashSet<IHittable> _currentHits = new();
        private readonly HashSet<IHittable> _lastHits = new();

        private void Awake()
        {
            GetComponentsInChildren(_emitters);
            GetComponentsInChildren(_triggers);
            GetComponentsInChildren(_receivers);
        }
    }
}
