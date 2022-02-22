using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChiliGames.VROffice;
using Photon.Pun;
using Photon.Voice.Unity;

public class PCBody : MonoBehaviour
{

    public Transform[] body;
    public Animator bodyAnim;
    public Transform Model;
    public Transform bodyPosition;

    public SkinnedMeshRenderer faceBlend;
    public SkinnedMeshRenderer bodyBlend;

    public Speaker speaker;

    PhotonView pv;

    public int MouthBlendshape;
    public int LEyeshape;
    public int REyeshape;

    float mouth = 0;
    float eye = 0;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        Invoke("blink", Random.Range(6f, 15f));
    }


    void Update()
    {
        //for syncing with their own local position
        if (pv.IsMine)
        {
            for (int i = 0; i < body.Length; i++)
            {
                body[i].position = PlatformManager.instance.vrRigParts[i].position;
                body[i].rotation = PlatformManager.instance.vrRigParts[i].rotation;
            }
        }

        //for syncing online
        Model.position = bodyPosition.position;
        Model.rotation = bodyPosition.rotation;

        //mouth blendshape value
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

        bodyAnim.SetFloat("Mouth", mouth);

        //set blendshape value
        faceBlend.SetBlendShapeWeight(MouthBlendshape, bodyAnim.GetFloat("Mouth"));
        faceBlend.SetBlendShapeWeight(LEyeshape, eye);
        faceBlend.SetBlendShapeWeight(REyeshape, eye);

    }

    //eye blendshape value
    public void blink()
    {
        if (eye == 0)
        {
            eye = 100;
        }

        if (eye > 0)
        {
            eye-=5;
            Invoke("blink", Time.deltaTime);
        }
        else if (eye <= 0)
        {
            eye = 0;
            Invoke("blink", Random.Range(6f, 15f));
        }

    }
}
