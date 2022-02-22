using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineSceneManager : MonoBehaviour
{
    public bool ReadyToWarp = false;
    public Animator teleport;
    private string targetScene = "";
    public static OfflineSceneManager localInstance;
    private void Awake()
    {
        localInstance = this;
    }

    private void Update()
    {
        teleport.SetBool("Ready", ReadyToWarp);
    }

    public void LoadTargetScene(string target)
    {
        targetScene = target;
        teleport.SetBool("Fading", true);
        Invoke("DelayLoad", 2);
    }

    public void DelayLoad()
    {
        SceneManager.LoadScene(targetScene);
    }
}
