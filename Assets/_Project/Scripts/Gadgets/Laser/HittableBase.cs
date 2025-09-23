using UnityEngine;
using UnityEngine.Events;

namespace PortalVR.Gadgets.Laser
{
    [RequireComponent(typeof(Collider))]
    public abstract class HittableBase : MonoBehaviour, IHittable
    {
        public UnityEvent OnLaserEnter;
        public UnityEvent OnLaserExit;

        private Renderer _renderer;

        protected virtual void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.color = Color.red;
        }

        public virtual void LaserEnter(LaserEmitter emitter)
        {
            _renderer.material.color = Color.green;
            OnLaserEnter?.Invoke();
        }

        public virtual void LaserExit(LaserEmitter emitter)
        {
            _renderer.material.color = Color.red;
            OnLaserExit?.Invoke();
        }
    }
}
