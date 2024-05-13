using System;
using UnityEngine;

public class QuestCondition : MonoBehaviour
{
    [SerializeField] private bool condition;

    public Action<bool> OnConditionUpdated;

    public void UpdateCondition(bool _condition)
    {
        condition = _condition;

        OnConditionUpdated?.Invoke(condition);
    }
}
