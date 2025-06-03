using UnityEngine;

public class SelectSingle: Selection
{

    public LayerMask selectableLayers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ClickRouter.beeSelectEvent += OnSelect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelect(GameObject selectedBee)
    {
        Selectable selectable = selectedBee.transform.parent.GetComponentInChildren<Selectable>();
        if (selectable != null)
        {
            ClearSelection();
            Select(selectable);
        }
    }
}
