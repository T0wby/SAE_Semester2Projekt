using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_Towby
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields
        private PlayerControls _playerControls;
        private InputAction _moveAround;
        private InputAction _lookAround;
        private InputAction _menuAction;
        private InputAction _generatePlanets;
        private InputAction _toggleSandstorm;


        private Rigidbody _thisRb;
        private Vector2 _look;
        private Vector2 _move;
        private Vector3 _direction;
        private Vector3 _angles;
        private float _angleX;
        private Transform _cameraTransform;

        [Header("Settings")]
        [SerializeField] private float _walkSpeed = 10f;
        [SerializeField] private float _runSpeed = 30f;
        [SerializeField] private float _rotationPower = 1;
        [SerializeField] private Transform _rotationFollow;
        #endregion

        #region Unity
        private void Awake()
        {
            _playerControls = new PlayerControls();
            _thisRb = GetComponent<Rigidbody>();
            _cameraTransform = Camera.main.transform;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnEnable()
        {
            _moveAround = _playerControls.PlayerMovement.Movement;
            _lookAround = _playerControls.PlayerMovement.Camera;
            EnableMovement();

            _menuAction = _playerControls.Menu.OpenMenu;
            _menuAction.Enable();

            _generatePlanets = _playerControls.Menu.GeneratePlanet;
            _generatePlanets.Enable();

            _toggleSandstorm = _playerControls.Menu.ToggleSandstorm;
            _toggleSandstorm.Enable();
        }

        private void OnDisable()
        {
            DisableMovement();
            _menuAction.Disable();
            _generatePlanets.Disable();
            _toggleSandstorm.Disable();
        }

        private void Update()
        {
            Move(_move);
            Rotation(_look);
        }
        #endregion

        #region Callbacks
        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (UIManager.Instance.IsInMenu)
                return;

            _look = context.ReadValue<Vector2>();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Calculates direction and velocity of the player
        /// </summary>
        /// <param name="dir">Direction to move in</param>
        private void Move(Vector2 dir)
        {
            _direction = new Vector3(dir.x, 0, dir.y);
            _direction = Quaternion.Euler(0, _cameraTransform.eulerAngles.y, 0) * _direction;

            _thisRb.velocity += _direction * _walkSpeed * Time.deltaTime;
            if (_thisRb.velocity.magnitude > _runSpeed)
            {
                _thisRb.velocity = Vector3.ClampMagnitude(_thisRb.velocity, _runSpeed);
            }
        }

        /// <summary>
        /// Calculates the rotation of the player
        /// </summary>
        /// <param name="look">Rotation vector from the mouse input</param>
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

        /// <summary>
        /// Used for the slider in the UI
        /// </summary>
        /// <param name="value">Slider value</param>
        public void RotationPowerChange(float value)
        {
            _rotationPower = value;
        } 

        public void EnableMovement()
        {
            _moveAround.Enable();
            _lookAround.Enable();
        }

        public void DisableMovement()
        {
            _moveAround.Disable();
            _lookAround.Disable();
        }
        #endregion
    }
}

