using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    GameObject birdObj;
    Bird birdScript;
    private Enemy[] enemies;
    private static int nextLevelIndex = 1;
    private float waitTime = 0, start = 0, target = 56;

    private void Update() {
        enemies = FindObjectsOfType<Enemy>();
        birdObj = GameObject.Find("GreenB");
        birdScript = birdObj.GetComponent<Bird>();

        // loop until no enemy left
        foreach (Enemy enemy in enemies) {
            if (enemy != null) {
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

                return;
            }
        }

        // if no enemy is left do all this
        Debug.Log("You killed all enemies");
        waitTime += Time.deltaTime;

        if (waitTime > 3) {
            // prepare for next level
            nextLevelIndex++;
            string nextLevelName = "Level" + nextLevelIndex;

            // if next level is found
            if (Application.CanStreamedLevelBeLoaded(nextLevelName)) {
                // slower the cam
                FindObjectOfType<CameraController>().lerpFactor = 0.05f;

                // directs cam to the next loc (TO DO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!)
                // change init pos in global var
                FindObjectOfType<GlobalVar>().birdInitPos = new Vector2(target, -2);
                FindObjectOfType<GlobalVar>().pointInitPos = new Vector2(target, -2);
                // end TO DO
                
                // wait for few more until cam reaches new pos then load next level
                if (waitTime > 6) {
                    //SceneManager.LoadScene("Main");
                    SceneManager.LoadScene(nextLevelName, LoadSceneMode.Additive);
                }
            }
            // if not found, back to Level1
            else {
                //    Debug.Log("init");
                
                //    Debug.Log("after loading Main");
                SceneManager.LoadScene("Level1");
                    Debug.Log("after loading Level1");
                nextLevelIndex = 1;
            }
        }
    }
}
