﻿using UnityEngine;

namespace ChiliGames.VROffice
{
    //Class that moves the target of Avatar's IK corresponding to the position of the VR user
    [System.Serializable]
    public class VRMap
    {
        public Transform vrTarget;
        public Transform rigTarget;
        public Vector3 trackingPositionOffset;
        public Vector3 trackingRotationOffset;

        //Map Rig's target to the vr tracker/target.
        public void Map()
        {
            if (vrTarget == null) return;
            rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }

    public class Avatar : MonoBehaviour
    {
        public VRMap head;
        public VRMap leftHand;
        public VRMap rightHand;
        public VRMap body;

        public Transform Controller;
        public Transform headConstraint;
        public Transform bodyConstraint;
        Vector3 headBodyOffset;
        private float turnSmoothness = 3f;

        void Start()
        {
            headBodyOffset = transform.position - headConstraint.position;

            //if this is our avatar, we disable the skinned mesh renderer, as we want to only see hands. //not for me O xO
            if (GetComponentInParent<Photon.Pun.PhotonView>().IsMine)
            {
                //GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
        }

        void Update()
        {
            //Lerp our avatar's forward from the head's forward vector projected on the Y/Up plane.
            //this part making the body rotation weird, must fix
            //transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

            Controller.transform.rotation = Quaternion.Euler(0, headConstraint.rotation.eulerAngles.y, 0);
            Controller.transform.position = bodyConstraint.position;

            head.Map();
            leftHand.Map();
            rightHand.Map();
            body.Map();
        }
    }
}
