using KrazyKrakenGames;
using KrazyKrakenGames.DetectiveGame.AI;
using KrazyKrakenGames.DetectiveGame.Gameplay;
using KrazyKrakenGames.DetectiveGame.Gameplay.Feature.Shooting;
using KrazyKrakenGames.DetectiveGame.Managers;
using KrazyKrakenGames.DetectiveGame.UI;
using RootMotion.FinalIK;
using StarterAssets;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static KrazyKrakenGames.DetectiveGame.Global.MetaConstants;


[DefaultExecutionOrder(1)]
public class ThirdPersonPlayer : MonoBehaviour
{
    [Header("Game Player Manager reference")]
    [SerializeField] private GamePlayerManager playerManager;
    [SerializeField] private bool isInputAllowed;

    [Space(5)]
    [Header("Object detection system")]
    [Tooltip("Position from where the object detector raycaster will be fired")]
    [SerializeField] private Transform visionPivot;
    [SerializeField] private Camera mainCamera;

    [Space(5)]
    [Header("Nearby interactable object")]
    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private bool nearbyInteractableObj;

    [Space(5)]
    [Header("Shooting System References")]
    [SerializeField] private ShootingSystem shootingSystem;

    [Space(5)]
    [Header("IK References")]
    [SerializeField] private AimIK aimIk;
    [SerializeField] private LookAtIK lookAtIK;


    #region Player Locomotion Variables


    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    #endregion


    #region Cinemachine Camera Variables

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private const float _threshold = 0.01f;

    #endregion


    #region Animation IDs Variables

    // animation IDs
    protected int _animIDSpeed;
    protected int _animIDGrounded;
    protected int _animIDJump;
    protected int _animIDMotionSpeed;
    protected int _animIDFreeFall;
    protected int _animIDirection;

    #endregion


    #region Dynamic Variable References


#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    private Animator _animator;
    private bool _hasAnimator;
    private CharacterController _controller;
    private StarterAssetsInputs _input;
    public StarterAssetsInputs Input => _input;

    private GameObject _mainCamera;


    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    protected float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    private bool shootInputProcessed = false;

