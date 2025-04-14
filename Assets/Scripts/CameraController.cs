using System.ComponentModel;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera myCamera;
    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private float cameraRotationSpeed;
    [SerializeField] private float cameraMovementRange_Z;
    [SerializeField] private float cameraDefaultDistance_Z;

    [SerializeField, ReadOnly(true)] private Bounds backgroundBounds;
    [SerializeField, ReadOnly(true)] private Bounds cameraMovementBounds;
    void Start()
    {
        myCamera = GetComponent<Camera>();
        FindBackgroundBounds();
        CreateCameraMovementBounds();
        SetStartingPosition();
        SetStartingRotation();
    }

   
    void Update()
    {
        
    }

    private void FindBackgroundBounds()
    {
        var bgImageObject = GameObject.FindGameObjectWithTag("BackgroundImage");
        Debug.Assert(bgImageObject != null, "bgImageObject is null!");
        var renderer = bgImageObject.GetComponent<Renderer>();
        Debug.Assert(renderer != null, "renderer is null!");
        backgroundBounds = renderer.bounds;
        Debug.Assert(backgroundBounds != null, "backgroundBounds is null");
    }

    private void CreateCameraMovementBounds()
    {
        
        Vector3 boundCenter = new Vector3 (backgroundBounds.center.x, 
                                           backgroundBounds.center.y,
                                           backgroundBounds.min.z - cameraDefaultDistance_Z);
        Vector3 extents = new Vector3(backgroundBounds.extents.x,
                                      backgroundBounds.extents.y,
                                      cameraMovementRange_Z);
        cameraMovementBounds = new Bounds(boundCenter, extents);
    }

    private void SetStartingPosition()
    {
        transform.position = cameraMovementBounds.center;
    }

    private void SetStartingRotation()
    {
        myCamera.transform.LookAt(backgroundBounds.center);
    }
}
