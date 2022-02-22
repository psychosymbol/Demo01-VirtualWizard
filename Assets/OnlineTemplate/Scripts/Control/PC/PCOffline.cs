using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCOffline : MonoBehaviour
{
    private PCControl pcControl;
    public Animator targetAnim;

    Vector2 moveInput;

    public float moveSpeed = .5f;
    public Transform HeadCamera;

    private int runMultiplier = 1;

    private bool isJump = false;
    private bool isGrounded = true;

    private float jumpTime = 0f;
    private float jumpDelay = 1f;
    private void Awake()
    {
        pcControl = new PCControl();
        HeadCamera.parent = null;
    }

    private void OnEnable()
    {
        pcControl.Player.Enable();
    }
    private void OnDisable()
    {
        pcControl.Player.Disable();
    }

    private void Update()
    {
        pcControl.Player.Run.performed += _ => Run(true);
        pcControl.Player.Run.canceled += _ => Run(false);
        pcControl.Player.Jump.performed += _ => Jump();
        HeadCamera.position = transform.position;

        DelayJump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Tag {collision.transform.tag}");
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
            isJump = false;
            targetAnim.SetBool("Jump", isJump);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log($"Tag {collision.transform.tag}");
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
            isJump = false;
            targetAnim.SetBool("Jump", isJump);
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    public void HandleMovement()
    {
        moveInput = pcControl.Player.Move.ReadValue<Vector2>();
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, HeadCamera.rotation.eulerAngles.y, 0);
        }
        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime);

        targetAnim.SetFloat("XAxis", moveInput.x);
        targetAnim.SetFloat("YAxis", moveInput.y);
    }

    private void Run(bool isRun)
    {
        targetAnim.SetBool("Run", isRun);
        moveSpeed = isRun ? 1.5f : .5f;
    }

    private void Jump()
    {
        Debug.Log($"Jump: {isJump} | Grounded: {isGrounded}");
        if(!isJump && isGrounded && jumpTime > jumpDelay)
        {
            Debug.LogWarning("JUMP!!!");
            isJump = true;
            isGrounded = false;
            targetAnim.SetBool("Jump", isJump);
        }
    }

    private void DelayJump()
    {
        if (!isJump)
        { jumpTime += 1 * Time.deltaTime; }
    }
}
