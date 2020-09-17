using UnityEngine;

public class Apoint : MonoBehaviour
{
    public Vector2 initPos;

    private void Awake() {
        initPos = transform.position;
    }
}
