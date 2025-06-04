using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera myCamera;
    [SerializeField] private float cameraMovementSpeed;
    private float zoomLerpConstant;
    [SerializeField] private float moveLerpConstant;
    [SerializeField] private float cameraRotationSpeed;
    private float cameraMovementRange_Z;
    private float cameraDefaultDistance_Z;
    private float cameraZoomSpeed;
    private float maxFOV;
    private float minFOV;
    private Bounds backgroundBounds;
    private Bounds cameraMovementBounds;

    [SerializeField] private InputReader inputReader;


    // Coroutines

    private Coroutine zoomCoroutine;
    private Coroutine moveCoroutine;
    void Start()
    {
        myCamera = GetComponent<Camera>();
        LevelConfig levelConfig = LevelConfig.instance;
        Debug.Assert(levelConfig != null, "LevelConfig instance is null in CameraController.");
        cameraMovementRange_Z = levelConfig.cameraMovementRange;
        cameraDefaultDistance_Z = levelConfig.baseCameraDistance;
        cameraZoomSpeed = levelConfig.cameraZoomSpeed;
        zoomLerpConstant = levelConfig.cameraZoomLerpConstant;
        maxFOV = levelConfig.cameraMaxFOV;
        minFOV = levelConfig.cameraMinFOV;

        FindBackgroundBounds();
        CreateCameraMovementBounds();
        SetStartingPosition();
        SetStartingRotation();
    }

    private void OnEnable()
    {
        inputReader.MoveEvent += OnMove;
        inputReader.ZoomEvent += OnZoom;
        inputReader.PauseEvent += OnPause;
    }



    private void OnDisable()
    {
        inputReader.MoveEvent -= OnMove;
        inputReader.ZoomEvent -= OnZoom;
        inputReader.PauseEvent -= OnPause;
    }
    #region Callbacks
    private void OnPause()
    {
        inputReader.MoveEvent -= OnMove;
        inputReader.ZoomEvent -= OnZoom;
        inputReader.PauseEvent -= OnPause;

        inputReader.ResumeEvent += OnResume;
    }

    private void OnResume()
    {
        inputReader.ResumeEvent -= OnResume;

        inputReader.MoveEvent += OnMove;
        inputReader.ZoomEvent += OnZoom;
        inputReader.PauseEvent += OnPause;
    }

    private void OnZoom(float zoom)
    {
        float targetFOV = myCamera.fieldOfView + zoom * cameraZoomSpeed;
        
        
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }

        zoomCoroutine = StartCoroutine(SmoothZoom(targetFOV));

        IEnumerator SmoothZoom(float targetFOV)
        {
            while (targetFOV != myCamera.fieldOfView)
            {
                float newFOV = Mathf.Lerp(myCamera.fieldOfView, targetFOV, zoomLerpConstant);
                myCamera.fieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
                yield return null;
            }
        }
    }

    private void OnMove(Vector2 move)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (move == Vector2.zero) return;
       

        moveCoroutine = StartCoroutine(SmoothMove(new Vector3(move.x, move.y)));

        IEnumerator SmoothMove(Vector3 moveDirection)
        {
            Vector3 targetPosition;
            while (true)
            {
                targetPosition = transform.position + moveDirection * cameraMovementSpeed;
                float newX = Mathf.Lerp(transform.position.x, targetPosition.x, moveLerpConstant);
                float newY = Mathf.Lerp(transform.position.y, targetPosition.y, moveLerpConstant);
                transform.position = new Vector3(newX, newY, transform.position.z);
                yield return null;
            }
        }
    }
    #endregion
    private void FindBackgroundBounds()
    {
        var backgroundObject = GameObject.FindGameObjectWithTag("BackgroundImage");
        Debug.Assert(backgroundObject != null, "Couldn't find any object with tag BackgroundImage");
        BackgroundProperties backgroundProperties = backgroundObject.GetComponent<BackgroundProperties>();
        Debug.Assert(backgroundProperties != null, "Background object doesn't have background properties component");
        backgroundBounds = backgroundProperties.GetBackgroundBounds();
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
