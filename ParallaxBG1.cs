﻿using UnityEngine;

public class ParallaxBG1 : MonoBehaviour {
    // The bigger the factor, the slower the movement following the camera, vice versa
    // scaleFactor >> for BG
    // scaleFactor << for FG
    private float scaleFactor;
    Vector2 initPos, deltaPos;
    Vector3 camInit;

    private void Awake() {
        scaleFactor = GameObject.FindObjectOfType<GlobalVar>().parallaxBG1;
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
