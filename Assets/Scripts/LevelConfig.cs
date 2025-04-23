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
}
