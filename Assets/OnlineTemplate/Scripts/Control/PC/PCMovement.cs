using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMovement : MonoBehaviour
{
    private PCControl pcControl;
    [SerializeField] private Animator animator;

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
        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, HeadCamera.rotation.eulerAngles.y, 0);

        animator.SetFloat("XAxis", moveInput.x);
        animator.SetFloat("YAxis", moveInput.y);
    }

    private void Run(bool isRun)
    {
        animator.SetBool("Run", isRun);
        moveSpeed = isRun ? 1.5f : .5f;
    }
}
