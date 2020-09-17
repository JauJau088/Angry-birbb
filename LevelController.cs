﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    GameObject birdObj;
    Bird birdScript;
    private Enemy[] enemies;
    private static int levelIndex = 1;
    private string levelName, tmpName;
    private float waitTime = 0, start = 0, target = 56;

    bool playTrigger = true, loadTrigger = false;

    private void Awake() {
        SceneManager.LoadScene("Level1");
    }

    private void Start() {
        birdObj = GameObject.Find("GreenB");
        birdScript = birdObj.GetComponent<Bird>();
        enemies = FindObjectsOfType<Enemy>();
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
            if (birdScript.birdWasLaunched && birdObj.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1) {
                waitTime += Time.deltaTime;
            }

            // if out of screen or stay still few sec
            if (birdObj.transform.position.x > 15 || birdObj.transform.position.x < -15 ||
                birdObj.transform.position.y > 15 || birdObj.transform.position.y < -15 ||
                waitTime > 3) {
                // then reset
                birdObj.transform.position = birdScript.initialPosition;
                birdObj.transform.rotation = Quaternion.identity;
                birdScript.birdWasLaunched = false;
                waitTime = 0;
                birdObj.GetComponent<Rigidbody2D>().gravityScale = 0;
                birdObj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                birdObj.GetComponent<Rigidbody2D>().angularVelocity = 0;
            }

            foreach (Enemy enemy in enemies) {
                if (enemy != null) {
                    return;
                }
            }

            // if this part is reached, then that means:
            Debug.Log("You killed all enemies");

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

        // if next level is found
        if (Application.CanStreamedLevelBeLoaded(levelName)) {
            Debug.Log("==================== New level can be loaded");

            // slower the cam
            FindObjectOfType<CameraController>().lerpFactor = 0.05f;

            // directs cam to the next loc (TO DO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!)
            // change init pos in global var
            FindObjectOfType<GlobalVar>().birdInitPos = new Vector2(target, -2);
            FindObjectOfType<GlobalVar>().pointInitPos = new Vector2(target, -2);
            // end TO DO
            
            // wait for few more until cam reaches new pos then load next level
            /*if (waitTime > 6) {
                Debug.Log("==================== Init loading new Level");
                if (SceneManager.GetSceneByName(levelName).isLoaded == false) {
                    SceneManager.LoadScene(levelName);
                    Debug.Log("==================== Load new Level");
                }
            }*/

            Debug.Log("==================== Init loading new Level");
            if (SceneManager.GetSceneByName(levelName).isLoaded == false) {
                SceneManager.LoadScene(levelName);
                Debug.Log("==================== Load new Level");
            }
        }
        // if not found, back to Level1
        else {
            Debug.Log("==================== Init loading Level1");

            if (SceneManager.GetSceneByName("Level1").isLoaded == false) {
                SceneManager.LoadScene("Level1");
                Debug.Log("==================== Load Level1");
                levelIndex = 1;
            }
        }

        playTrigger = false;
        Debug.Log("---------------------------- QUIT PLAY ----------------------------");

        yield return new WaitForSeconds(3f);

        // re-init
        Debug.Log("---------------------------- re-init ----------------------------");

        birdObj = GameObject.Find("GreenB");
        birdScript = birdObj.GetComponent<Bird>();
        enemies = FindObjectsOfType<Enemy>();
        playTrigger = true;

        Debug.Log("---------------------------- START PLAY ----------------------------");
    }

    IEnumerator loadTriggerReset() {
        yield return new WaitForSeconds(5f);

        loadTrigger = true;
    }
}
