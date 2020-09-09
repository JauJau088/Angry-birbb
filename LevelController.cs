using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private Enemy[] _enemies;
    private static int nextLevelIndex = 1;

    private void OnEnable() {
        _enemies = FindObjectsOfType<Enemy>();
    }

    private float waitTime = 0;

    private void Update() {
        // loop until no enemy left
        foreach (Enemy enemy in _enemies) {
            if (enemy != null)
                return;
        }

        // if no enemy is left, wait
        Debug.Log("You killed all enemies");
        waitTime += Time.deltaTime;

        // then go to the next level
        if (waitTime > 3) {
            nextLevelIndex++;
            string nextLevelName = "Level" + nextLevelIndex;

            if (Application.CanStreamedLevelBeLoaded(nextLevelName)) {
                SceneManager.LoadScene(nextLevelName);
            }
            else {
                SceneManager.LoadScene("Level1");
                nextLevelIndex = 1;
            }
        }
    }
}
