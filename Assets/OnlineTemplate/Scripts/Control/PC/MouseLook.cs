using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //private PCControl pcControl;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;
    public Transform body;
    Quaternion bodyRotation;

    public Vector2 mouseDelta;
    private void Awake()
    {
        //pcControl = new PCControl();
    }
    void Start()
    {
        originalRotation = transform.localRotation;
        bodyRotation = body.localRotation;
    }

    private void OnEnable()
    {
        //pcControl.Player.Enable();
    }
    private void OnDisable()
    {
        //pcControl.Player.Disable();
    }

    void Update()
    {
        //mouseDelta = pcControl.Player.Look.ReadValue<Vector2>();
        // Read the mouse input axis
        //rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        //rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX += mouseDelta.x * sensitivityX;
        rotationY += mouseDelta.y * sensitivityY;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
        transform.localRotation = originalRotation * yQuaternion;
        body.localRotation = bodyRotation * xQuaternion;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
         angle += 360F;
        if (angle > 360F)
         angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
