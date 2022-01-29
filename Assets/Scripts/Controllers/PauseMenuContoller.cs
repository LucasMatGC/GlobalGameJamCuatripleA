using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class PauseMenuContoller : MonoBehaviour
{

    private bool pausedGame;
    public Canvas menuPause;
    
    private GameController gameController;
    private string Menu = "MainMenu", CurrentLevel = "TestLevelLucas";
    private bool mouseUser = false;
    private Vector3 mouseInitPos;

    // Start is called before the first frame update
    void Start()
    {

        gameController = GameController.instance;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("MenuButton"))
        {

            PauseMenu();

            
        }

        if (pausedGame)
        {
            
            if (!mouseUser && Input.mousePosition != mouseInitPos) mouseUser = true;
        }

        else EventSystem.current.SetSelectedGameObject(null);

        if (mouseUser) Cursor.visible = true;
        else Cursor.visible = false;
        
    }

    private void PauseMenu()
    {

        if(!gameController)
            gameController = GameController.instance;

        pausedGame = !pausedGame;
        menuPause.enabled = pausedGame;
        Time.timeScale = (pausedGame) ? 0 : 1f;
        if (pausedGame)
        {
            gameController.getCurrentMusic().Pause();
            mouseInitPos = Input.mousePosition;
        }
        else
        {
            if (gameController.IsGameRunning())
            gameController.getCurrentMusic().Play();
            mouseUser = false;
        }

    }

    public void Resume()
    {

        pausedGame = false;
        menuPause.enabled = false;
        Time.timeScale = 1f;
        if (gameController.IsGameRunning())
            gameController.getCurrentMusic().Play();
        mouseUser = false;

    }

    public void Restart()
    {

        menuPause.enabled = false;
        mouseUser = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(CurrentLevel);

    }

    public void LoadMenu()
    {
        mouseUser = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(Menu);

    }

    public void QuitGame()
    {

        Application.Quit();

    }
}
