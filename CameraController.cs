using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject boundary;
    private Bird bird;
    private Apoint aPoint;

    private Vector3 desiredPosition, smoothedPosition, velocity = Vector3.zero;
    private float leftLim, rightLim, bottomLim, topLim;
    private float leftCam, rightCam, bottomCam, topCam, camHeight, camWidth;
    private float minX, maxX, minY, maxY;
    private float dampTime, dampPlay = 0.2f, dampTransition = 2f;
    private string bound = "CamBoundary";

    public Vector2 camInit;
    public bool triggerTransition = false, triggerPlay = false;
    
    private void Start() {
        dampTime = dampPlay;
        StartCoroutine(CamBound("CamBoundary"));
    }

    private void FixedUpdate() {
        //===================================================================||  TRIGGERS CONNECTED TO STATE MACHINE
        if (triggerTransition == true) {
            dampTime = dampTransition;

            // re-init cam bound
            bound = "CamTransitionBoundary";
            StartCoroutine(CamBound(bound));

            // turn trigger off
            triggerTransition = false;

            return;
        }

        if (triggerPlay == true) {
            dampTime = dampPlay;

            // re-init cam bound
            bound = "CamBoundary";
            StartCoroutine(CamBound(bound));

            // turn trigger off
            triggerPlay = false;

            return;
        }
        //===================================================================||  END OF TRIGGERS CONNECTED TO STATE MACHINE

        //===================================================================||  CAMERA FOLLOW
        // only if not null, follow bird's and the reference point aPoint movements
        if (bird != null && aPoint != null) {
            // cam follow birb and smoothen its movement
            desiredPosition = new Vector3 (
                bird.transform.position.x + (aPoint.transform.position.x - bird.transform.position.x) / 2,
                bird.transform.position.y + (aPoint.transform.position.y - bird.transform.position.y) / 2,
                transform.position.z
            );

            smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, dampTime);

            transform.position = smoothedPosition;
            
            // limit to where cam can move
            transform.position = new Vector3 (
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY),
                transform.position.z
            );
        }
        //===================================================================||  END OF CAMERA FOLLOW
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

        yield return null;
    }
    //===================================================================||  END OF CAM BOUNDARY FUNCTION
}
