using System;
using UnityEngine;

public class QuestOutcome : MonoBehaviour
{
    [SerializeField] private QuestCondition condition;

    [SerializeField] private bool outcome;

    public bool OutCome => outcome;

    public Action<bool> OnOutComeUpdateEvent;

    private void Start()
    {
        if(condition != null)
        {
            condition.OnConditionUpdated += OnConditionUpdatedHandler;
        }
    }

    private void OnDestroy()
    {
        if (condition != null)
        {
            condition.OnConditionUpdated -= OnConditionUpdatedHandler;
        }
    }

    private void OnConditionUpdatedHandler(bool _updatedCondition)
    {
        outcome = _updatedCondition;

        OnOutComeUpdateEvent?.Invoke(outcome);
    }
}
