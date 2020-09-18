using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private Bird bird;
    private Enemy[] enemies;
    private GameObject levelBoundary;

    private static int levelIndex = 1;
    private string levelName, tmpName;
    private float waitTime = 0, start = 0, target = 56;
    private float minX, maxX, minY, maxY;
    private bool playTrigger = true, loadTrigger = false;
    bool tmp = false;

    private void Awake() {
        SceneManager.LoadScene("Level1");
    }

    private void Start() {
        bird = FindObjectOfType<Bird>();
        enemies = FindObjectsOfType<Enemy>();
        levelBoundary = GameObject.Find("levelBoundary");

        // level boundaries, to limit how far the bird can go
        minX = levelBoundary.GetComponent<Renderer>().bounds.min.x;
        maxX = levelBoundary.GetComponent<Renderer>().bounds.max.x;
        minY = levelBoundary.GetComponent<Renderer>().bounds.min.y;
        maxY = levelBoundary.GetComponent<Renderer>().bounds.max.y;
    }

    private void Update() {
        levelName = "Level" + levelIndex;

        // Level checker
        if (tmpName != levelName) {
            Debug.Log(levelName);
        }
        tmpName = levelName;

    //===================================================================||  PLAY STATE
        if (playTrigger == true) {
            // debug
            if (tmp != bird.birdWasLaunched) {
                Debug.Log("birdWasLaunched = " + bird.birdWasLaunched);
            }
            tmp = bird.birdWasLaunched;
            // end of debug

            // if bird was launched and move very slowly, start timer
            if (bird.birdWasLaunched && (bird.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1)) {
                waitTime += Time.deltaTime;
            }

            // if out of screen or stay still few sec
            if (bird.transform.position.x > maxX || bird.transform.position.x < minX ||
                bird.transform.position.y > maxY || bird.transform.position.y < minY ||
                waitTime > 3) {
                // then reset
                bird.transform.position = bird.initPos;
                bird.transform.rotation = Quaternion.identity;
                bird.birdWasLaunched = false;
                bird.GetComponent<Rigidbody2D>().gravityScale = 0;
                bird.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                bird.GetComponent<Rigidbody2D>().angularVelocity = 0;
                waitTime = 0;

                Debug.Log("bird position reset");
            }

            foreach (Enemy enemy in enemies) {
                if (enemy != null) {
                    return;
                }
            }

            // if this part is reached, then that means:
            Debug.Log("You killed all enemies");

            // stop the Play state
            playTrigger = false;
            Debug.Log("---------------------------- QUIT PLAY ----------------------------");

            // trigger load function
            loadTrigger = true;
        }
    //===================================================================||  END OF PLAY STATE

    //===================================================================||  LOAD STATE
        // load only once then reset the trigger back off
        if (loadTrigger == true) {
            StartCoroutine(Load());

            loadTrigger = false;
            StartCoroutine(loadTriggerReset());
        }
    //===================================================================||  END OF LOAD STATE
    }

    IEnumerator Load() {
        // prepare for next level
        levelIndex++;
        levelName = "Level" + levelIndex;

        // slower the cam
        FindObjectOfType<CameraController>().lerpFactor = 0.05f;

        // if next level is found
        if (Application.CanStreamedLevelBeLoaded(levelName)) {
            Debug.Log("==================== New level can be loaded");
            Debug.Log("==================== Init loading new Level");

            // load only once
            if (SceneManager.GetSceneByName(levelName).isLoaded == false) {
                SceneManager.LoadScene(levelName);
                Debug.Log("==================== Load new Level");
            }
        }
        // if not found, back to Level1
        else {
            Debug.Log("==================== Init loading Level1");

            // load only once
            if (SceneManager.GetSceneByName("Level1").isLoaded == false) {
                SceneManager.LoadScene("Level1");
                Debug.Log("==================== Load Level1");
                levelIndex = 1;
            }
        }

        // after loading, wait for few sec
        yield return new WaitForSeconds(3f);

        // re-init
        Debug.Log("---------------------------- re-init ----------------------------");

        bird = FindObjectOfType<Bird>();
        enemies = FindObjectsOfType<Enemy>();
        levelBoundary = GameObject.Find("levelBoundary");

        // level boundaries, to limit how far the bird can go
        minX = levelBoundary.GetComponent<Renderer>().bounds.min.x;
        maxX = levelBoundary.GetComponent<Renderer>().bounds.max.x;
        minY = levelBoundary.GetComponent<Renderer>().bounds.min.y;
        maxY = levelBoundary.GetComponent<Renderer>().bounds.max.y;

        // start the Play state
        playTrigger = true;
        Debug.Log("---------------------------- START PLAY ----------------------------");
    }

    IEnumerator loadTriggerReset() {
        yield return new WaitForSeconds(5f);

        loadTrigger = true;
    }
}
