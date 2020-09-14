using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private Enemy[] enemies;
    private static int nextLevelIndex = 1;

    private float waitTime = 0, start = 0, target = 56;
    GameObject birb, aPoint;

    private void Awake() {
        enemies = FindObjectsOfType<Enemy>();

        birb = GameObject.Find("GreenB");
        aPoint = GameObject.Find("aPoint");
    }    

    private void Update() {
        // loop until no enemy left
        foreach (Enemy enemy in enemies) {
            if (enemy != null)
                return;
        }

        // if no enemy is left, wait
        Debug.Log("You killed all enemies");
        waitTime += Time.deltaTime;

        // then go to the next level
        /*if (waitTime > 3) {
            nextLevelIndex++;
            string nextLevelName = "Level" + nextLevelIndex;

            if (Application.CanStreamedLevelBeLoaded(nextLevelName)) {
                SceneManager.LoadScene(nextLevelName);
            }
            else {
                SceneManager.LoadScene("Level1");
                nextLevelIndex = 1;
            }
        }*/

        /*
        if (waitTime > 3) {
            if (start < target) {
                start = start + (float)0.5;

                birb.GetComponent<Renderer>().enabled = false;
            } else {
                birb.GetComponent<Renderer>().enabled = true;
            }

            birb.transform.position = new Vector2(start, -2);
            aPoint.transform.position = new Vector2(start, -2);
        }*/

        if (waitTime > 3) {
            // slower the cam
            GameObject.FindObjectOfType<CameraController>().lerpFactor = 0.05f;

            // change init pos in global var
            GameObject.FindObjectOfType<GlobalVar>().birdInitPos = new Vector2(target, -2);
            GameObject.FindObjectOfType<GlobalVar>().pointInitPos = new Vector2(target, -2);

            // reset this script
            Destroy(gameObject.GetComponent<Bird>());
            gameObject.AddComponent<Bird>();
            
            // change gameobjects pos and hide them
            // (the objects are different from the script, hence need to change them as well separately)
            birb.GetComponent<Renderer>().enabled = false;
            birb.transform.position = GameObject.FindObjectOfType<GlobalVar>().birdInitPos;
            aPoint.transform.position = GameObject.FindObjectOfType<GlobalVar>().pointInitPos;

            // upon arrival to the new location
            if (waitTime > 6) {
                // return to original
                GameObject.FindObjectOfType<CameraController>().lerpFactor = 0.125f;
                birb.GetComponent<Renderer>().enabled = true;
            }
        }
    }
}
