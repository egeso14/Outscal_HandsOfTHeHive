using UnityEngine;

public class SelectSingle: MonoBehaviour
{
    private InputReader inputReader;
    private LayerMask selectableLayers;
    private float length_RayFromScreenToWorld = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Assert(inputReader != null);
        inputReader.SelectEvent += OnSelect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelect(Vector2 mousePosition)
    {
        // do a raycast to determine whether there are any selectables underneath the mouse position
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, length_RayFromScreenToWorld, selectableLayers))
        {
            Selectable selectable = hit.collider.GetComponent<Selectable>();
            if (selectable != null)
            {
                selectable.Select();
            }
        }
    }
}
