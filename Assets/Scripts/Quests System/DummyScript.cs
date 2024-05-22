using KrazyKrakenGames.DetectiveGame.UI;
using UnityEngine;
using UnityEngine.Events;


public class DummyScript : MonoBehaviour
{
    public void TestingDum()
    {
        Debug.Log("Testing part 2 also fired");
    }

    public void PuzzleTest()
    {
        UIManager.instance.AddToasterMessage("Puzzle Solved!");
        Debug.Log("Puzzle has been solved! Wohoo!");
    }
}
