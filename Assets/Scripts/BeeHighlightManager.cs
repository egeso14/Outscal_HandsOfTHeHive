
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public enum HighlightColor
{
    Red,
    Green,
    Blue
}

public class BeeHighlightManager : MonoBehaviour
{
    [SerializeField] private RenderingLayerMask redHighlightMask;
    [SerializeField] private RenderingLayerMask greenHighlightMask;
    [SerializeField] private RenderingLayerMask blueHighlightMask;
    [SerializeField] private RenderingLayerMask outlineBorderMask; // This is used for the outline border
    private RenderingLayerMask allHighlightsMask;

    private static BeeHighlightManager instance;
    public static void HighlightMe(GameObject bee, HighlightColor color)
    {
        var renderer = bee.GetComponentInChildren<Renderer>();
        Debug.Assert(renderer != null, "Couldn't find this bee's renderer");
        // first clear the past highlight
        renderer.renderingLayerMask &= ~((uint)instance.allHighlightsMask);
        // then add the new highlight
        switch (color)
        {
            case HighlightColor.Red:
                renderer.renderingLayerMask |= instance.redHighlightMask;
                break;
            case HighlightColor.Green:
                renderer.renderingLayerMask |= instance.greenHighlightMask;
                break;
            case HighlightColor.Blue:
                renderer.renderingLayerMask |= instance.blueHighlightMask;
                break;
            default:
                Debug.LogWarning("Unknown highlight color specified.");
                break;
        }
    }


    public static void AddOutlineBorderToMe(GameObject bee)
    {
        var renderer = bee.GetComponentInChildren<Renderer>();
        Debug.Assert(renderer != null, "Couldn't find this bee's renderer");
        renderer.renderingLayerMask |= instance.outlineBorderMask;
    }
    public static void RemoveOutlineBorderFromMe(GameObject bee)
    {
        var renderer = bee.GetComponentInChildren<Renderer>();
        Debug.Assert(renderer != null, "Couldn't find this bee's renderer");
        renderer.renderingLayerMask &= ~((uint)instance.outlineBorderMask);
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of BeeHighlightManager detected. Destroying the new instance.");
            Destroy(gameObject);
        }

        allHighlightsMask = redHighlightMask | greenHighlightMask | blueHighlightMask;
    }


}
