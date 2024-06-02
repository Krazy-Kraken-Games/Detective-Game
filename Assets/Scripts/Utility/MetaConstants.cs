
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Global
{
    public class MetaConstants
    {
        #region Gameplay Feature Constants

        public static string PlayerTag = "Player";

        #region Detective Mode

        public static readonly float TimeToActivate = 3.5f;
        public static readonly float DetectiveModeEndTime = 8f;

        #endregion

        #region Game Player Manager Constants
        public enum GameState
        {
            NORMAL = 0,
            DETECTIVE = 1,
            VIEWER = 2,
            PUZZLE = 3,
            SHOOT = 4
        }

        public enum PlayerInputMode
        {
            PRIMARY = 0, //Refers to Third person input mode

            SECONDARY = 1 // Refers to First person input mode
        }

        public enum GameCameraState
        {
            PRIMARY = 0,
            SECONDARY = 1,
            INVENTORY = 2
        }

        #endregion

        #region Environment Interactable Object Constants

        public static readonly string ObjInterestTag = "ObjectOfInterest";
        public enum InteractableType
        {
            PROCEDURAL = 0, //This denotes that on trigger enter it will perform final ik animations, Stay in third person mode
            PUZZLE = 1,
            PICKUP = 2,  //Same as Procedural, but item will be picked up, while procedural is generic, pick up is specific
            DIALOG = 3 // Denotes player is interacting with an NPC
        }

        #endregion

        #region Puzzle Related Constants

        public enum PuzzleState
        {
            UNSOLVED = 0,
            SOLVED = 1
        }

        public static readonly string PieceTag = "PuzzlePiece";

        #endregion


        #endregion


        #region UI Messages and Constants

        public static readonly string CancelMessage = "Press X to Skip";

        #endregion
    }

    public class GameCameraConstants
    {
        public static readonly float FollowCameraDistance = 3f;
        public static readonly float ShootCameraDistance = 2f;

        public static readonly Vector3 FollowCameraShoulderOffset = new Vector3(3,0,0);
        public static readonly Vector3 ShootCameraShoulderOffset = new Vector3(1.5f, 0.25f, 0);
    }

    public class PostProcessingConstants
    {
        public enum ProcessVolume
        {
            DETECTIVE = 0
        }
    }

    public class GameControlConstants
    {
        public static readonly string Move = "Move";
        public static readonly string Look = "Look";
        public static readonly string Sprint = "Sprint";
        public static readonly string Interact = "Interact";
        public static readonly string Cancel = "Cancel";
        public static readonly string LeftTrigger = "LTriggerOne";
        public static readonly string LeftTrigger2 = "LTriggerTwo";
        public static readonly string RightTrigger = "RTriggerOne";
        public static readonly string RightTrigger2 = "RTriggerTwo";

        public enum ActionKey
        {
            Move,
            Look,
            Sprint,
            Interact,
            Cancel,
            LTriggerOne,
            LTriggerTwo,
            RTriggerOne,
            RTriggerTwo
        }
    }

    public class GamePlayConstants
    {
        public static readonly float ProjectileSpeed = 30f;

        public static readonly string NoShootingAllowedTag = "NoShooting";

        public static readonly string AllowShootingTag = "AllowShooting";
    }

    public class ToasterMessageTemplates
    {
        public static readonly string ShootingLocked = "Shooting has been locked";

        public static readonly string ShootingUnlocked = "Shooting is allowed";
    }

    public class InputDeviceConstants
    {
        public static readonly string XboxControllerLabel = "Xbox Controller";

        public static readonly float GP_RightStickMoveSpeed = 1000f;
    }
}
