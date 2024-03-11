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

public class SaveMgr : MonoBehaviour
{
    private GameObject _escMenuMgrOnject;
    private EscMenuMgr _escMenuMgr;

    private struct SaveObject
    {
        public Image ImgScene;
        public GameObject ImgMask;
        public GameObject RoleHp;
        public GameObject XiaoXiHp;
        public Text TxtLetterNumber;
        public Text TxtTime;
        public Text TxtSceneName;

        public void SetLink(GameObject _object)
        {
            ImgScene = _object.transform.Find("Contant/ImgScene").GetComponent<Image>();
            ImgMask = _object.transform.Find("ImgBG").gameObject;
            RoleHp = _object.transform.Find("Contant/RoleStatus").gameObject;
            XiaoXiHp = _object.transform.Find("Contant/XiaoXiStatus").gameObject;
            TxtLetterNumber = _object.transform.Find("Contant/TxtLetterNumber").GetComponent<Text>();
            TxtTime = _object.transform.Find("Contant/TxtTime").GetComponent<Text>();
            TxtSceneName = _object.transform.Find("Contant/TxtSceneName").GetComponent<Text>();
        }
    }
    private SaveObject[] _saveObjects = new SaveObject[3];
    private GameObject[] _btnChooseObject = new GameObject[3];


    public void FirstLoad()
    {
        _escMenuMgrOnject = transform.parent.parent.parent.parent.gameObject;
        _escMenuMgr = _escMenuMgrOnject.GetComponent<EscMenuMgr>();

        GameObject tempSaveObject;
        for (int i = 0; i < 3; ++i)
        {
            tempSaveObject = transform.Find("SaveSlot" + (i + 1).ToString()).gameObject;
            _saveObjects[i].SetLink(tempSaveObject);
            _btnChooseObject[i] = transform.Find("BtnChoose/" + (i + 1).ToString()).gameObject;
            int temp = i;
            _btnChooseObject[i].GetComponent<Button>().onClick.AddListener(() => { _escMenuMgr.OpenSavePopup(temp); });
        }

        // update
        
    }

    public void reUpdate()
    {
        for (int i = 0; i < 3; ++i)
        {
            SaveData tempSaveData = SavedataMgr.Instance.GetSave(i);
            if (!tempSaveData.IsUsed)
            {
                _saveObjects[i].ImgMask.SetActive(true);
                _btnChooseObject[i].SetActive(false);
            }
            else
            {
                Texture2D img = Resources.Load<Texture2D>("DetailPicture/´æµµµ×Í¼/" + tempSaveData.SceneName);
                Sprite sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
                _saveObjects[i].ImgScene.sprite = sprite;

                _escMenuMgr.UpdateHp(_saveObjects[i].RoleHp, tempSaveData.RoleHp);
                _escMenuMgr.UpdateHp(_saveObjects[i].XiaoXiHp, tempSaveData.XiaoXiHp);
                _saveObjects[i].TxtLetterNumber.text = tempSaveData.LetterNumber.ToString();
                _saveObjects[i].TxtTime.text = tempSaveData.SaveTime;
                _saveObjects[i].TxtSceneName.text = tempSaveData.SceneName;
                _saveObjects[i].ImgMask.SetActive(false);
                _btnChooseObject[i].SetActive(true);
            }
        }
    }
}
