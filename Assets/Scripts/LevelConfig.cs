using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    private static LevelConfig _instance;

    public static LevelConfig instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<LevelConfig>(typeof(LevelConfig).Name);

                if (_instance == null)
                {
                    Debug.LogError($"Singleton ScriptableObject of type {typeof(LevelConfig).Name} not found in Resources!");
                }
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
        beeSpawnLocation = new Vector3(backgroundCenterPositon.x,
                                        backgroundCenterPositon.y,
                                        backgroundCenterPositon.z - flyZoneDepth / 2);
    }
    //Camera
    [SerializeField] public float baseCameraDistance;
    [SerializeField] public float baseCameraSpeed;
    [SerializeField] public float maxCameraZoomInFactor;
    [SerializeField] public float maxCameraZoomOutFactor;

    //Background
    [SerializeField] public Vector2 backgroundDimensions;
    [SerializeField] public Vector3 backgroundCenterPositon;

    //Gameplay
    [SerializeField] public float flyZoneDepth;
    [SerializeField] public float beeSpawnInterval;
    [SerializeField] public int maxBees;
    [SerializeField] public List<LayerMask> selectableLayers;


    // LevelSetup
    [SerializeField] public Vector2 hiveDistanceToSpawnPoint;
    [SerializeField] public float navmeshEdgeLength;
    public Vector3 beeSpawnLocation;
}
