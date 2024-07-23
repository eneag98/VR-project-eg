using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        MENU,
        GAME,
        PAUSE
    }

    public static GameManager Instance;
    public State _state;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("MainMenuScene");
        _state = State.MENU;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public bool pauseAvailable()
    {
        return _state != State.MENU;
    }

    public void pauseGame(bool choice)
    {
        if (_state == State.MENU) 
            return; //can't pause the main menu scene

        if (choice)
            _state = State.PAUSE;
        else
            _state = State.GAME;
    }
    

    public void OnClickStartGame()
    {
        Debug.Log("Start Game button pressed!");
        SceneManager.LoadScene("GameScene");

        _state = State.GAME;
    }

    public void OnClickExitGame()
    {
        Debug.Log("Exit button pressed!");
        Application.Quit();
    }

    public void OnClickMainMenu()
    {
        Debug.Log("Main Menu button pressed!");
        SceneManager.LoadScene("MainMenuScene");

        _state = State.MENU;
    }

    public void OnClickRestartGame()
    {
        Debug.Log("Restart button pressed!");
        return;
    }
}
