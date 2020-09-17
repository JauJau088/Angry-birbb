using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject boundary;
    Bird bird;
    Apoint aPoint;

    private float leftLim, rightLim, bottomLim, topLim;
    private float leftCam, rightCam, bottomCam, topCam, camHeight, camWidth;
    private float minX, maxX, minY, maxY;

    public Vector2 camInit;
    public float lerpFactor = 0.125f;

//===================================================================||  DEFINE CAM BOUNDARIES
    private void Start() {
        // init
        boundary = GameObject.Find("boundary");
        bird = FindObjectOfType<Bird>();
        aPoint = FindObjectOfType<Apoint>();

        Debug.Log("apint init pos = " + aPoint.transform.position);
        Debug.Log("apint init pos init = " + aPoint.initPos);

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
    }
//===================================================================||  END OF DEFINE CAM BOUNDARIES

    private void FixedUpdate() {
        // re-init on new level
        // this is necessary to handle scene transition because the obj will be destroyed
        if (bird == null) {
            bird = FindObjectOfType<Bird>();
        }

        if (aPoint == null) {
            aPoint = FindObjectOfType<Apoint>();
        }

        // only if not null, follow bird's movements and the reference point aPoint
        if (bird != null && aPoint != null) {
            // cam follow birb and smoothen its movement
            Vector3 desiredPosition = new Vector3 (
                bird.transform.position.x + (aPoint.transform.position.x - bird.transform.position.x) / 2,
                bird.transform.position.y + (aPoint.transform.position.y - bird.transform.position.y) / 2,
                transform.position.z
            );

            Debug.Log("bird's pos = " + bird.transform.position);
            Debug.Log("apoint's pos = " + aPoint.transform.position);
            Debug.Log("desiredPosition = " + desiredPosition);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, lerpFactor);

            transform.position = smoothedPosition;

            Debug.Log("transform.pos1" + transform.position);
            
            // limit to where cam can move
            transform.position = new Vector3 (
                Mathf.Clamp(transform.position.x, minX, maxX),
                Mathf.Clamp(transform.position.y, minY, maxY),
                transform.position.z
            );

            /*
            Debug.Log("minX = " + minX);
            Debug.Log("maxX = " + maxX);
            Debug.Log("minY = " + minY);
            Debug.Log("maxY = " + maxY);
            */

            Debug.Log("transform.pos2" + transform.position);
        }
    }

    // debugging: just a nice overview on the boundary 
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector2(minX, maxY), new Vector2(maxX, maxY));
        Gizmos.DrawLine(new Vector2(maxX, maxY), new Vector2(maxX, minY));
        Gizmos.DrawLine(new Vector2(maxX, minY), new Vector2(minX, minY));
        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(minX, maxY));
    }
}
