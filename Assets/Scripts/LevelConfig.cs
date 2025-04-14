using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField] public Bounds levelDimensions;

    //Camera
    [SerializeField] public float baseCameraDistance;
    [SerializeField] public float baseCameraSpeed;
    [SerializeField] public float maxCameraZoomInFactor;
    [SerializeField] public float maxCameraZoomOutFactor;
}
