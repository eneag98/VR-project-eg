using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float runSpeed;
    public float rotationSpeed;
    public Camera camera;
    public Terrain playableTerrain;

    Animator _animator;
    Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool runningPressed = Input.GetKey("left shift");

        bool isWalking = horizontalInput != 0f || verticalInput != 0f;
        _animator.SetBool("isWalking", isWalking);
        _animator.SetBool("isRunning", runningPressed && isWalking);

        if (!isWalking)
        {
            _rigidBody.useGravity = true;
            return;
        } else
            _rigidBody.useGravity = false;


        Vector3 diff = transform.position - camera.transform.position;
        Vector3 forward = Vector3.ProjectOnPlane(diff, Vector3.up);
        Vector3 right = camera.transform.right;

        Vector3 fixedForward = verticalInput * forward;
        Vector3 fixedRight = horizontalInput * right;
        Vector3 cameraRelativeMovement = fixedForward + fixedRight;
        cameraRelativeMovement.Normalize();

        if (cameraRelativeMovement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(cameraRelativeMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        float speed = _animator.GetBool("isRunning") ? runSpeed : moveSpeed;

        transform.Translate(cameraRelativeMovement * speed * Time.deltaTime, Space.World);

        transform.position = new Vector3(transform.position.x,
                                         playableTerrain.SampleHeight(transform.position) - 0.1f,
                                         transform.position.z);
    }
}