    private bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
        }
    }

    #endregion


    #region Detective Mode References

    [Space(5)]
    [Header("Detective Mode Script Reference")]
    [SerializeField] private DetectiveMode detectiveMode;
    [SerializeField] private bool isDetectiveModeActive;
    [SerializeField] private bool isDetectiveModeInProcess;

    #endregion

    #region Events Fired Section
    public Action OnFocusedKeyPressed;
    #endregion


    #region Unity Methods

    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {

        mainCamera = Camera.main;
        playerManager = GamePlayerManager.instance;

        shootingSystem = GetComponent<ShootingSystem>();    

        RegisterEvents();
        
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
        lookAtIK = GetComponent<LookAtIK>();
        lookAtIK.solver.SetIKPositionWeight(0f);

        aimIk = GetComponent<AimIK>();
        aimIk.solver.SetIKPositionWeight(0f);
        
        
        _animator.SetLayerWeight(1, 0f);
        AssignAnimationIDs();
    }

    private void OnDestroy()
    {
       
        UnregisterEvents();
    }


    private void RegisterEvents()
    {
        if (playerManager != null)
        {
            playerManager.OnPlayerInputModeChangedEvent += OnPlayerInputModeChangedEventHandler;

            OnPlayerInputModeChangedEventHandler(playerManager.playerInputMode);
        }

        if(detectiveMode != null)
        {
            detectiveMode.OnDetectiveModeActivated += OnDetectiveModeActivatedHandler;
            detectiveMode.OnDetectiveModeProcessEvent += OnDetectiveModeInProcessHandler;
            detectiveMode.OnDetectiveModeDeactivated += OnDetectiveModeDeactivatedHandler;
        }
    }

    private void UnregisterEvents()
    {
        if (playerManager != null)
        {
            playerManager.OnPlayerInputModeChangedEvent -= OnPlayerInputModeChangedEventHandler;
        }

        if (detectiveMode != null)
        {
            detectiveMode.OnDetectiveModeActivated -= OnDetectiveModeActivatedHandler;
            detectiveMode.OnDetectiveModeDeactivated -= OnDetectiveModeDeactivatedHandler;
            detectiveMode.OnDetectiveModeProcessEvent += OnDetectiveModeInProcessHandler;
        }
    }

    public void Update()
    {
        if (isInputAllowed)
        {
            GroundedCheck();
            EnforceGravity();
            Locomotion();

            if (playerManager.gameState != GameState.SHOOT)
            {
                //Detective or Focused Mode
                FocusMode();

                //Interact with environment
                Interact();
            }
            else
            {
                //Shoot mode is activated, drop raycasts here
                shootingSystem.PreShootRaycasting();
            }

            HandleShootButtonHit();

            HandleInventoryButton();

            //TODO: Decide if we want to make game not switch to shooting mode if in detective mode
            ShootInput();

            CancelInputHandling();
        }
    }

    private void LateUpdate()
    {
        if (isInputAllowed)
        {
            CameraRotation();
        }
    }

    #endregion


    #region Events and Variable Registrations

    private void OnPlayerInputModeChangedEventHandler(PlayerInputMode playerInputMode)
    {
        if (playerInputMode == PlayerInputMode.PRIMARY)
        {
            isInputAllowed = true;
        }
        else
        {
            //This will stop the 3rd person player motion script from taking input
            isInputAllowed = false;

            if (_animator != null)
            {
                _animator.SetFloat(_animIDSpeed, 0.0f);
            }
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDirection = Animator.StringToHash("Direction");
    }

    #endregion


    #region Camera Handling Section

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    #endregion


    #region Movement Update Code

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }


    private void EnforceGravity()
    {
        if (Grounded)
        {
            return;
        }
        else
        {
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    }

    private void Locomotion()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed;

        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        ///Rotate player to face the camera always
        transform.rotation = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f);

        //Camera and input related movement
        Vector3 forwardRelativeVerticalMovement = _input.move.y * forward;
        Vector3 rightRelativeHorizontalMovement = _input.move.x * right;

        Vector3 cameraRelativeMovement = 
            forwardRelativeVerticalMovement + rightRelativeHorizontalMovement;

        if(_input.move.y > 0.0f && (Mathf.Abs(_input.move.y) > Mathf.Abs(_input.move.x)))
        {
            //The player is running in a forward direction! Then only sprint!

            targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
        }
        else
        {
            targetSpeed = MoveSpeed;
        }
        
        _controller.Move(cameraRelativeMovement.normalized * (targetSpeed * Time.deltaTime) +
                              new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);


        if(_input.move.y > 0.0f)
        {
            //We run only for sprint
            if (_input.sprint)
            {
                _animator.SetFloat(_animIDSpeed, _input.move.y);
            }
            else
            {
                _animator.SetFloat(_animIDSpeed, 0.5f);
            }
        }
        else
        {
            _animator.SetFloat(_animIDSpeed, _input.move.y);
        }
        _animator.SetFloat(_animIDirection, _input.move.x);
        
    }

    #endregion


    #region Input Handling
    private void FocusMode()
    {
        if (_input.leftShoulder)
        {
            _input.leftShoulder = false;

            //Logic to handle starting the detective mode
            detectiveMode.StartDetectiveMode();
        }
    }

    private void ShootInput()
    {
        if (_input.leftTrigger && shootingSystem.Allowed)
        {
            if (!shootInputProcessed)
            {
                playerManager.UpdateMode(GameState.SHOOT);
                shootInputProcessed = true;

                aimIk.solver.SetIKPositionWeight(1f);
                lookAtIK.solver.SetIKPositionWeight(1f);

                _animator.SetLayerWeight(1, 1f);

                shootingSystem.ActivateShootMode();
            }
        }
        else
        {
            if (shootInputProcessed)
            {
                //Shoot input was detected some time
                playerManager.UpdateMode(GameState.NORMAL);
                _input.rightTrigger = false;

                aimIk.solver.SetIKPositionWeight(0f);
                lookAtIK.solver.SetIKPositionWeight(0f);

                _animator.SetLayerWeight(1, 0f);

                shootingSystem.DeactivateShootMode();
            }

            shootInputProcessed = false;
        }
    }

    private void HandleShootButtonHit()
    {
        if (_input.rightTrigger)
        {
            if(playerManager.gameState == GameState.SHOOT)
            {
                shootingSystem.Shoot();
            }
            _input.rightTrigger = false;
        }
    }

    private void HandleInventoryButton()
    {
        if (_input.inventory)
        {
            //Inventory button is pressed, toggle mode and screen

            if (isDetectiveModeInProcess || isDetectiveModeActive)
            {
                detectiveMode.EndDetectiveMode();
            }

            UIManager.instance.ToggleInventory();

            _input.inventory = false;
        }
    }

    private void Interact()
    {
        if (_input.interact)
        {
            _input.interact = false;

            if(nearbyInteractableObj && interactableObject != null)
            {
                //TODO: Move all this into a function later
                _animator.SetFloat(_animIDSpeed, 0.0f);
                isInputAllowed = false;

                var triggerBox = interactableObject.triggerBox;

                // Check if position override for player exists, perform update on position if yes
                if (triggerBox.overridePlayerPosition && triggerBox.PlayerPosition != null)
                {
                    transform.position = triggerBox.PlayerPosition.position;
                }

                //Check if detective mode is active, if true then force override to end it

                if (isDetectiveModeInProcess || isDetectiveModeActive)
                {
                    detectiveMode.EndDetectiveMode();
                }

                #region TO BE DELETED SECTION
                //if (interactableObject.type == InteractableType.PUZZLE)
                //{
                //    Debug.Log("Go into secondary");
                //    CameraManager.instance.SetState(GameCameraState.SECONDARY, lookAt);
                //    playerManager.UpdateInputMode(PlayerInputMode.SECONDARY);
                //}

                //else if (interactableObject.type == InteractableType.PROCEDURAL || interactableObject.type == InteractableType.PICKUP)
                //{
                //    if (interactableObject != null)
                //    {
                //        interactableObject.Interact();
                //    }
                //}

                //else if(interactableObject.type == InteractableType.DIALOG)
                //{
                //    Debug.Log("Dialog interaction started with an NPC");

                //    if (interactableObject != null)
                //    {
                //        interactableObject.Interact();
                //    }

                //    GamePlayerManager.instance.UpdateInputMode(MetaConstants.PlayerInputMode.PRIMARY);
                //}

                #endregion

                if (interactableObject != null)
                {
                    interactableObject.Interact();
                }
            }
        }
    }
    #endregion

    
    #region Cancel Input Handling

    private void CancelInputHandling()
    {
        if (_input.cancel)
        {
            _input.cancel = false;
            //Check if instruction box is currently open
            if (UIManager.instance.InstructionActive())
            {
                UIManager.instance.HideInstructionBox();
            }
        }
    }

    #endregion


    #region Object Detection System Section
    private void ObjectDetectionSystem()
    {
        if(mainCamera != null)
        {
            Vector3 combinedDirection = visionPivot.forward + mainCamera.transform.forward;

            Debug.DrawRay(visionPivot.position, combinedDirection);
        }
    }
    #endregion


    #region Detective Mode Handling Section

    private void OnDetectiveModeActivatedHandler()
    {
        isDetectiveModeActive = true;
        isDetectiveModeInProcess = false;
    }

    private void OnDetectiveModeDeactivatedHandler()
    {
        isDetectiveModeActive = false;
        isDetectiveModeInProcess = false;
    }

    private void OnDetectiveModeInProcessHandler()
    {
        isDetectiveModeInProcess = true;
    }

    #endregion


    #region Collision and Triggers Detection Section

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var collidedWith = hit.gameObject;
        if (collidedWith.tag == "Enemy")
        {
            if (collidedWith.TryGetComponent<Crawler>(out var crawler))
            {
                crawler.OnCollisionWithPlayer();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "TriggerBox")
        {
            TriggerBox triggerBox = other.gameObject.GetComponent<TriggerBox>();

            if(triggerBox != null)
            {
                interactableObject = triggerBox.interactionObject;
                nearbyInteractableObj = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TriggerBox")
        {
            Debug.Log("Player left trigger box");
            interactableObject = null;
            nearbyInteractableObj = false;

            PlayerInteractionSystem.instance.interactableObject = null;
        }
    }

    #endregion


    #region Utility Section

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    #endregion

    #region Debugging Section
    private void OnDrawGizmos()
    {
        if (KrakenDebugger.Instance != null)
        {
           
        }
    }

    #endregion

}
