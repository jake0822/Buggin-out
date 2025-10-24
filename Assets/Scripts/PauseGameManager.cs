using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameManager : MonoBehaviour
{
    [HideInInspector] public bool canPause = false;
    bool isPaused;
    public GameObject pauseMenu;

    public GameObject startMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(false);
        startMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            UnPauseGame();
        }
    }

    public void PauseGame()
    {
        if (canPause)
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true;
        }
    }

    public void UnPauseGame()
    {
        if (canPause)
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor again for gameplay
            Cursor.visible = false;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // This only works in the Editor
#else
        Application.Quit(); // This works in the built game
#endif
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        startMenu.SetActive(false);
        canPause = true;
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor again for gameplay
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        StartGame(); //idk why this dont work
    }

    IEnumerator LoadLevel() //woudlnt work this way either
    {
        // Start async load
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Wait until done
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // The new scene is now loaded — do post-load logic
        
        StartGame();
        print("hello???");
        yield return null;
    }


}


