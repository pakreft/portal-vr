using UnityEngine;

namespace PortalVR.Gadgets
{
    public class HardLightBridge : MonoBehaviour
    {
        public GameObject bridge; // The bridge plane prefab
        public LayerMask stopLayers; // Layers that stop the bridge
        public float bridgeWidth;

        void Update()
        {
            CastBridge();
        }

        private void CastBridge()
        {
            Vector3 outerRightPoint = transform.position + transform.right * (bridgeWidth / 2f);
            Vector3 outerLeftPoint = transform.position - transform.right * (bridgeWidth / 2f);

            float nearestDistance = Mathf.Infinity;
            bool hitSomething = false;

            // Cast from right edge
            if (
                Physics.Raycast(outerRightPoint, transform.forward, out RaycastHit rightHit, Mathf.Infinity, stopLayers)
            )
            {
                nearestDistance = rightHit.distance;
                hitSomething = true;
            }

            // Cast from left edge
            if (Physics.Raycast(outerLeftPoint, transform.forward, out RaycastHit leftHit, Mathf.Infinity, stopLayers))
            {
                if (!hitSomething || leftHit.distance < nearestDistance)
                {
                    nearestDistance = leftHit.distance;
                    hitSomething = true;
                }
            }

            if (hitSomething)
            {
                if (!bridge.activeSelf)
                    bridge.SetActive(true);

                Vector3 scale = bridge.transform.localScale;
                scale.z = nearestDistance;
                bridge.transform.localScale = scale;
            }
            else if (bridge.activeSelf)
            {
                bridge.SetActive(false);
            }

            Debug.DrawRay(outerRightPoint, transform.forward * 50f, Color.red);
            Debug.DrawRay(outerLeftPoint, transform.forward * 50f, Color.blue);
        }
    }
}
