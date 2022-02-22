using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMovement : MonoBehaviour
{
    private PCControl pcControl;
    public Animator targetAnim;

    Vector2 moveInput;

    public float moveSpeed = .5f;
    public Transform HeadCamera;

    private int runMultiplier = 1;
    private void Awake()
    {
        pcControl = new PCControl();
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
}
