using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Utility {
    public class KrakenUtility
    {
        public Vector3 GetScreenPositionFromRightStick(Vector2 rightStickValue)
        {
            // Get the screen dimensions
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // Normalize the right stick value to range from 0 to 1
            Vector2 normalizedRightStickValue = (rightStickValue + Vector2.one) / 2;

            // Convert the normalized value to screen position
            Vector3 screenPosition = new Vector3(
                normalizedRightStickValue.x * screenWidth,
                normalizedRightStickValue.y * screenHeight,
                2f
            );

            return screenPosition;
        }
    }
}
