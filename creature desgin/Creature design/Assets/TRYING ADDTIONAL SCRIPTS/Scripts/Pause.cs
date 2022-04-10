using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject pauseCanvas;
    private Button resume;
    private Button quit;

    private void Start()
    {
        pauseCanvas = transform.GetChild(1).gameObject;
        resume = pauseCanvas.transform.GetChild(0).GetComponent<Button>();
        resume.onClick.AddListener(ResumeButton);
        quit = pauseCanvas.transform.GetChild(1).GetComponent<Button>();
        quit.onClick.AddListener(QuitButton);
    }

    private void Update()
    {
        bool inspecting = GetComponentInChildren<interact>().getInspecting();


        if (Input.GetKeyDown(KeyCode.Escape) && !inspecting)
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                UnPauseGame();
            }
        }       
    }

    // sets game speed to zero and opens pause menu
    private void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        pauseCanvas.SetActive(true);
    }

    // sets game speed to 1 and closes pause menu
    private void UnPauseGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseCanvas.SetActive(false);
    }

    // when clicked the game is resumed
    private void ResumeButton()
    {
        UnPauseGame();
    }

    // closes game
    private void QuitButton()
    {
        Application.Quit();
    }

    // getter
    public bool getIsPaused()
    {
        return isPaused;
    }
}
