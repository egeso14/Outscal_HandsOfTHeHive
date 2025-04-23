using System.Drawing;
using UnityEngine;
using UnityEngine.Analytics;

public class FlyBoundaryFactory : MonoBehaviour
{
    private LevelConfig levelConfig;

    // Boundary Game Objects
    private GameObject leftBoundary;
    private GameObject rightBoundary;
    private GameObject topBoundary;
    private GameObject bottomBoundary;
    private GameObject frontBoundary;
    private GameObject backBoundary;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelConfig = LevelConfig.instance;
        Debug.Assert(levelConfig != null);
        CreateBoundaryColliders();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateBoundaryColliders()
    {


        float thickness = 1f; // Arbitrary thickness value for the boundaries
        Vector3 center = levelConfig.backgroundCenterPositon;
        Vector3 size = levelConfig.backgroundDimensions; 

        // FRONT boundary
        frontBoundary = new GameObject("FrontBoundary");
        frontBoundary.transform.parent = this.transform;
        frontBoundary.transform.position = center - new Vector3(0, 0, size.z / 2 + thickness / 2 + levelConfig.flyZoneDepth);
        frontBoundary.AddComponent<BoxCollider>().size = new Vector3(size.x, size.y, thickness);

        // BACK boundary
        backBoundary = new GameObject("BackBoundary");
        backBoundary.transform.parent = this.transform;
        backBoundary.transform.position = center + new Vector3(0, 0, size.z / 2 + thickness / 2);
        backBoundary.AddComponent<BoxCollider>().size = new Vector3(size.x, size.y, thickness );

        // LEFT boundary
        leftBoundary = new GameObject("LeftBoundary");
        leftBoundary.transform.parent = this.transform;
        leftBoundary.transform.position = center - new Vector3(size.x / 2 + thickness / 2, 0, 0 + levelConfig.flyZoneDepth / 2);
        leftBoundary.AddComponent<BoxCollider>().size = new Vector3(thickness, size.y, size.z + levelConfig.flyZoneDepth);

        // RIGHT boundary
        rightBoundary = new GameObject("RightBoundary");
        rightBoundary.transform.parent = this.transform;
        rightBoundary.transform.position = center + new Vector3(size.x / 2 + thickness / 2, 0, 0 - levelConfig.flyZoneDepth / 2);
        rightBoundary.AddComponent<BoxCollider>().size = new Vector3(thickness, size.y, size.z + levelConfig.flyZoneDepth);

        // TOP boundary
        topBoundary = new GameObject("TopBoundary");
        topBoundary.transform.parent = this.transform;
        topBoundary.transform.position = center + new Vector3(0, size.y / 2 + thickness / 2, 0 - levelConfig.flyZoneDepth / 2);
        topBoundary.AddComponent<BoxCollider>().size = new Vector3(size.x, thickness, size.z + levelConfig.flyZoneDepth);

        // BOTTOM boundary
        bottomBoundary = new GameObject("BottomBoundary");
        bottomBoundary.transform.parent = this.transform;
        bottomBoundary.transform.position = center - new Vector3(0, size.y / 2 + thickness / 2, 0 + levelConfig.flyZoneDepth / 2);
        bottomBoundary.AddComponent<BoxCollider>().size = new Vector3(size.x, thickness, size.z + levelConfig.flyZoneDepth);

    }
}
