using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public GameObject menu;
    public GameObject winUI;
    public TextMeshProUGUI scoreText;
    public PlayerMovement player;
    public CameraController cameraController;
    public GameTarget gameTarget;
    public Terrain gameArea;

    bool _isTimerRunning;
    float _timer;
    int _correctionFactor = 3;
    GameManager _gameManager;
    GUIStyle _headStyle;
    Animator _playerAnimator;

    Vector3 _initTargetPosition;
    Vector3 _initPlayerPosition;
    Quaternion _initPlayerRotation;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        _gameManager = GameManager.Instance;
        _headStyle = new GUIStyle();
        _headStyle.fontSize = 30;
        if(player)
            _playerAnimator = player.GetComponent<Animator>();

        _isTimerRunning = false;
        _timer = 0f;
    }

    void Start()
    {
        if (_gameManager.IsGameStarted()) {
            Random.InitState((int)Time.time);
            Vector3 areaSize = gameArea.terrainData.size;

            int playerQuarter = Random.Range(0, 4);
            player.transform.position = GetQuarterCenter(playerQuarter);
            Vector3 lookDirection = new Vector3(areaSize.x / 2, 0, areaSize.z / 2) - player.transform.position;
            player.transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            cameraController.transform.position = player.transform.position;
            cameraController.transform.rotation = player.transform.rotation;

            gameTarget.transform.position = GetRandomExcludingQuarter(playerQuarter);
            gameTarget.transform.position = new Vector3(gameTarget.transform.position.x,
                                        gameArea.SampleHeight(gameTarget.transform.position) + 10f,
                                        gameTarget.transform.position.z);

            _initTargetPosition = gameTarget.transform.position;
            _initPlayerPosition = player.transform.position;
            _initPlayerRotation = player.transform.rotation;

            gameTarget.enabled = true;
            _isTimerRunning = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool escPressed = Input.GetKeyDown(KeyCode.Escape);
        if(_gameManager != null)
            if (_gameManager.PauseAvailable() && escPressed)
            {
                Debug.Log("Esc key pressed!");
                menu.SetActive(!menu.activeSelf);
                PauseGame(menu.activeSelf);
            }

        if(_isTimerRunning)
            _timer += Time.deltaTime;
    }

    void OnGUI()
    {
        if (!_isTimerRunning)
            return;

        Vector3 pos = player.transform.position;

        string position2D = "(" + (int)pos.x + ", " + (int)pos.z + ")";

        GUI.Label(new Rect(10, 10, 250, 100), "Elapsed time: " + GetTimestamp(_timer), _headStyle);
        GUI.Label(new Rect(10, 50, 250, 100), "Coordinates: " + position2D, _headStyle);
    }

    private static string GetTimestamp(float timer)
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public Vector3 GetQuarterCenter(int quarter)
    {
        Vector3 center = Vector3.zero;
        switch (quarter)
        {
            case 0:
                center = new Vector3(gameArea.terrainData.size.x / 4, 0, gameArea.terrainData.size.z / 4);
                break;
            case 1:
                center = new Vector3(gameArea.terrainData.size.x / 4, 0, gameArea.terrainData.size.z * 3 / 4);
                break;
            case 2:
                center = new Vector3(gameArea.terrainData.size.x * 3 / 4, 0, gameArea.terrainData.size.z * 3 / 4);
                break;
            case 3:
                center = new Vector3(gameArea.terrainData.size.x * 3 / 4, 0, gameArea.terrainData.size.z / 4);
                break;
        }

        return center;
    }

    public Vector3 GetRandomExcludingQuarter(int quarter)
    {
        Vector3 position;
        Vector2 bottom_left_corner = Vector2.zero;
        Vector2 top_right_corner = Vector2.zero;
        float x = gameArea.terrainData.size.x;
        float z = gameArea.terrainData.size.z;

        switch (quarter)
        {
            case 0:
                bottom_left_corner = new Vector2(0, 0);
                top_right_corner = new Vector2(x / 2, z / 2);
                break;
            case 1:
                bottom_left_corner = new Vector2(0, z / 2);
                top_right_corner = new Vector2(x / 2, z);
                break;
            case 2:
                bottom_left_corner = new Vector2(x / 2, z / 2);
                top_right_corner = new Vector2(x, z);
                break;
            case 3:
                bottom_left_corner = new Vector2(x / 2, 0);
                top_right_corner = new Vector2(x, z / 2);
                break;
        }

        do
        {
            position = new Vector3(Random.Range(_correctionFactor, x - _correctionFactor), 0, Random.Range(_correctionFactor, z - _correctionFactor));
        } while (PositionInArea(position, bottom_left_corner, top_right_corner));

        return position;
    }

    public bool PositionInArea(Vector3 pos, Vector2 bottom_left, Vector2 top_right)
    {
        bool verticalCheck = pos.z >= bottom_left.y && pos.z <= top_right.y;
        bool horizontalCheck = pos.x >= bottom_left.x && pos.x <= top_right.x;

        return verticalCheck && horizontalCheck;
    }

    public void PauseGame(bool choice)
    {
        _gameManager.PauseGame(choice);
        _isTimerRunning = !choice;

        if (_playerAnimator != null)
            _playerAnimator = player.gameObject.GetComponent<Animator>();

        cameraController.enabled = !choice;
        gameTarget.enabled = !choice;
        _playerAnimator.enabled = !choice;
        player.enabled = !choice;
    }

    public void WinGame()
    {
        _gameManager.WinGame();
        PauseGame(true);
        scoreText.SetText(GetTimestamp(_timer));
        winUI.SetActive(true);
    }

    public void OnClickStartGame()
    {
        _gameManager.OnClickStartGame();
        enabled = false;
    }

    public void OnClickMainMenu()
    {
        _gameManager.OnClickMainMenu();
        enabled = false;
    }
    public void OnClickRestart()
    {
        player.transform.position = _initPlayerPosition;
        player.transform.rotation = _initPlayerRotation;
        cameraController.transform.position = _initPlayerPosition;
        cameraController.transform.rotation = _initPlayerRotation;
        gameTarget.transform.position = _initTargetPosition;

        menu.SetActive(false);

        _timer = 0f;
        PauseGame(false);

    }
    public void OnClickExitGame()
    {
        _gameManager.OnClickExitGame();
        enabled = false;
    }
}
