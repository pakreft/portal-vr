using UnityEngine;

namespace PortalVR.Portals
{
    public class Portalable : MonoBehaviour
    {
     [SerializeField]
        private Rigidbody _rigidbody;
        private Transform _inPortalTransform;
        private Transform _outPortalTransform;
        private GameObject _clone;
        private Collider _collider;

        private static GameObject _cloneContainer;

        private void Awake()
        {
            _cloneContainer ??= new GameObject("PortalableCloneContainer");
            _collider = GetComponent<Collider>();
            enabled = false;
        }

        private void LateUpdate()
        {
            SetClonePositionAndRotation();
        }

        public void Teleport(Portal inPortal, Portal outPortal)
        {
            var inTransform = inPortal.transform;
            var outTransform = outPortal.transform;
            var halfTurn = Quaternion.Euler(0, 180, 0);

            // Update position of object
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = halfTurn * relativePos;
            transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of object
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * outTransform.rotation;
            relativeRot = halfTurn * relativeRot;
            transform.rotation = relativeRot;

            // Update velocity of rigidbody
            Vector3 relativeVel = inTransform.InverseTransformDirection(_rigidbody.linearVelocity);
            relativeVel = halfTurn * relativeVel;
            _rigidbody.linearVelocity = outTransform.TransformDirection(relativeVel);
        }

        private void SetClonePositionAndRotation()
        {
            var inTransform = _inPortalTransform;
            var outTransform = _outPortalTransform;
            var halfTurn = Quaternion.Euler(0, 180, 0);

            // Position
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = halfTurn * relativePos;
            _clone.transform.position = outTransform.TransformPoint(relativePos);

            // Rotation
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = halfTurn * relativeRot;
            _clone.transform.rotation = outTransform.rotation * relativeRot;
        }

        private void CreateClone()
        {
            _clone = new GameObject(gameObject.name + " (Clone)");
            _clone.transform.SetParent(_cloneContainer.transform);

            var meshFilter = _clone.AddComponent<MeshFilter>();
            var meshRenderer = _clone.AddComponent<MeshRenderer>();

            meshFilter.mesh = GetComponent<MeshFilter>().mesh;
            meshRenderer.materials = GetComponent<MeshRenderer>().materials;

            _clone.transform.localScale = transform.localScale;
        }

        public void EnterPortalTrigger(Transform inPortalTransform, Transform outPortalTransform, Collider wallCollider)                   
        { 
            _inPortalTransform = inPortalTransform;
            _outPortalTransform = outPortalTransform;

            Physics.IgnoreCollision(_collider, wallCollider);

            if (_clone == null)
                CreateClone();
            else
            {
                _clone.SetActive(true);
            }

            enabled = true;
        }

        public void ExitPortalTrigger(Collider wallCollider)
        {
            Physics.IgnoreCollision(_collider, wallCollider, false);
            _clone.SetActive(false);
            enabled = false;
        }
    }
}
