
namespace KrazyKrakenGames.DetectiveGame.Global
{
    public class MetaConstants
    {
        #region Gameplay Feature Constants

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
            PUZZLE = 3
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

        public enum ActionKey
        {
            Move,
            Look,
            Sprint,
            Interact,
            Cancel,
            LTriggerOne,
        }
    }
}
