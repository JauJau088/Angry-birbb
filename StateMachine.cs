using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateMachine : MonoBehaviour
{
    private Bird bird;
    private Enemy[] enemies;
    private GameObject levelBoundary;

    private static int levelIndex = 1;
    private string levelName, tmpName;
    private float waitTime = 0;
    private float minX, maxX, minY, maxY;
    private bool playTrigger = true;

    private void Awake() {
        SceneManager.LoadScene("Level1");
    }

    private void Start() {
        bird = FindObjectOfType<Bird>();
        enemies = FindObjectsOfType<Enemy>();
        levelBoundary = GameObject.Find("LevelBoundary");

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
            // if bird was launched and move very slowly, start timer
            if (bird.birdWasLaunched && (bird.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1)) {
                waitTime += Time.deltaTime;
            }

            // if out of screen or stay still few sec
            if (bird.transform.position.x > maxX || bird.transform.position.x < minX ||
                bird.transform.position.y > maxY || bird.transform.position.y < minY ||
                waitTime > 4.5f) {
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

            // STOP PLAY STATE
            Debug.Log("---------------------------- QUIT PLAY ----------------------------");
            playTrigger = false;

            // TRIGGER LOAD FUNCTION
            StartCoroutine(Load());
        }
        //===================================================================||  END OF PLAY STATE
    }

    IEnumerator Load() {
        //===================================================================||  LOAD STATE
        yield return new WaitForSeconds(4.5f);

        // prepare for next level
        levelIndex++;
        levelName = "Level" + levelIndex;

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
            levelIndex = 1;
            levelName = "Level" + levelIndex;

            // load only once
            if (SceneManager.GetSceneByName(levelName).isLoaded == false) {
                SceneManager.LoadScene(levelName);
                Debug.Log("==================== Load Level1");
            }
        }

        // return until finished loading
        yield return SceneManager.LoadSceneAsync(levelName);
        //===================================================================||  END OF LOAD STATE

        //===================================================================||  LEVEL TRANSITION STATE
        // re-init
        Debug.Log("==================== Re-init Game Objects");

        bird = FindObjectOfType<Bird>();
        enemies = FindObjectsOfType<Enemy>();
        levelBoundary = GameObject.Find("LevelBoundary");

        // re-init level boundaries, to limit how far the bird can go
        minX = levelBoundary.GetComponent<Renderer>().bounds.min.x;
        maxX = levelBoundary.GetComponent<Renderer>().bounds.max.x;
        minY = levelBoundary.GetComponent<Renderer>().bounds.min.y;
        maxY = levelBoundary.GetComponent<Renderer>().bounds.max.y;

        // set cam for level transition
        FindObjectOfType<CameraController>().triggerTransition = true;

        yield return new WaitForSeconds(6f);

        // TRIGGER PLAY STATE
        // reset cam for play state
        FindObjectOfType<CameraController>().triggerPlay = true;

        // start Play state
        Debug.Log("---------------------------- START PLAY ----------------------------");
        playTrigger = true;
        //===================================================================||  END OF LEVEL TRANSITION STATE
    }
}
