using UnityEngine;

public class Bird : MonoBehaviour
{
    public Vector3 initPos;
    public bool birdWasLaunched = false;
    private bool doThisOnce = true, doThisOnce2 = true, collided = false;
    private float timer = 0;
    [SerializeField] private int launchPower = 2300;
    [SerializeField] private float gravity = (float)0.14;

    private void Awake() {
        initPos = transform.position;
    }

    private void Update() {
        // line renderer
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, initPos);

        // if bird was launched, play magic sprinkle music after few mili sec
        if (birdWasLaunched && doThisOnce) {
            timer += Time.deltaTime;
            if (timer >= 0.3f) {
                FindObjectOfType<AudioManager>().Play("MagicSprinkle");

                doThisOnce = false;
                timer = 0;
            }
        }
        
        // then play BirdFly
        if (birdWasLaunched && doThisOnce2) {
            timer += Time.deltaTime;
            if (timer >= 0.2f) {
                FindObjectOfType<AudioManager>().Play("BirdFly");

                doThisOnce2 = false;
                timer = 0;
            }
        }

        // reset timer when collided
        if (collided) {
            collided = false;
            timer = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // collision detected
        collided = true;
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
        // debug start
            Vector3 n = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // smoother first
            Vector3 smoothedPosition = Vector2.Lerp(transform.position, n, 0.125f);
            // move position
            transform.position = smoothedPosition;
        // debug end
        /*
        if (birdWasLaunched == false) {
            ////////////
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // direction of the target
            Vector2 direction = target - initPos;

            // if target is above origin, then value = negative
            // so, top angles are between 0 and 180, bottom angles are between 0 and -180
            float sign;

            sign = (direction.y >= 0) ? 1 : -1;

            // right = reference
            angle = Vector2.Angle(Vector2.right, direction) * sign;

            // calculate the new x y positions based on predetermined radius
            Vector2 newPosition, n;

            // max boundaries
            newPosition.x = initPos.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            newPosition.y = initPos.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

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
        }*/
    }

    private void OnMouseUp() {
        if (birdWasLaunched == false) {
            // set color back to original
            GetComponent<SpriteRenderer>().color = Color.white;

            // move to the opposite direction with force multiplier = launchPower
            Vector2 directionToInitPos = initPos - transform.position;
            GetComponent<Rigidbody2D>().AddForce(directionToInitPos * launchPower);

            // set gravity
            GetComponent<Rigidbody2D>().gravityScale = gravity;

            // hide LineRenderer
            GetComponent<LineRenderer>().enabled = false;

            // bird launch trigger var
            birdWasLaunched = true;
            doThisOnce = true;
            doThisOnce2 = true;
            collided = false;

            // turn off SFX
            FindObjectOfType<AudioManager>().Stop("RubberStretch");

            // play SFX
            FindObjectOfType<AudioManager>().Play("RubberRelease");

            /*
            int rand = Random.Range(0,3);
            Debug.Log(rand);
            if (rand == 0) {
                FindObjectOfType<AudioManager>().Play("BirdFly");
            } else if (rand == 1) {
                FindObjectOfType<AudioManager>().Play("BirdFly2");
            } else {
                FindObjectOfType<AudioManager>().Play("BirdFly3");
            }
            */
        }
    }
}
