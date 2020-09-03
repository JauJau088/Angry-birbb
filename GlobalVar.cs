using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    // other scripts
    private Apoint aPoint;
    private Bird bird;
    private CameraController cameraController;
    private Enemy enemy;
    private LevelController levelController;

    // global variables
    public Vector2 pointInitPos;
    public Vector2 birdInitPos;

    private void Awake() {
        // initialize
        aPoint = GameObject.FindObjectOfType<Apoint>();
        bird = GameObject.FindObjectOfType<Bird>();
        cameraController = GameObject.FindObjectOfType<CameraController>();
        enemy = GameObject.FindObjectOfType<Enemy>();
        levelController = GameObject.FindObjectOfType<LevelController>();

        // pass the values to the other scripts
        aPoint.pointInitPos(pointInitPos);
        
        bird.birdInitPos(birdInitPos);
        
        cameraController.pointInitPos(pointInitPos);
        cameraController.birdInitPos(birdInitPos);
    }
}
