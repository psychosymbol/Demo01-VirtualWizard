using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRControllerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Vector2 UIInput;
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
        xrControl.XRIRightHand.Enable();
    }
    private void OnDisable()
    {
        xrControl.XRILeftHand.Disable();
        xrControl.XRIRightHand.Disable();
    }
    float LGrip = 0, RGrip = 0, LIndex = 0, RIndex = 0;
    private void FixedUpdate()
    {
        //movement
        moveInput = xrControl.XRILeftHand.Move.ReadValue<Vector2>();
        characterForward.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0); //this will make our character move forward to where we're looking
        //moveSpeed = (moveInput.y < 0) ? .125f : .5f;
        transform.Translate(characterForward.forward * moveInput.y * moveSpeed * Time.deltaTime);
        transform.Translate(characterForward.right * moveInput.x * moveSpeed * Time.deltaTime);
        //movement

        //UI
        UIInput = xrControl.XRIRightHand.Move.ReadValue<Vector2>();
        float UIangle = Angle(UIInput);
        //grip and trigger input
        float LTrigger = xrControl.XRILeftHand.Activate.ReadValue<float>();
        float RTrigger = xrControl.XRIRightHand.Activate.ReadValue<float>();
        float LGrip = xrControl.XRILeftHand.Select.ReadValue<float>();
        float RGrip = xrControl.XRIRightHand.Select.ReadValue<float>();


        if (UIangle >= 315 || UIangle <= 45)
        {
            //do 1
        }
        else if (UIangle > 45 && UIangle <= 135)
        {
            //do 2
        }
        else if (UIangle > 135 && UIangle <= 225)
        {
            //do 3
        }
        else if (UIangle > 225 && UIangle <= 315)
        {
            //do 4
        }

        //UI

        //animation
        if (targetAnim)
        {
            targetAnim.SetFloat("Walk", moveInput.y); //move forward or backward animation


            //hand blendshape
            targetAnim.SetFloat("LPoint", LTrigger * 100);
            targetAnim.SetFloat("RPoint", RTrigger * 100);
            targetAnim.SetFloat("LGrip", LGrip * 100);
            targetAnim.SetFloat("RGrip", RGrip * 100);
            //hand blendshape

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
    public static float Angle(Vector2 vector2)
    {
        if (vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
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
