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
        WIN,
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

    public bool IsGameStarted()
    {
        return _state == State.GAME;
    }

    public bool PauseAvailable()
    {
        return _state != State.MENU && _state != State.WIN;
    }

    public void PauseGame(bool choice)
    {
        if (_state == State.MENU || _state == State.WIN) 
            return; //can't pause the main menu scene

        if (choice)
            _state = State.PAUSE;
        else
            _state = State.GAME;
    }

    public void WinGame()
    {
        _state = State.WIN;
    }

    public void OnClickStartGame()
    {
        SceneManager.LoadScene("GameScene");

        _state = State.GAME;
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");

        _state = State.MENU;
    }
}
