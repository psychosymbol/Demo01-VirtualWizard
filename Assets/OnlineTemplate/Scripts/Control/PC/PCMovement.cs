using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMovement : MonoBehaviour
{
    //private PCControl pcControl;
    Vector2 moveInput;
    public float moveSpeed = .5f;
    private void Awake()
    {
        //pcControl = new PCControl();
    }

    private void OnEnable()
    {
        //pcControl.Player.Enable();
    }
    private void OnDisable()
    {
        //pcControl.Player.Disable();
    }

    private void FixedUpdate()
    {
        //moveInput = pcControl.Player.Move.ReadValue<Vector2>();
        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime);
        //transform.rotation = new Quaternion(transform.rotation.x, head.rotation.y, transform.rotation.z, transform.rotation.w);
    }


}
