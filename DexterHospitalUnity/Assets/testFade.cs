using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class testFade : MonoBehaviour
{
    //_SceneName _sceneName;

    public SceneFadeMgr sceneFade;

    public Button startBtn;

    private void Awake()
    {
       
    }
    private void Start()
    {

        /*Button btn = this.GetComponent<Button>();

        btn.onClick.AddListener(OnClick);*/

        startBtn.onClick.AddListener(delegate () {
            Debug.Log("test!");
            //SceneFadeMgr.loadSceneVoid("");
            //sceneFade.LoadScene("GameStart");
            StartCoroutine(sceneFade.LoadScene("test1_fadeInOut"));
        });
    }

    private  void OnClick()
    {
        //_sceneName._sceneName = "GameStart";
        //sceneFade.LoadScene("GameStart");
        //sceneFade.loadSceneVoid("GameStart");
        Debug.Log("click");
    }
}
