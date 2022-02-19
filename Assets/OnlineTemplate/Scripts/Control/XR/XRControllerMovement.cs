using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRControllerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Vector2 rotInput;
    public float moveSpeed = .5f;

    private XRIDefaultInputActions xrControl;
    public Transform head;
    public Transform headPosition;
    public Transform body;
    public Transform characterForward;
    public Animator targetAnim;
    Quaternion originalRotation;

    public float sensitivityX = 3F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    float rotationX = 0F;

    public SkinnedMeshRenderer bodyblend;
    private void Awake()
    {
        xrControl = new XRIDefaultInputActions();
        sensitivityX = 3F;
        //originalRotation = body.transform.localRotation;
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        xrControl.XRILeftHand.Enable();
    }
    private void OnDisable()
    {
        xrControl.XRILeftHand.Disable();
    }
    float LGrip = 0, RGrip = 0, LIndex = 0, RIndex = 0;
    private void FixedUpdate()
    {
        //movement
        moveInput = xrControl.XRILeftHand.Move.ReadValue<Vector2>();
        characterForward.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0); //this will make our character move forward to where we're looking
        transform.Translate(characterForward.forward * moveInput.y * moveSpeed * Time.deltaTime);
        transform.Translate(characterForward.right * moveInput.x * moveSpeed * Time.deltaTime);
        //movement

        //animation
        if (targetAnim)
        {
            targetAnim.SetFloat("Walk", moveInput.y); //move forward or backward animation
        }
        //animation

        //fix head position
        head.position = headPosition.position;
        body.position = headPosition.position;
        //this.GetComponent<XRRig>().cameraYOffset = headPosition.position.y - transform.position.y;

        //rotate via controller (unused)
        //rotInput = xrControl.XRIRightHand.Move.ReadValue<Vector2>();
        //rotationX += rotInput.x * sensitivityX;
        //rotationX = ClampAngle(rotationX, minimumX, maximumX);
        //Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        //body.transform.localRotation = originalRotation * xQuaternion;
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
