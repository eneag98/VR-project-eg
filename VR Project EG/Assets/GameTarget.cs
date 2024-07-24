using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTarget : MonoBehaviour
{
    public float floatingSpeed = 1f;

    LevelManager _levelManager;
    Vector3 _lowerPos;
    Vector3 _upperPos;

    bool _isMoving;

    private void Start()
    {
        _levelManager = LevelManager.Instance;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        _isMoving = true;
        _lowerPos = transform.position + new Vector3(0f, -2f, 0f);
        _upperPos = transform.position + new Vector3(0f, 2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
            transform.position = Vector3.Lerp(_lowerPos, _upperPos, Mathf.PingPong(Time.time / floatingSpeed, 1));
    }

    void OnTriggerEnter(Collider other)
    {
        _isMoving = false;
        Debug.Log("WIN!");
        _levelManager.WinGame();
        gameObject.SetActive(false);
    }
}
