using UnityEngine;

public class Destroyer : MonoBehaviour
{
    GameObject levelBoundary;
    private float maxX, minX, maxY, minY;

    private void Awake() {
        levelBoundary = GameObject.Find("LevelBoundary");
    }

    private void Start() {
        minX = levelBoundary.GetComponent<Renderer>().bounds.min.x;
        maxX = levelBoundary.GetComponent<Renderer>().bounds.max.x;
        minY = levelBoundary.GetComponent<Renderer>().bounds.min.y;
        maxY = levelBoundary.GetComponent<Renderer>().bounds.max.y;
    }

    private void Update() {
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY) {
                Destroy(gameObject);
        }
    }
}
