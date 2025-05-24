using UnityEngine;

public abstract class Selectable: MonoBehaviour
{
    private bool isSelected;
    public void Select()
    {
        isSelected = true;
        RespondToSelection();
    }

    public void Deselect()
    {
        isSelected = false;
        RespondToDeselection();
    }

    protected abstract void RespondToSelection();
    protected abstract void RespondToDeselection();

}
