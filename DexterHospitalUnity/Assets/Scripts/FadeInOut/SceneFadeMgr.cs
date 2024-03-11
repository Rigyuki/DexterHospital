using GameDataManagement;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneFadeMgr : MonoBehaviour
{
    public Animator loadSceneAnim;
 
    private void Start()
    {
        GameObject.DontDestroyOnLoad(loadSceneAnim);
    }

    public IEnumerator LoadScene(string sceneName)
    {
        Debug.Log("jump Scene");
        loadSceneAnim.SetBool("FadeIn", true);
        Debug.Log("fadein");
        loadSceneAnim.SetBool("FadeOut", false);

        yield return new WaitForSeconds(1f);

        AsyncOperation asyn = SceneManager.LoadSceneAsync(sceneName);
        asyn.completed += OnLoadScene;
        //SceneManager.LoadScene(sceneName);
    }

    private void OnLoadScene(AsyncOperation obj)
    {
        Debug.Log("fade out");
        loadSceneAnim.SetBool("FadeIn", false);
        loadSceneAnim.SetBool("FadeOut", true);
    }
}

