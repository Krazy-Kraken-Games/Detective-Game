
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
            PUZZLE = 1
        }

        #endregion

        #region Puzzle Related Constants

        public enum PuzzleState
        {
            UNSOLVED = 0,
            SOLVED = 1
        }

        #endregion


        #endregion
    }


    public class PostProcessingConstants
    {
        public enum ProcessVolume
        {
            DETECTIVE = 0
        }
    }
}
