using KrazyKrakenGames.DetectiveGame.Gameplay.Puzzles;
using UnityEngine;

[CreateAssetMenu(fileName = "Investigation Model",menuName = "Krazy Kraken Games/Inventory/Investigation Model SO")]
public class InvestigationModelSO : ScriptableObject
{
    public string objectName;
    public InvestigationObject investigationObjectPrefab;
}
