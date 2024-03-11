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
public class AttributesMgr : MonoBehaviour
{
    private GameObject _escMenuMgrOnject;
    private EscMenuMgr _escMenuMgr;

    private GameObject _roleHpObject;
    private GameObject _lettersObject;
    private int _letterSum;

    // Start is called before the first frame update
    
    public void FirstLoad ()
    {
        _escMenuMgrOnject = transform.parent.parent.parent.parent.gameObject;
        _escMenuMgr = _escMenuMgrOnject.GetComponent<EscMenuMgr>();

        _letterSum = 22;
        _roleHpObject = transform.Find("ImgHp").gameObject;
        _lettersObject = transform.Find("Letters").gameObject;
        for (int i = 0; i < _letterSum; ++i)
        {
            Button _btn = _lettersObject.transform.Find((i + 1).ToString() + "/ImgLetter").GetComponent<Button>();
            string name = UserdataMgr.Instance.GetLetters(i).Name;
            _btn.onClick.AddListener(() => { _escMenuMgr.OpenPicturePopup("DetailPicture/场景对话/书信相关文件/捡到的信件/" + name, name, false); });
        }
    }

    public void reUpdate()
    {
        _escMenuMgr.UpdateHp(_roleHpObject, UserdataMgr.Instance.GetHp());
        GameObject _letterObject;
        for (int i = 0; i < _letterSum; ++i)
        {
            _letterObject = _lettersObject.transform.Find((i + 1).ToString() + "/ImgLetter").gameObject;
            if (UserdataMgr.Instance.GetLetters(i).isFound == 1)
            {
                _letterObject.SetActive(true);
            }
            else
            {
                _letterObject.SetActive(false);
            }
        }
    }
}
