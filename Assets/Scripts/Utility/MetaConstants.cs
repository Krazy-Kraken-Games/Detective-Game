
namespace KrazyKrakenGames.Global
{
    public class MetaConstants
    {
        #region Gameplay Feature Constants

        #region Detective Mode

        public static readonly float DetectiveModeSaturationValue = -100;
        public static readonly float GameModeSaturationValue = 20;

        public static readonly float TimeToActivate = 1f;
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
            UNLOCKED = 0,
            LOCKED = 1,
            OBJECTVIEWER = 2,
            PUZZLE = 3
        }
        #endregion

        #region Environment Interactable Object Constants

        public static readonly string ObjInterestTag = "ObjectOfInterest";
        public enum InteractableType
        {
            INFO = 0,   //A simple text box would be shown for the default object
            VIEWER = 1, //The model viewer system will show the model in detail
            EVIDENCE = 2,  //Building up on type 1, this type would also be able to save in inventory
            PUZZLE = 3  // This will fire up a puzzle, currently we only have reconstruction puzzle elements
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
}
