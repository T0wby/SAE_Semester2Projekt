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

        [Header("Settings")]
        [SerializeField] private float _walkSpeed = 10f;
        [SerializeField] private float _runSpeed = 30f;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _thisRb = GetComponent<Rigidbody>();
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
            if (CameraController.Instance != null)
            {
                CameraController.Instance.FollowTarget(Time.deltaTime);
                CameraController.Instance.HandleCameraRotation(Time.deltaTime, _look.x, _look.y);
            }
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

            _thisRb.velocity += _direction * _walkSpeed * Time.deltaTime;
        }
    }
}

