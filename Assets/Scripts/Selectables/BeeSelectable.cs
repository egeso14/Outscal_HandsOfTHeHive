using UnityEngine;

public class BeeSelectable : Selectable
{
    private SkinnedMeshRenderer meshRenderer;
    private BeeBrain beeBrain;
    [SerializeField] private RenderingLayerMask defaultRenderingMask;
    [SerializeField] private RenderingLayerMask highlightRenderingMask;


    private void Start()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Debug.Assert(meshRenderer != null);
        beeBrain = GetComponent<BeeBrain>();
        Debug.Assert(beeBrain != null);



        if (defaultRenderingMask == 0 || highlightRenderingMask == 0)
        {
            Debug.LogError("Default rendering mask is not set.");
            return;
        }

    }
    protected override void RespondToSelection()
    {
        BeeHighlightManager.AddOutlineBorderToMe(gameObject);
        CommandEngine.commandBroadcast += CommunicateCommand;
    }

    protected override void RespondToDeselection()
    {
        BeeHighlightManager.RemoveOutlineBorderFromMe(gameObject);
        CommandEngine.commandBroadcast -= CommunicateCommand;
    }

    private void CommunicateCommand(CommandData commandData)
    {
        beeBrain.InformOfCommand(commandData);
    }

    
}
