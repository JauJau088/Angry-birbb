using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject cloudParticlePrefab;
    private AudioManager audio;
    private string[] sound = {"Scream1", "Scream2", "Scream3", "Scream4"};
    private int i;

    private void Start() {
        audio = FindObjectOfType<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.GetComponent<Bird>() != null) {
            Instantiate(cloudParticlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null) {
            return;
        }

        // if hit from the top (y < 0)
        if (collision.contacts[0].normal.y < 0) {
            Instantiate(cloudParticlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        // if fall from above

        // to do
        // if on the ground and if
        //
        // upside down
        if (Vector3.Dot(transform.up, Vector3.down) > 0) {
            Instantiate(cloudParticlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        i = Random.Range(0,4);

        if (audio != null) {
            audio.Play(sound[i]);
        }
    }
}
