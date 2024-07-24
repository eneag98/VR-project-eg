using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 720f;

    Animator animator;
    int isWalkingHash;
    int isWalkingBackwardHash;
    int isRunningHash;
    int isTurningRightHash;
    int isTurningLeftHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isWalkingBackwardHash = Animator.StringToHash("isWalkingBackward");
        isRunningHash = Animator.StringToHash("isRunning");
        isTurningRightHash = Animator.StringToHash("isTurningRight");
        isTurningLeftHash = Animator.StringToHash("isTurningLeft");
    }

    // Update is called once per frame
    void Update()
    {
        bool wp = Input.GetKey("w");
        bool ap = Input.GetKey("a");
        bool sp = Input.GetKey("s");
        bool dp = Input.GetKey("d");
        bool rp = Input.GetKey("left shift");

        handleMovement(wp, sp, ap, dp, rp);
        handleRotation(wp, sp, ap, dp);
    }

    public void handleMovement(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed)
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isWalkingBackward = animator.GetBool(isWalkingBackwardHash);
        bool isTurningLeft = animator.GetBool(isTurningLeftHash);
        bool isTurningRight = animator.GetBool(isTurningRightHash);

        if(forwardPressed && backwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isWalkingBackwardHash, false);
            return;
        }

        if (!forwardPressed && !backwardPressed)
        {
            if (!isTurningLeft && leftPressed)
                animator.SetBool(isTurningLeftHash, true);

            if (isTurningLeft && !leftPressed)
                animator.SetBool(isTurningLeftHash, false);

            if (!isTurningRight && rightPressed)
                animator.SetBool(isTurningRightHash, true);

            if (isTurningRight && !rightPressed)
                animator.SetBool(isTurningRightHash, false);
        }
            
        
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isWalkingBackward && backwardPressed)
        {
            animator.SetBool(isWalkingBackwardHash, true);
        }

        if (isWalkingBackward && !backwardPressed)
        {
            animator.SetBool(isWalkingBackwardHash, false);
        }

        bool isMoving = forwardPressed || backwardPressed;

        if (!isRunning && (isMoving && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }

        if (isRunning && (!isMoving || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
        
    }

    public void handleRotation(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed)
    {
        if (rightPressed && leftPressed)
            return;

        float forward = forwardPressed ? 1f : 0f;
        float right = rightPressed ? 1f : 0f;
        float left = leftPressed ? -1f : 0f;

        Vector3 movementDirection = new Vector3( right != 0 ? right : left, 0f, forward);
        movementDirection.Normalize();
        Debug.Log(movementDirection);

        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World );

        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);  
        }

    }
}
