using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PCControl pcControl;

    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cameraTransform;
    public LayerMask collisionLayer;
    private float defaultPosition;
    private Vector3 cameFollowVeclocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffSet = 0.2f;
    public float cameraCollisionRadius = 2;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle;
    public float pivotAngle;
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;

    public float mouseSensitive = 1f;

    private float mouseX;
    private float mouseY;

    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;

    private bool isEnableCemaraMove = false;

    public Vector2 mouseDelta;


    private void Awake()
    {
        pcControl = new PCControl();
    }

    private void Start()
    {
        defaultPosition = cameraTransform.localPosition.z;
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
        //mouseX = Input.GetAxis("Mouse X") * mouseSensitive * Time.deltaTime;
        //mouseY = Input.GetAxis("Mouse Y") * mouseSensitive * Time.deltaTime;

        pcControl.Player.RightMouse.performed += _ => EnableCameraMovement();
        pcControl.Player.RightMouse.canceled += _ => DisableCameraMovement();

        if (isEnableCemaraMove)
        {
            HandleMouseInput();
        }
    }

    private void LateUpdate()
    {
        if (isEnableCemaraMove)
        {
            HandleAllCameraMovement();
        }
    }

    private void EnableCameraMovement()
    {
        isEnableCemaraMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void DisableCameraMovement()
    {
        isEnableCemaraMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HandleMouseInput()
    {
        mouseDelta = pcControl.Player.Look.ReadValue<Vector2>();

        mouseX = mouseDelta.x * mouseSensitive * Time.deltaTime;
        mouseY = mouseDelta.y * mouseSensitive * Time.deltaTime;
    }


    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameFollowVeclocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotaion;

        lookAngle = lookAngle + (mouseX * cameraLookSpeed);
        pivotAngle = pivotAngle - (mouseY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotaion = Quaternion.Euler(rotation);
        transform.rotation = targetRotaion;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotaion = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotaion;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayer))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffSet);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffSet)
        {
            targetPosition = targetPosition - minimumCollisionOffSet;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
