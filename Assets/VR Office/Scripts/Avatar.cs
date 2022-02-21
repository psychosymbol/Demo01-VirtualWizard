using UnityEngine;
using Photon.Voice.Unity;

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
        public SkinnedMeshRenderer bodyBlendshape;
        public SkinnedMeshRenderer faceBlendshape;
        public Animator animator;
        public Speaker speaker;
        Vector3 headBodyOffset;
        private float turnSmoothness = 3f;

        private float lerp = 0;
        private float duration = 1f;
        private Vector3 endPos;

        int mouth = 0;
        int eye = 0;

        void Start()
        {
            headBodyOffset = transform.position - headConstraint.position;

            //if this is our avatar, we disable the skinned mesh renderer, as we want to only see hands. //not for me O xO
            if (GetComponentInParent<Photon.Pun.PhotonView>().IsMine)
            {
                //GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }

            Invoke("blink", Random.Range(3f, 6f));
        }

        void Update()
        {
            //Lerp our avatar's forward from the head's forward vector projected on the Y/Up plane.
            //this part making the body rotation weird, must fix
            //transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);
            
            Controller.transform.rotation = Quaternion.Euler(0, headConstraint.rotation.eulerAngles.y, 0);

            if (Controller.transform.position != endPos)
            {
                lerp += Time.deltaTime / duration; // Tune duration between 0.8 - 1
                lerp = (endPos.y < 0) ? lerp * 2f : lerp;
                Controller.transform.position = Vector3.Lerp(Controller.transform.position, endPos, lerp);
            }
            else
            {
                lerp = Time.deltaTime / duration;
                lerp = (endPos.y < 0) ? lerp * 2f : lerp;
                endPos = bodyConstraint.position;
            }


            //blendshape talk
            if (speaker.IsPlaying)
            {
                if (mouth == 0)
                {
                    mouth = 50;
                }

                if (mouth >= 100) mouth--;
                else if (mouth <= 50) mouth++;
            }
            else
            {
                mouth = 0;
            }
            faceBlendshape.SetBlendShapeWeight(animator.GetInteger("Mouth"), mouth);
            //blendshape talk

            faceBlendshape.SetBlendShapeWeight(animator.GetInteger("EyeL"), eye);
            faceBlendshape.SetBlendShapeWeight(animator.GetInteger("EyeR"), eye);

            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("LThumb"), animator.GetFloat("LThumbin"));
            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("RThumb"), animator.GetFloat("RThumbin"));

            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("LIndex"), animator.GetFloat("LPoint"));
            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("RIndex"), animator.GetFloat("RPoint"));

            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("LMiddle"), animator.GetFloat("LGrip"));
            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("LRing"), animator.GetFloat("LGrip"));
            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("LPinky"), animator.GetFloat("LGrip"));
            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("RMiddle"), animator.GetFloat("RGrip"));
            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("RRing"), animator.GetFloat("RGrip"));
            bodyBlendshape.SetBlendShapeWeight(animator.GetInteger("RPinky"), animator.GetFloat("RGrip"));

            head.Map();
            leftHand.Map();
            rightHand.Map();
            body.Map();
        }

        //blendshape blink
        public void blink()
        {
            if (eye == 0)
            {
                eye = 100;
            }
            
            if(eye > 0)
            {
                eye--;
                Invoke("blink", Time.deltaTime);
            }
            else if (eye <= 0)
            {
                eye = 0;
                Invoke("blink", Random.Range(3f, 6f));
            }

        }
    }
}

