using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 constDistance;
    public float scrollSpeed;
    public float rotationSpeed;

    Camera m_Camera;

    void Start()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;

        if (m_Camera.orthographic)
            m_Camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        else
            m_Camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

        bool dx = Input.GetKey(KeyCode.Mouse1);
        if (dx)
            CamOrbit();
    }

    public void CamOrbit()
    {
        float verticalInput = Input.GetAxis("Mouse Y");
        float horizontalInput = Input.GetAxis("Mouse X");

        if( verticalInput != 0 || horizontalInput != 0 )
        {
            verticalInput *= rotationSpeed * Time.deltaTime; 
            horizontalInput *= rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.left, verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }
    }
}
