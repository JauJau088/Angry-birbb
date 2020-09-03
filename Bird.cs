using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    public Vector3 _initialPosition;
    private bool _birdWasLaunched;
    private float _timeSittingAround = 0;
    [SerializeField] private int _launchPower = 180;
    [SerializeField] private float _gravity = (float)0.5;

    public void birdInitPos (Vector2 value) {
        _initialPosition = value;

        transform.position = _initialPosition;
    }

    private void Update() {
        // line renderer
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, _initialPosition);

        // if bird was launched and move very slowly
        if (_birdWasLaunched && GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1) {
            _timeSittingAround += Time.deltaTime;
        }

        // if out of screen or stay still few sec, reset
        if (transform.position.x > 30 || transform.position.x < -30 ||
            transform.position.y > 30 || transform.position.y < -30 ||
            _timeSittingAround > 3)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);

            // turn off SFX
            FindObjectOfType<AudioManager>().Stop("BirdFly");
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // turn off SFX
            FindObjectOfType<AudioManager>().Stop("BirdFly");
    }

    private void OnMouseDown() {
        // overlay the sprite color
        GetComponent<SpriteRenderer>().color = Color.red;

        // show LineRenderer
        GetComponent<LineRenderer>().enabled = true;

        // play SFX
        FindObjectOfType<AudioManager>().Play("RubberStretch");
    }
    
    private float angle;
    [SerializeField] private float radius = (float)0.5;

    private void OnMouseDrag() {
        ////////////
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // direction of the target
        Vector2 direction = target - _initialPosition;

        // if target is above origin, then value = negative
        // so, top angles are between 0 and 180, bottom angles are between 0 and -180
        float sign;

        sign = (direction.y >= 0) ? 1 : -1;

        // right = reference
        angle = Vector2.Angle(Vector2.right, direction) * sign;

        // calculate the new x y positions based on predetermined radius
        Vector2 newPosition, n;

        // max boundaries
        newPosition.x = _initialPosition.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        newPosition.y = _initialPosition.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

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

    private void OnMouseUp() {
        // set color back to original
        GetComponent<SpriteRenderer>().color = Color.white;

        // move to the opposite direction with force multiplier = _launchPower
        Vector2 directionToInitialPosition = _initialPosition - transform.position;
        GetComponent<Rigidbody2D>().AddForce(directionToInitialPosition * _launchPower);

        // set gravity back to 1
        GetComponent<Rigidbody2D>().gravityScale = _gravity;

        // hide LineRenderer
        GetComponent<LineRenderer>().enabled = false;

        //
        _birdWasLaunched = true;

        // turn off SFX
        FindObjectOfType<AudioManager>().Stop("RubberStretch");

        // play SFX
        FindObjectOfType<AudioManager>().Play("BirdFly");
    }
}
