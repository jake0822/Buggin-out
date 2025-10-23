using UnityEditor;
using UnityEngine;
public class PauseGameManager : MonoBehaviour
{
    bool isPaused;
    public GameObject pauseMenu;
    public GameObject startMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PauseGame();
        pauseMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused) {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            UnPauseGame();
        }
    }

    public void PauseGame() {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true;
    }

    public void UnPauseGame() {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor again for gameplay
        Cursor.visible = false;
    }

    public void QuitGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // This only works in the Editor
#else
        Application.Quit(); // This works in the built game
#endif
    }

    public void StartGame() {
        UnPauseGame();
        startMenu.SetActive(false);
    }
}


