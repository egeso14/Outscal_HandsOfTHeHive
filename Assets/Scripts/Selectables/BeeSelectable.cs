using UnityEngine;

public class BeeSelectable : Selectable
{
    [SerializeField] private GameObject selectedHighlightEffectPrefab;
    protected override void RespondToSelection()
    {

    }

    protected override void RespondToDeselection()
    {

    }

    private void TriggerSelectedHighlightEffect()
    {

    }
}
