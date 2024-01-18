using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool PausedGame = false;
    public GameObject pauseMenu;
    public GameObject optionMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausedGame)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }


        if (PausedGame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
        Time.timeScale = 1f;
        PausedGame = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        PausedGame = true;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
