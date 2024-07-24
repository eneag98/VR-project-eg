using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float runSpeed;
    public float rotationSpeed;
    public Terrain playableTerrain;

    [Header("Invisible Walls Detection")]
    [SerializeField] LayerMask wallsLayer;
    public float deceleration = 0.2f;

    Camera _camera;
    Animator _animator;
    CapsuleCollider _collider;
    bool _wallCollisionCheck;

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindFirstObjectByType<Camera>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
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

        Vector3 diff = transform.position - _camera.transform.position;
        Vector3 forward = Vector3.ProjectOnPlane(diff, Vector3.up).normalized;
        Vector3 right = _camera.transform.right.normalized;

        Vector3 fixedForward = verticalInput * forward;
        Vector3 fixedRight = horizontalInput * right;

        Vector3 cameraRelativeMovement = fixedForward + fixedRight;
        cameraRelativeMovement.Normalize();

        if (cameraRelativeMovement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(cameraRelativeMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        float speed;
        _wallCollisionCheck = Physics.Raycast(transform.position, fixedForward, _collider.radius, wallsLayer);
        if(!_wallCollisionCheck)
            _wallCollisionCheck = Physics.Raycast(transform.position, fixedRight, _collider.radius, wallsLayer);

        if (_wallCollisionCheck)
            speed = 0f;
        else
            speed = _animator.GetBool("isRunning") ? runSpeed : moveSpeed;


        transform.Translate(cameraRelativeMovement * speed * Time.deltaTime, Space.World);

        transform.position = new Vector3(transform.position.x,
                                         playableTerrain.SampleHeight(transform.position) - 0.01f,
                                         transform.position.z);
    }
}
