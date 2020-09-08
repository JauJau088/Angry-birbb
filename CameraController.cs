using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject birb;
    GameObject aPoint;
    GameObject boundary;

    private float leftLim, rightLim, bottomLim, topLim;
    private float leftCam, rightCam, bottomCam, topCam;
    private float minX, maxX, minY, maxY;
    private Vector2 camToPos, birdInit, pointInit;

    private void Awake() {
        // init
        birb = GameObject.Find("GreenB");
        aPoint = GameObject.Find("aPoint");
        boundary = GameObject.Find("boundary");
        birdInit =  GameObject.FindObjectOfType<GlobalVar>().birdInitPos;
        pointInit =  GameObject.FindObjectOfType<GlobalVar>().pointInitPos;
    }

    private void Start() {
        //
        float initCenterX = birdInit.x + (pointInit.x - birdInit.x) / 2;
        float initCenterY = birdInit.y + (pointInit.y - birdInit.y) / 2;

        //
        camToPos.y = Camera.main.orthographicSize * 2;
        camToPos.x = camToPos.y * Screen.width / Screen.height;

        // cam
        //Vector3 camToPos = Camera.main.ScreenToWorldPoint(new Vector3 ((float)Screen.width, (float)Screen.height));

        leftCam = initCenterX - camToPos.x / 2;
        rightCam = initCenterX + camToPos.x / 2;
        bottomCam = initCenterY - camToPos.y / 2;
        topCam = initCenterY + camToPos.y / 2;

        // background boundary position
        leftLim = boundary.GetComponent<Renderer>().bounds.min.x;
        rightLim = boundary.GetComponent<Renderer>().bounds.max.x;
        bottomLim = boundary.GetComponent<Renderer>().bounds.min.y;
        topLim = boundary.GetComponent<Renderer>().bounds.max.y;

        Debug.Log("bottomLim = " + bottomLim);
        Debug.Log("Screen.width = " + Screen.width);
        Debug.Log("camToPos.x = " + camToPos.x);
        Debug.Log("y Ortho = " + camToPos.y);

        //
        minX = initCenterX + (leftLim - leftCam);
        maxX = initCenterX + (rightLim - rightCam);
        minY = initCenterY + (bottomLim - bottomCam);
        maxY = initCenterY + (topLim - topCam);
    }

    private void FixedUpdate() {
        Vector3 desiredPosition = new Vector3 (
            birb.transform.position.x + (aPoint.transform.position.x - birb.transform.position.x) / 2,
            birb.transform.position.y + (aPoint.transform.position.y - birb.transform.position.y) / 2,
            transform.position.z
        );

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 0.125f);

        transform.position = smoothedPosition;
        
        //
        transform.position = new Vector3 (
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }

    private void OnDrawGizmos() {
        //Vector3 camToPos = Camera.main.ScreenToWorldPoint();

        Gizmos.color = Color.red;

        //Debug.Log("Screen width = " + (float)Screen.width);
        //Debug.Log("posToCam.x = " + posToCam.x);

        Gizmos.DrawLine(new Vector2(minX, maxY), new Vector2(maxX, maxY));
        Gizmos.DrawLine(new Vector2(maxX, maxY), new Vector2(maxX, minY));
        Gizmos.DrawLine(new Vector2(maxX, minY), new Vector2(minX, minY));
        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(minX, maxY));
    }
}
