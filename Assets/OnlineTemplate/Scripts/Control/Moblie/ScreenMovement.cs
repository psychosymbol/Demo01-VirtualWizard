using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMovement : MonoBehaviour
{
    Vector2 moveInput;
    public float moveSpeed = .5f;

    //uncomment this after you import Joystick Pack
    //public FixedJoystick joystick;

    private void FixedUpdate()
    {
        //moveInput = joystick.Direction;
        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime);
        //transform.rotation = new Quaternion(transform.rotation.x, head.rotation.y, transform.rotation.z, transform.rotation.w);
    }
}
