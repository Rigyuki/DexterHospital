using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using GameDataManagement;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;

public class SettingsMgr : MonoBehaviour
{
    private GameObject _escMenuMgrOnject;
    private EscMenuMgr _escMenuMgr;

    private SceneFadeMgr _sceneFade;

    public void FirstLoad()
    {
        _escMenuMgrOnject = transform.parent.parent.parent.parent.gameObject;
        _escMenuMgr = _escMenuMgrOnject.GetComponent<EscMenuMgr>();

        Button _btnReturnStart = transform.Find("BtnReturnStart").GetComponent<Button>();
        _btnReturnStart.onClick.AddListener(OnBtnReturnStartClick);
        _sceneFade = _escMenuMgrOnject.transform.parent.Find("SceneFadeMgr").GetComponent<SceneFadeMgr>();
    }

    private void OnBtnReturnStartClick()
    {
        StartCoroutine(_sceneFade.LoadScene("GameStart"));
    }

    public void reUpdate()
    {

    }
}
