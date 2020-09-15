using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject birb, aPoint, boundary;

    private float leftLim, rightLim, bottomLim, topLim;
    private float leftCam, rightCam, bottomCam, topCam, camHeight, camWidth;
    private float minX, maxX, minY, maxY;
    private Vector2 camInit;

    public float lerpFactor = 0.125f;

    private void Awake() {
        
    }

    private void Start() {
        // init
        birb = GameObject.Find("GreenB");
        aPoint = GameObject.Find("aPoint");
        boundary = GameObject.Find("boundary");
        camInit = GameObject.FindObjectOfType<GlobalVar>().camInitPos;

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

    private void FixedUpdate() {
        Vector3 desiredPosition = new Vector3 (
            birb.transform.position.x + (aPoint.transform.position.x - birb.transform.position.x) / 2,
            birb.transform.position.y + (aPoint.transform.position.y - birb.transform.position.y) / 2,
            transform.position.z
        );

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, lerpFactor);

        transform.position = smoothedPosition;
        
        //
        transform.position = new Vector3 (
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector2(minX, maxY), new Vector2(maxX, maxY));
        Gizmos.DrawLine(new Vector2(maxX, maxY), new Vector2(maxX, minY));
        Gizmos.DrawLine(new Vector2(maxX, minY), new Vector2(minX, minY));
        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(minX, maxY));
    }
}
