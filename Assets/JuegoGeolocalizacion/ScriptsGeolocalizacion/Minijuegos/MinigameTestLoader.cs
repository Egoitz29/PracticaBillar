using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameTestLoader : MonoBehaviour
{
    public string minigameSceneName = "MiniRompeCubo2D";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Ha entrado");
            SceneManager.LoadScene(minigameSceneName);
        }
    }
}
