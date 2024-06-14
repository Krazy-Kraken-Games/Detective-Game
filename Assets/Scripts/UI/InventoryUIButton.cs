using KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles;
using KrazyKrakenGames.DetectiveGame.UI;
using TMPro;
using UnityEngine;

public class InventoryUIButton : MonoBehaviour
{
    [SerializeField] private InvestigationModelSO modelData;

    [SerializeField] private InvestigationObject investigationObjectPrefab;

    [SerializeField] private TextMeshProUGUI labelNameText;

    private void Start()
    {
       if(modelData != null)
        {
            SetData(modelData);
        }
    }

    public void SetData(InvestigationModelSO _data)
    {
        modelData = _data;
        investigationObjectPrefab = modelData.investigationObjectPrefab;
        labelNameText.text = modelData.objectName;
    }

    public void OnUIButtonClicked()
    {
        UIManager.instance.SetInvestigationModel(investigationObjectPrefab);
    }
}
