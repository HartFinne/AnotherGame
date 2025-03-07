using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void LoadStartScreenScene()
    {
        SceneManager.LoadScene("Start Screen");
    }

    public void QuitGame()
    {
        Debug.Log("Game is quitting..."); // This helps verify in the Unity Editor
        Application.Quit(); // Quits the game (only works in a built application)
    }
}
