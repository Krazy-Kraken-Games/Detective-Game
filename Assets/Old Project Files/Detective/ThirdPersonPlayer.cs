using KrazyKrakenGames.Managers;
using StarterAssets;
using System;
using UnityEngine;

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
    //[SerializeField] private InteractableObject interactableObject;
    [SerializeField] private bool nearbyInteractableObj;


    public Action OnFocusedKeyPressed;

    public override void Start()
    {
        base.Start();

        mainCamera = Camera.main;
        playerManager = GamePlayerManager.instance;

        if(playerManager != null )
        {
            //playerManager.OnPlayerInputModeChangedEvent += OnPlayerInputModeChangedEventHandler;

            //OnPlayerInputModeChangedEventHandler(playerManager.playerInputMode);
        }
    }

    private void OnDestroy()
    {
        //if (playerManager != null)
        //{
        //    playerManager.OnPlayerInputModeChangedEvent -= OnPlayerInputModeChangedEventHandler;
        //}
    }

    //public override void Update()
    //{
    //   base.Update();

    //    if (isInputAllowed)
    //    {
    //        //JumpAndGravity();
    //        //GroundedCheck();
    //        Locomotion();

    //        //Detective or Focused Mode
    //        FocusMode();

    //        //Interact with environment
    //        Interact();
    //    }
    //}

    //public override void LateUpdate()
    //{
    //    if (isInputAllowed)
    //    {
    //        CameraRotation();
    //    }
    //}

    //private void OnPlayerInputModeChangedEventHandler(PlayerInputMode playerInputMode)
    //{
    //    if(playerInputMode == PlayerInputMode.LOCKED)
    //    {
    //        isInputAllowed = false;
    //    }
    //    else if(playerInputMode == PlayerInputMode.UNLOCKED)
    //    {
    //        isInputAllowed = true;
    //    }
    //    else if(playerInputMode == PlayerInputMode.OBJECTVIEWER)
    //    {
    //        //This will stop the 3rd person player motion script from taking input
    //        isInputAllowed = false;
    //    }
    //}


    #region Movement Update Code

    private void Locomotion()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

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
        //if (_input.focusMode)
        //{
        //    OnFocusedKeyPressed?.Invoke();

        //    _input.focusMode = false;
        //}
    }

    private void Interact()
    {
        //if (nearbyInteractableObj)
        //{
        //    if (_input.interact)
        //    {
        //        if (interactableObject != null)
        //        {
        //            interactableObject.Interact(this);
        //        }
        //        _input.interact = false;
        //    }
        //}
        //else
        //{
        //    if (_input.interact)
        //    {
        //        _input.interact = false;
        //    }
        //}
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


    #region Collision and Triggers Detection Section

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == MetaConstants.ObjInterestTag)
        //{
        //    nearbyInteractableObj = true;
        //    interactableObject = other.GetComponent<InteractableObject>();  
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == MetaConstants.ObjInterestTag)
        //{
        //    nearbyInteractableObj = false;
        //    interactableObject = null;
        //}
    }

    #endregion

}
