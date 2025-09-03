using UnityEngine;

namespace PortalVR.Portals
{
    public class PortalTransporter : MonoBehaviour
    {
        private Portal _portal;
        private Portal _otherPortal;
        private Collider _wallCollider;

        private void Awake()
        {
            _portal = GetComponentInParent<Portal>();

            if (_portal == null)
            {
                Debug.LogError("Portal-Component not found in Parent.");
                return;
            }

            _otherPortal = _portal.OtherPortal;
            _wallCollider = _portal.WallCollider;
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log(other.name);
            var portalable = other.GetComponent<Portalable>();
            if (portalable != null)
            {
                var localPos = transform.InverseTransformPoint(portalable.transform.position);
                var isBehindTrigger = localPos.z > 0;

                if (isBehindTrigger)
                {
                    portalable.Teleport(_portal, _otherPortal);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var portalable = other.GetComponent<Portalable>();
            if (portalable != null)
            {
                portalable.EnterPortalTrigger(transform, _otherPortal.transform, _wallCollider);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var portalable = other.GetComponent<Portalable>();
            if (portalable != null)
            {
                portalable.ExitPortalTrigger(_wallCollider);
            }
        }
    }
}
