using UnityEngine;

public class GlobalVar : MonoBehaviour
{
    // global variables
    public Vector2 pointInitPos, birdInitPos;
    // do not serialize this
    public Vector2 camInitPos;

    private void Awake() {
        camInitPos.x = birdInitPos.x + (pointInitPos.x - birdInitPos.x) / 2;
        camInitPos.y = birdInitPos.y + (pointInitPos.y - birdInitPos.y) / 2;
    }
}
