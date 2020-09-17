using UnityEngine;

public class ParallaxFG0 : MonoBehaviour {
    // The bigger the factor, the slower the movement following the camera, vice versa
    // scaleFactor >> for BG
    // scaleFactor << for FG
    CameraController cam;
    private float scaleFactor;
    Vector2 initPos, deltaPos;
    Vector3 camInit;

    private void Start() {
        scaleFactor = FindObjectOfType<GlobalVar>().parallaxFG0;
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
