using UnityEngine;

public class ParallaxFG : MonoBehaviour {
    // The bigger the factor, the slower the movement, vice versa
    // scaleFactor > 1 for BG
    // 0 < scaleFactor < 1 for FG
    private float scaleFactor = (float)5;
    Vector2 initPos, deltaPos;
    Vector3 camInit;

    private void Awake() {
        initPos = transform.position;
        camInit = GameObject.FindObjectOfType<GlobalVar>().camInitPos;
        deltaPos = Camera.main.transform.position - camInit;
    }

    private void Update() {
        transform.position = new Vector3 (
            initPos.x + (Camera.main.transform.position.x - deltaPos.x) / scaleFactor,
            initPos.y + (Camera.main.transform.position.y - deltaPos.y) / scaleFactor,
            transform.position.z
        );
    }
}
