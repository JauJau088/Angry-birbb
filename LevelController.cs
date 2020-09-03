using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private Enemy[] _enemies;
    private static int _nextLevelIndex = 1;

    private void OnEnable() {
        _enemies = FindObjectsOfType<Enemy>();
    }

    private float waitTime = 0;

    void Update() {
        // loop until no enemy left
        foreach (Enemy enemy in _enemies) {
            if (enemy != null)
                return;
        }

        // if no enemy is left, wait
        Debug.Log("You killed all enemies");

        waitTime += Time.deltaTime;

        // then go to the next level
        if (waitTime > 2) {
            _nextLevelIndex++;
            string nextLevelName = "Level" + _nextLevelIndex;
            SceneManager.LoadScene(nextLevelName);
        }
    }
}
