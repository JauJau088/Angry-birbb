using UnityEngine;

public class Apoint : MonoBehaviour
{
    private Vector2 initialPosition;

    private void Awake() {
        initialPosition =  GameObject.FindObjectOfType<GlobalVar>().pointInitPos;

        transform.position = initialPosition;
    }
}
