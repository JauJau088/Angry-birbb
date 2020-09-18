using UnityEngine;

public class ParallaxController : MonoBehaviour {
    // The bigger the factor, the slower the movement following the camera, vice versa
    // scaleFactor >> for BG
    // scaleFactor << for FG
    private CameraController cam;
    private Vector2 initPos, deltaPos;
    private Vector3 camInit;
    private float scaleFactor;

    public int index;

    private void Start() {
        scaleFactor = FindObjectOfType<ParallaxSettings>().parallaxes[index].scaleFactor;
        Debug.Log("scaleFactor = " + scaleFactor);
        initPos = transform.position;
        deltaPos = Camera.main.transform.position - camInit;
    }

    private void Update() {
        if (camInit == null) {
            camInit = cam.camInit;
        }

        transform.position = new Vector3 (
            initPos.x + (Camera.main.transform.position.x - deltaPos.x) / scaleFactor,
            initPos.y + (Camera.main.transform.position.y - deltaPos.y) / scaleFactor,
            transform.position.z
        );
    }
}
