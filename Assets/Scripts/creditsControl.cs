using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creditsControl : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
