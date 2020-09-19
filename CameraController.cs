using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject boundary;
    private Bird bird;
    private Apoint aPoint;

    private float leftLim, rightLim, bottomLim, topLim;
    private float leftCam, rightCam, bottomCam, topCam, camHeight, camWidth;
    private float minX, maxX, minY, maxY;

    public Vector2 camInit;
    public float lerpFactor = 0.125f;
    public bool trigger = false;
    public string bound = "CamBoundary";
    
    private void Start() {
        StartCoroutine(CamBound("CamBoundary"));
    }

    private void FixedUpdate() {
        if (trigger == true) {
            StartCoroutine(CamBound(bound));
        }

        // only if not null, follow bird's and the reference point aPoint movements
        if (bird != null && aPoint != null) {
            // cam follow birb and smoothen its movement
            Vector3 desiredPosition = new Vector3 (
                bird.transform.position.x + (aPoint.transform.position.x - bird.transform.position.x) / 2,
                bird.transform.position.y + (aPoint.transform.position.y - bird.transform.position.y) / 2,
                transform.position.z
            );

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, lerpFactor);

            transform.position = smoothedPosition;
            
            // limit to where cam can move
            transform.position = new Vector3 (
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY),
                transform.position.z
            );
        }
    }

    //===================================================================||  CAM BOUNDARY FUNCTION
    IEnumerator CamBound(string boundaryName) {
        // init
        boundary = GameObject.Find(boundaryName);
        bird = FindObjectOfType<Bird>();
        aPoint = FindObjectOfType<Apoint>();

        // cam init pos
        camInit.x = bird.initPos.x + (aPoint.initPos.x - bird.initPos.x) / 2;
        camInit.y = bird.initPos.y + (aPoint.initPos.y - bird.initPos.y) / 2;

        // Cam height and width in WorldPoint
        // orthographicSize * 2 = full height of the cam in WorldPoint
        camHeight = Camera.main.orthographicSize * 2;
        camWidth = camHeight * Screen.width / Screen.height;

        // Camera boundaries
        leftCam = camInit.x - camWidth / 2;
        rightCam = camInit.x + camWidth / 2;
        bottomCam = camInit.y - camHeight / 2;
        topCam = camInit.y + camHeight / 2;

        // Reference boundaries
        leftLim = boundary.GetComponent<Renderer>().bounds.min.x;
        rightLim = boundary.GetComponent<Renderer>().bounds.max.x;
        bottomLim = boundary.GetComponent<Renderer>().bounds.min.y;
        topLim = boundary.GetComponent<Renderer>().bounds.max.y;

        // Min and max x and y coordinate in which camera may move to
        minX = camInit.x + (leftLim - leftCam);
        maxX = camInit.x + (rightLim - rightCam);
        minY = camInit.y + (bottomLim - bottomCam);
        maxY = camInit.y + (topLim - topCam);

        trigger = false;

        yield return null;
    }
    //===================================================================||  END OF CAM BOUNDARY FUNCTION
}
