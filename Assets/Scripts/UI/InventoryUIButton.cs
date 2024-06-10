using KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles;
using KrazyKrakenGames.DetectiveGame.UI;
using UnityEngine;

public class InventoryUIButton : MonoBehaviour
{
    //Would be turned into a scriptable object soon
    [SerializeField] private InvestigationObject investigationObjectPrefab;


    public void OnUIButtonClicked()
    {
        UIManager.instance.SetInvestigationModel(investigationObjectPrefab);
    }
}
