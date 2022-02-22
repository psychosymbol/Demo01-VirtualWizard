using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    private PCControl pcControl;

    [SerializeField] private CinemachineVirtualCamera followVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform aimPivot;

    [SerializeField] private LayerMask aimLayerMask = new LayerMask();
    //[SerializeField] private Transform aimTransform;

    [SerializeField] private Image crosshair;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform shootingPosition;

    public CameraController cameraController;
    public PCMovement pcMovement;

    private bool isAim = false;
    private bool isFire = false;
    private float currentTime = 0f;
    private float delayFire = 1f;

    private void Awake()
    {
        pcControl = new PCControl();

    }
    private void Start()
    {
        crosshair.gameObject.SetActive(false);
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

        pcControl.Player.Aim.performed += _ => IsAim(true);
        pcControl.Player.Aim.canceled += _ => IsAim(false);


        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimLayerMask))
        {
            //aimTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        Debug.LogWarning($"Is aim: {isAim}");

        if (isAim)
        {
            EnableAimCamera();
            Aim(mouseWorldPosition);
        }
        else
        {
            DisableAimCamera();
        }

        pcControl.Player.Fire.performed += _ => Shoot(mouseWorldPosition);

        DelayFire();

    }

    public void EnableAimCamera()
    {
        aimVirtualCamera.gameObject.SetActive(true);
        cameraController.ChangeCamera(aimVirtualCamera.transform, aimPivot);
        pcMovement.ChangeCamera(aimVirtualCamera.transform);
        crosshair.gameObject.SetActive(true);

        
    }
    public void DisableAimCamera()
    {
        aimVirtualCamera.gameObject.SetActive(false);
        cameraController.ChangeCamera(followVirtualCamera.transform, cameraPivot);
        pcMovement.ChangeCamera(followVirtualCamera.transform);
        crosshair.gameObject.SetActive(false);
    }

    private void IsAim(bool aim)
    {
        isAim = aim;
    }

    private void Aim(Vector3 mouseWorldPosition)
    {
        //Debug.LogWarning("Aiming");
        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
    }

    private void Shoot(Vector3 mouseWorldPosition)
    {
        //Debug.LogWarning("Shoot");
        if (!isFire && isAim)
        {
            isFire = true;
            Vector3 aimDir = (mouseWorldPosition - shootingPosition.position).normalized;
            Instantiate(arrowPrefab, shootingPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        }
    }

    private void DelayFire()
    {
        if (!isFire)
            return;

        if(currentTime > delayFire && isFire == true)
        {
            currentTime = 0f;
            isFire = false;
        }
        else
        {
            currentTime += Time.deltaTime * 1f;
        }
    }
}
