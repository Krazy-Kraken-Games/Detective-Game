using Benchmarking;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public Vector2 navigate;

		public Vector2 consoleNavigate;
		public bool isConsoleNavigateActive;

		public bool jump;
		public bool sprint;
		public bool interact;
		public bool cancel;
		public bool leftShoulder;
		public bool leftTrigger;
		public bool rightTrigger;
		public bool inventory;


		public bool kraken; //Only for debugger mode

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public PlayerManager CameraManager;

		private bool m_IgnoreInput;


		private static bool m_FocusActionsSetUp;

		[Header("Active Input Device")]
		[SerializeField] public InputDevice currentDevice;

		private void Start()
		{
			if (!m_FocusActionsSetUp)
			{
#if UNITY_EDITOR
				var ignoreInput = new InputAction(binding: "/Keyboard/escape");
				ignoreInput.performed += context => m_IgnoreInput = true;
				ignoreInput.Enable();

				var enableInput = new InputAction(binding: "/Mouse/leftButton");
				enableInput.performed += context => m_IgnoreInput = false;
				enableInput.Enable();
#endif
				
				//var touchFocus = new InputAction(binding: "<pointer>/press");
				//touchFocus.performed += context => CameraManager.NotifyPlayerMoved();
				//touchFocus.Enable();
				
				m_FocusActionsSetUp = true;
			}
		}

        private void Update()
        {
            foreach (var device in InputSystem.devices)
            {
                if (device.wasUpdatedThisFrame)
                {
					currentDevice = device;
                }
            }

            if (consoleNavigate != Vector2.zero)
            {
                isConsoleNavigateActive = true;

            }
            else
            {
                isConsoleNavigateActive = false;
            }

        }

        private void OnDestroy()
		{
			m_FocusActionsSetUp = false;
		}


#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			if (m_IgnoreInput)
			{
				MoveInput(Vector2.zero);
				return;
			}
			
			if (CameraManager != null)
			{
				CameraManager.NotifyPlayerMoved();
			}
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (m_IgnoreInput)
			{
				LookInput(Vector2.zero);
				return;
			}
			
			if (CameraManager != null)
			{
				CameraManager.NotifyPlayerMoved();
			}
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (CameraManager != null)
			{
				CameraManager.NotifyPlayerMoved();
			}
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (CameraManager != null)
			{
				CameraManager.NotifyPlayerMoved();
			}
			SprintInput(value.isPressed);
		}

		

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
        }

		public void OnInventory(InputValue value)
		{
			InventoryButtonInput(value.isPressed);
		}

		public void OnCancel(InputValue value)
		{
			CancelInput(value.isPressed);
		}

		public void OnKraken(InputValue value)
		{
			KrakenInput(value.isPressed);
		}

		public void OnLTriggerOne(InputValue value)
		{
			LeftShoulderInput(value.isPressed);
		}

		public void OnLTriggerTwo(InputValue value)
		{
			LeftTriggerInput(value.isPressed);

        }

		public void OnRTriggerTwo(InputValue value)
		{
			RightTriggerInput(value.isPressed);
		}

		public void OnPuzzleNavigate(InputValue value)
		{
			NavigateInput(value.Get<Vector2>());

        }

        public void OnPuzzleConsoleNavigate(InputValue value)
        {
			ConsoleNavigateInput(value.Get<Vector2>());
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{ 
			look = newLookDirection;
		}

		public void NavigateInput(Vector2 newNavigationVector)
		{
			navigate = newNavigationVector;
		}

		public void ConsoleNavigateInput(Vector2 newNavigationVector)
		{
			consoleNavigate = newNavigationVector;

			
			
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}

		public void InventoryButtonInput(bool newInventoryButtonState)
		{
			inventory = newInventoryButtonState;
		}


        public void CancelInput(bool newCancelState)
		{
            cancel = newCancelState;

        }

		public void KrakenInput(bool newKrakenState)
		{
			kraken = newKrakenState;
		}

		public void LeftShoulderInput(bool newLeftTriggerState)
		{
			leftShoulder = newLeftTriggerState;
		}

		public void LeftTriggerInput(bool newLeftTriggerState)
		{
			leftTrigger = newLeftTriggerState;
		}

		public void RightTriggerInput(bool newRightTriggerState)
		{
			rightTrigger = newRightTriggerState;
		}


        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
			m_IgnoreInput = !hasFocus;
		}

		public void SetCursorInGame(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
			Cursor.visible = false;
		}

		private void SetCursorState(bool newState)
		{
			if (PerformanceTest.RunningBenchmark)
				return;

			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}