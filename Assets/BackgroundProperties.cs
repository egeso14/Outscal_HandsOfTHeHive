using NUnit.Framework;
using UnityEngine;

public class BackgroundProperties : MonoBehaviour
{
    private LevelConfig configObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        configObject = LevelConfig.instance;
        Debug.Assert(configObject != null, "Couldn't find config object");
        SetTransformScale();
        //SetTransformPosition();
    }

    public Bounds GetBackgroundBounds()
    { 
        var renderer = GetComponent<Renderer>();
        Debug.Assert(renderer != null, "renderer is null!");
        return renderer.bounds;
    }

    public void SetTransformScale()
    {
        Vector3 scale = new Vector3(configObject.backgroundDimensions.x, configObject.backgroundDimensions.y, 1);
        transform.localScale = scale;
    }
    public void SetTransformPosition() => transform.position = configObject.backgroundCenterPositon;


    // Update is called once per frame
    void Update()
    {
        
    }
}
