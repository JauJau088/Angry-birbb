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

        if (waitTime > 3) {
            if (start < target) {
                start = start + (float)0.5;

                birb.GetComponent<Renderer>().enabled = false;
            } else {
                birb.GetComponent<Renderer>().enabled = true;
            }

            birb.transform.position = new Vector2(start, -2);
            aPoint.transform.position = new Vector2(start, -2);
        }
    }
}
