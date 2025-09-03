using Fragilem17.MirrorsAndPortals;
using UnityEngine;

namespace PortalVR.Portals
{
    public class Portal : MonoBehaviour
    {
        [field: SerializeField]
        public Portal OtherPortal { get; private set; }

        [field: SerializeField]
        public PortalSurface PortalSurface { get; private set; }

        [field: SerializeField]
        public PortalTransporter PortalTransporter { get; private set; }

        [field: SerializeField]
        public Collider WallCollider { get; private set; }

        public void Place(Vector3 pos, Collider wallCollider, Vector3 surfaceNormal, Vector3 orientationVector)
        {
            WallCollider = wallCollider;

            transform.rotation = CalculateRotation(surfaceNormal, orientationVector);
            transform.position = pos + transform.forward * 0.01f;
        }

        private Quaternion CalculateRotation(Vector3 surfaceNormal, Vector3 orientationVector)
        {
            Quaternion finalRot;
            var portalForward = surfaceNormal;

            var dot = Vector3.Dot(Vector3.up, portalForward);
            var isFloorOrCeiling = Mathf.Approximately(dot, 1f) || Mathf.Approximately(dot, -1f);
            var isVerticalWall = Mathf.Approximately(dot, 0f);

            if (isFloorOrCeiling)
            {
                var portalRight = Vector3.Cross(orientationVector, portalForward).normalized;

                //  Edge case: if gun's up is parallel to portal forward, use a fallback right vector
                if (portalRight.sqrMagnitude < 0.001f)
                {
                    portalRight = Vector3.Cross(Vector3.right, portalForward).normalized;
                }

                var portalUp = Vector3.Cross(portalForward, portalRight);

                finalRot = Quaternion.LookRotation(portalForward, portalUp);
            }
            else if (isVerticalWall)
            {
                finalRot = Quaternion.LookRotation(portalForward, Vector3.up);
            }
            else
            {
                var portalRight = Vector3.Cross(Vector3.up, portalForward).normalized;
                var portalUp = Vector3.Cross(portalForward, portalRight).normalized;

                finalRot = Quaternion.LookRotation(portalForward, portalUp);
            }

            return finalRot;
        }
    }
}
