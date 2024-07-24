using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 constDistance;
    public float minFOV = 55f;
    public float maxFOV = 90f;
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
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");

        /*if (m_Camera.orthographic)
            m_Camera.orthographicSize -= scrollAmount * scrollSpeed;
        else*/
        if(scrollAmount != 0f)
        {
            m_Camera.fieldOfView -= scrollAmount * scrollSpeed;

            if(m_Camera.fieldOfView < minFOV)
                m_Camera.fieldOfView = minFOV;

            if(m_Camera.fieldOfView > maxFOV)
                m_Camera.fieldOfView = maxFOV;
        }
            

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
