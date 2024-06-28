using System.Collections.Generic;
using UnityEngine;

namespace KrazyKrakenGames.DetectiveGame.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance = null;

        public List<InvestigationModelSO> investigationModelsSO = new List<InvestigationModelSO>();

        [SerializeField] private InventoryUIButton inventoryButtonPrefab;
        [SerializeField] private Transform contentParent;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddModelData(InvestigationModelSO _data)
        {
            if (!investigationModelsSO.Contains(_data))
            {
                investigationModelsSO.Add(_data);

                CreateUIButton(_data);
                Debug.Log($"Added new UI invest: {_data.objectName}");
            }
        }

        private void CreateUIButton(InvestigationModelSO _data)
        {
            var inventUI = Instantiate(inventoryButtonPrefab, Vector3.zero, Quaternion.identity, contentParent);
            inventUI.SetData(_data);
        }
    }
}
