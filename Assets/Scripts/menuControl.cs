using UnityEngine;
using UnityEngine.SceneManagement;

public class menuControl : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
