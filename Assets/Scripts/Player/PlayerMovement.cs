using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Towby
{
    public class PlayerMovement : MonoBehaviour
    {
        
        private PlayerControls _playerControls;
        private InputAction _moveAround;
        private InputAction _lookAround;


        private Rigidbody _thisRb;
        private Vector2 _look;
        private Vector2 _move;
        private Vector3 _direction;
        private Vector3 _angles;
        private float _angleX;
        private Transform _cameraTransform;

        [Header("Settings")]
        [SerializeField] private float _walkSpeed = 10f;
        //[SerializeField] private float _runSpeed = 30f;
        [SerializeField] private float _rotationPower = 1;
        [SerializeField] private Transform _rotationFollow;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _thisRb = GetComponent<Rigidbody>();
            _cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            _moveAround = _playerControls.PlayerMovement.Movement;
            _moveAround.Enable();

            _lookAround = _playerControls.PlayerMovement.Camera;
            _lookAround.Enable();
        }

        private void OnDisable()
        {
            _moveAround.Disable();
            _lookAround.Disable();
        }

        private void Update()
        {
            Move(_move);
            Rotation(_look);
            //if (CameraController.Instance != null)
            //{
            //    CameraController.Instance.FollowTarget(Time.deltaTime);
            //    CameraController.Instance.HandleCameraRotation(Time.deltaTime, _look.x, _look.y);
            //}
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _look = context.ReadValue<Vector2>();
        }

        private void Move(Vector2 dir)
        {
            _direction = new Vector3(dir.x, 0, dir.y);
            _direction = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0) * _direction;

            _thisRb.velocity += _direction * _walkSpeed * Time.deltaTime;
            if (_thisRb.velocity.magnitude > _walkSpeed)
            {
                _thisRb.velocity = Vector3.ClampMagnitude(_thisRb.velocity, _walkSpeed);
            }
        }

        private void Rotation(Vector2 look)
        {
            _rotationFollow.rotation *= Quaternion.AngleAxis(look.x * _rotationPower, Vector3.up);


            _rotationFollow.rotation *= Quaternion.AngleAxis(-look.y * _rotationPower, Vector3.right);

            _angles = _rotationFollow.localEulerAngles;
            _angles.z = 0;

            _angleX = _rotationFollow.localEulerAngles.x;

            if (_angleX > 180 && _angleX < 340)
            {
                _angles.x = 340;
            }
            else if (_angleX < 180 && _angleX > 40)
            {
                _angles.x = 40;
            }

            _rotationFollow.localEulerAngles = _angles;

            //Set player Rotation to look transform
            transform.rotation = Quaternion.Euler(0, _rotationFollow.rotation.eulerAngles.y, 0);

            //Reset otherwise everything goes bonkers
            _rotationFollow.localEulerAngles = new Vector3(_angles.x, 0, 0);
        }
    }
}

