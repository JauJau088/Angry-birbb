using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    // global variables
    public Vector2 birdInitPos = new Vector2 (-10f, -2.5f);//, birdInitPos;
    public Vector2 pointInitPos = new Vector2 (4.5f, -0.5f);//, pointInitPos;
    // do not serialize this
    public Vector2 camInitPos;

    // parallax
    public float parallaxBG0 = 5, parallaxBG1 = 4, parallaxBG2 = 3;
    public float parallaxFG0 = 2;

    private void Awake() {
        //birdInitPos = birdInit;
        //pointInitPos = pointInit;

        camInitPos.x = birdInitPos.x + (pointInitPos.x - birdInitPos.x) / 2;
        camInitPos.y = birdInitPos.y + (pointInitPos.y - birdInitPos.y) / 2;
    }

    private void Update() {
        //birdInitPos = birdInit;
        //pointInitPos = pointInit;
    }
}
