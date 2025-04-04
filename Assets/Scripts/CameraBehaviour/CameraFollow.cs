using UnityEngine;

namespace CameraBehaviour
{
    public class CameraFollow : MonoBehaviour
    {
        private CameraFollowProperties _properties;
        private Transform _target;
        
        public void SetProperties(CameraFollowProperties properties, Transform target, Transform initialPosition)
        {
            _properties = properties;
            _target = target;

            Transform targetTransform = !_properties.FollowOnY && !_properties.FollowOnX ? initialPosition : target;
            transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
        }

        private void LateUpdate()
        {
            if (_properties != null && _target != null)
            {
                float xPos = _properties.FollowOnX ? Mathf.Clamp(_target.position.x, _properties.ClampX.x, _properties.ClampX.y) : transform.position.x;
                float yPos = _properties.FollowOnY ? Mathf.Clamp(_target.position.y, _properties.ClampY.y, _properties.ClampY.y) : transform.position.y;
                transform.position = new Vector3(xPos, yPos, transform.position.z);
            }
        }
    }
}