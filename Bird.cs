using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    [HideInInspector] public Vector3 initialPosition;
    private bool birdWasLaunched;
    private float timeSittingAround = 0;
    [SerializeField] private int launchPower = 2300;
    [SerializeField] private float gravity = (float)0.14;

    private void Awake() {
        initialPosition =  GameObject.FindObjectOfType<GlobalVar>().birdInitPos;

        transform.position = initialPosition;
    }

    private void Update() {
        // line renderer
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, initialPosition);

        // if bird was launched and move very slowly, start timer
        if (birdWasLaunched && GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1) {
            timeSittingAround += Time.deltaTime;
        }

        // if out of screen or stay still few sec, reset
        if (transform.position.x > 15 || transform.position.x < -15 ||
            transform.position.y > 15 || transform.position.y < -15 ||
            timeSittingAround > 3)
        {
            transform.position = initialPosition;
            transform.rotation = Quaternion.identity;
            birdWasLaunched = false;
            timeSittingAround = 0;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            FindObjectOfType<AudioManager>().Stop("BirdFly");
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // turn off SFX
            FindObjectOfType<AudioManager>().Stop("BirdFly");
    }

    private void OnMouseDown() {
        if (birdWasLaunched == false) {
            // overlay the sprite color
            GetComponent<SpriteRenderer>().color = Color.red;

            // show LineRenderer
            GetComponent<LineRenderer>().enabled = true;

            // play SFX
            FindObjectOfType<AudioManager>().Play("RubberStretch");
        }
    }
    
    private float angle;
    [SerializeField] private float radius = (float)0.5;

    private void OnMouseDrag() {
        if (birdWasLaunched == false) {
            ////////////
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // direction of the target
            Vector2 direction = target - initialPosition;

            // if target is above origin, then value = negative
            // so, top angles are between 0 and 180, bottom angles are between 0 and -180
            float sign;

            sign = (direction.y >= 0) ? 1 : -1;

            // right = reference
            angle = Vector2.Angle(Vector2.right, direction) * sign;

            // calculate the new x y positions based on predetermined radius
            Vector2 newPosition, n;

            // max boundaries
            newPosition.x = initialPosition.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            newPosition.y = initialPosition.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            // clamp x when outside of boundary
            if (direction.x > 0 )
                n.x = (target.x > newPosition.x) ? newPosition.x : target.x;
            else
                n.x = (target.x < newPosition.x) ? newPosition.x : target.x;
            // clamp y when outside of boundary
            if (direction.y > 0 )
                n.y = (target.y > newPosition.y) ? newPosition.y : target.y;
            else
                n.y = (target.y < newPosition.y) ? newPosition.y : target.y;

            // smoother first
            Vector3 smoothedPosition = Vector2.Lerp(transform.position, n, 0.125f);
            // move position
            transform.position = smoothedPosition;
        }
    }

    private void OnMouseUp() {
        if (birdWasLaunched == false) {
            // set color back to original
            GetComponent<SpriteRenderer>().color = Color.white;

            // move to the opposite direction with force multiplier = launchPower
            Vector2 directionToInitialPosition = initialPosition - transform.position;
            GetComponent<Rigidbody2D>().AddForce(directionToInitialPosition * launchPower);

            // set gravity
            GetComponent<Rigidbody2D>().gravityScale = gravity;

            // hide LineRenderer
            GetComponent<LineRenderer>().enabled = false;

            // bird launch trigger var
            birdWasLaunched = true;

            // turn off SFX
            FindObjectOfType<AudioManager>().Stop("RubberStretch");

            // play SFX
            FindObjectOfType<AudioManager>().Play("BirdFly");
        }
    }
}
