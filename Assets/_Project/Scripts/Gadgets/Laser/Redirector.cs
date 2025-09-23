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

        private void OnEnable() { }

        private void OnDisable() { }

        private void EnableEmitter()
        {
            _emitter.enabled = true;
        }

        private void DisableEmitter()
        {
            _emitter.enabled = false;
        }
    }
}
