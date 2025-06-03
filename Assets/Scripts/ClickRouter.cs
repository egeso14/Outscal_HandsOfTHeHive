using System;
using UnityEngine;
using UnityEngine.Rendering;

public class ClickRouter: MonoBehaviour
{
    public static ClickRouter instance;
    [SerializeField] private InputReader inputReader;
    private float clickRadius;
    private LevelConfig levelConfig;
    public static event Action<GameObject> beeSelectEvent;
    public static event Action<Vector3> mapClickEvent;
    public static event Action<Vector3> uiClickEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        levelConfig = LevelConfig.instance;
        Debug.Assert(levelConfig != null, "LevelConfig is null in ClickRouter.");
        clickRadius = levelConfig.clickRadius;

        Debug.Assert(inputReader != null, "InputReader is not assigned in ClickRouter.");
        AssignCallbacks();
    }

    private void AssignCallbacks()
    {
        inputReader.ScreenClickEvent += NotifyAppropriateListeners;
    }

    private void NotifyAppropriateListeners(Vector2 positionOnScreen)
    {


        var maxCameraDistance = levelConfig.baseCameraDistance +levelConfig.cameraMovementRange;
        // do a raycast with a layermask that includes all possible layers
        Ray ray = Camera.main.ScreenPointToRay(positionOnScreen);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Bee", "Terrain", "Background"); // Adjust layer names as necessary
        if (Physics.SphereCast(ray.origin, clickRadius, ray.direction, out hit, maxCameraDistance, layerMask))
        {
            Vector3 worldPos = hit.point;
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Bee"))
            {
                beeSelectEvent?.Invoke(hit.collider.gameObject);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                     hit.collider.gameObject.layer == LayerMask.NameToLayer("Background"))
            {
                mapClickEvent?.Invoke(worldPos);
            }
        }
    }

}
