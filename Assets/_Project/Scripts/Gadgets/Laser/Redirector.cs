using UnityEngine;

namespace PortalVR.Gadgets.Laser
{
    [RequireComponent(typeof(LaserEmitter))]
    public class Redirector : HittableBase
    {
        private LaserEmitter _emitter;

        private void Awake()
        {
            _emitter = GetComponent<LaserEmitter>();
        }

        private void Start()
        {
            // Disable in Start(), because in Awake() LaserEmitter.OnDisable does not get called, therefore not disabling LineRenderer
            _emitter.enabled = false;
        }

        public override void LaserEnter(LaserEmitter emitter)
        {
            _emitter.enabled = true;
        }

        public override void LaserExit(LaserEmitter emitter)
        {
            _emitter.enabled = false;
        }
    }
}
