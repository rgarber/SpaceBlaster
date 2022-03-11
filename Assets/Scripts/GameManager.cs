using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(0);
            
        }

        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true) // if game over then press R to restart
        {
            SceneManager.LoadScene(1); //reload current level
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC is pressed");
            _isGameOver = true;
            Application.Quit();
        }
 
    }
    public void GameOver()
    {
        _isGameOver = true;
    }

}
