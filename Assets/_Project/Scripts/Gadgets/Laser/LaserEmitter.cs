using System;
using System.Collections.Generic;
using UnityEngine;

namespace PortalVR.Gadgets.Laser
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserEmitter : MonoBehaviour
    {
        [SerializeField]
        private float _maxDistance = 10f;

        [SerializeField]
        private LayerMask _layerMask;

        private LaserManager _laserManager;
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _laserManager = GetComponentInParent<LaserManager>();
            if (_laserManager == null)
            {
                Debug.LogError("LaserManager not found in parents", this);
                return;
            }

            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = 2;
        }

        private void OnEnable() => _lineRenderer.enabled = true;

        private void OnDisable() => _lineRenderer.enabled = false;

        public List<IHittable> FireLaser()
        {
            var startPos = transform.position;
            var direction = transform.forward;
            var endPos = startPos + direction * _maxDistance;

            var hits = Physics.RaycastAll(startPos, direction, _maxDistance, _layerMask);
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            List<IHittable> hittables = new();

            foreach (var hit in hits)
            {
                var hittable = hit.collider.GetComponent<IHittable>();
                if (hittable != null)
                {
                    hittables.Add(hittable);
                }

                if (hittable is Trigger) { }
                else
                {
                    endPos = hit.point;
                    break;
                }
            }

            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);

            return hittables;
        }
    }
}
