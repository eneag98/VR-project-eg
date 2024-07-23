using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public GameObject menu;
    public Animator animator;
    public List<GameObject> objectsToBePaused;

    bool _isTimerRunning;
    float _timer;
    GameManager _gameManager;
    GUIStyle _headStyle;

    // Start is called before the first frame update
    void Awake()
    {
        _gameManager = GameManager.Instance;
        _headStyle = new GUIStyle();
        _headStyle.fontSize = 30;

        _isTimerRunning = true;
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool escPressed = Input.GetKeyDown(KeyCode.Escape);
        if(_gameManager != null)
            if (_gameManager.pauseAvailable() && escPressed)
            {
                Debug.Log("Esc key pressed!");
                menu.SetActive(!menu.activeSelf);
                pauseGame(menu.activeSelf);
            }

        if(_isTimerRunning)
            _timer += Time.deltaTime;
    }

    void OnGUI()
    {
        int minutes = Mathf.FloorToInt(_timer / 60F);
        int seconds = Mathf.FloorToInt(_timer - minutes * 60);
        Vector3 pos = animator.gameObject.transform.position;

        string position2D = "(" + (int)pos.x + ", " + (int)pos.z + ")";
        string time = string.Format("{0:00}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(10, 10, 250, 100), "Elapsed time: "+time, _headStyle);
        GUI.Label(new Rect(10, 50, 250, 100), "Coordinates: "+position2D, _headStyle);
    }

    public void pauseGame(bool choice)
    {
        _gameManager.pauseGame(choice);
        _isTimerRunning = !choice;

        if (animator != null)
            animator.enabled = !choice;

        /*foreach(GameObject o in objectsToBePaused)
        {
            o.SetActive(!choice);
        }*/

    }

    public void OnClickStartGame()
    {
        _gameManager.OnClickStartGame();
    }

    public void OnClickMainMenu()
    {
        _gameManager.OnClickMainMenu();
    }
    public void OnClickRestart()
    {
        _gameManager.OnClickRestartGame();
    }
    public void OnClickExitGame()
    {
        _gameManager.OnClickExitGame();
    }
}
