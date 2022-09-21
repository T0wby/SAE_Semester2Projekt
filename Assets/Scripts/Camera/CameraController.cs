using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player_Towby
{
    public class CameraController : Singleton<CameraController>
    {
        [Header("Positions")]
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _cameraPivotTransform;
        [SerializeField] private LayerMask _ignoreLayers;
        private Transform _thisTransform;
        private Vector3 _cameraPosition;
        private Vector3 _targetPosition;
        private Vector3 _rotation;
        private Vector3 _cameraFollowVelocity = Vector3.zero;
        private Quaternion _targetRotation;

        [Header("Settings")]
        [SerializeField] private float _lookSpeed;
        [SerializeField] private float _followSpeed;
        [SerializeField] private float _pivotSpeed;
        [SerializeField] private float _minimumPivot = -35f;
        [SerializeField] private float _maximumPivot = 35f;
        [SerializeField] private float _cameraSphereRadius = 0.2f;
        [SerializeField] private float _cameraCollisionOffset = 0.2f;
        [SerializeField] private float _minimumCollisionOffset = 0.2f;
        private float _cameraTargetPosition;
        private float _defaultPosition;
        private float _lookAngle;
        private float _pivotAngle;


        private void Start()
        {
            _thisTransform = transform;
            _defaultPosition = _cameraTransform.localPosition.z;
        }

        public void FollowTarget(float delta)
        {
            //_targetPosition = Vector3.Lerp(_thisTransform.position, _targetTransform.position, delta/_followSpeed);
            _targetPosition = Vector3.SmoothDamp(_thisTransform.position, _targetTransform.position, ref _cameraFollowVelocity, delta / _followSpeed);
            _thisTransform.position = _targetPosition;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseX, float mouseY)
        {
            _lookAngle += (mouseX * _lookSpeed) / delta;
            _pivotAngle -= (mouseY * _pivotSpeed) / delta;
            _pivotAngle = Mathf.Clamp(_pivotAngle, _minimumPivot, _maximumPivot);

            _rotation = Vector3.zero;
            _rotation.y = _lookAngle;
            _targetRotation = Quaternion.Euler(_rotation);
            _thisTransform.rotation = _targetRotation;

            _rotation = Vector3.zero;
            _rotation.x = _pivotAngle;
            _targetRotation = Quaternion.Euler(_rotation);
            _cameraPivotTransform.localRotation = _targetRotation;
        }

        private void HandleCameraCollision(float delta)
        {
            _cameraTargetPosition = _defaultPosition;
            RaycastHit hit;
            Vector3 camPivDirection = _cameraTransform.position - _cameraPivotTransform.position;
            camPivDirection.Normalize();

            if (Physics.SphereCast(_cameraPivotTransform.position, _cameraSphereRadius, camPivDirection, out hit, Mathf.Abs(_cameraTargetPosition), _ignoreLayers))
            {
                float dis = Vector3.Distance(_cameraPivotTransform.position, hit.point);
                _cameraTargetPosition = -(dis - _cameraCollisionOffset);
            }

            if (Mathf.Abs(_cameraTargetPosition) < _minimumCollisionOffset)
            {
                _cameraTargetPosition = -_cameraCollisionOffset;
            }

            _cameraPosition.z = Mathf.Lerp(_cameraTransform.localPosition.z, _cameraTargetPosition, delta / 0.2f);
            _cameraTransform.localPosition = _cameraPosition;
        }
    }
}

