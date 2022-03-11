using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadMenu();
            Debug.Log("M key is pressed");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC is pressed");
            Application.Quit();
        }
    }



    public void LoadMenu()
    {
        SceneManager.LoadScene(0); // Load menu
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1); // game scene
    }

    public void HelpMenu()
    {
        SceneManager.LoadScene(2); // Help scene
    }


    
}
