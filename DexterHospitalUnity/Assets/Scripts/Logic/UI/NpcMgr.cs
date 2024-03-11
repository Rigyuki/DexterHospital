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

public class NpcMgr : MonoBehaviour
{
    private GameObject _escMenuMgrOnject;
    private EscMenuMgr _escMenuMgr;

    private struct NpcObject
    {
        // public GameObject Object;
        public GameObject Role;
        public GameObject Favor;
        public GameObject Hp;
        public GameObject Unknow;
        public Button BtnOpenPopup;

        public void SetLink(GameObject _object)
        {
            Role = _object.transform.Find("ImgRole").gameObject;
            Favor = _object.transform.Find("ImgFavor").gameObject;
            Hp = _object.transform.Find("ImgHp").gameObject;
            Unknow = _object.transform.Find("ImgUnknow").gameObject;

            BtnOpenPopup = Role.GetComponent<Button>();
        }
    }
    private NpcObject[] _npcObjects = new NpcObject[4];

    public void FirstLoad()
    {
        _escMenuMgrOnject = transform.parent.parent.parent.parent.gameObject;
        _escMenuMgr = _escMenuMgrOnject.GetComponent<EscMenuMgr>();

        Npc tempNpc;
        GameObject tempNpcObject;
        string[] names = new string[4];
        for (int i = 0; i < 4; ++i)
        {
            tempNpc = UserdataMgr.Instance.GetNpcs(i);
            tempNpcObject = transform.Find("Role_" + tempNpc.Name).gameObject;
            _npcObjects[i].SetLink(tempNpcObject);
            names[i] = tempNpc.Name;
            int temp = i;
            _npcObjects[i].BtnOpenPopup.onClick.AddListener(() => { _escMenuMgr.OpenPicturePopup("DetailPicture/RoleDetail/" + names[temp], names[temp], true); });
        }

        // update
        
    }

    public void reUpdate()
    {
        Npc tempNpc;
        for (int i = 0; i < 4; ++i)
        {
            tempNpc = UserdataMgr.Instance.GetNpcs(i);
            if (tempNpc.isFound == 1)
            {
                _npcObjects[i].Unknow.SetActive(false);
                _npcObjects[i].Hp.SetActive(true);
                _escMenuMgr.UpdateHp(_npcObjects[i].Hp, tempNpc.Hp); // Hp
                _npcObjects[i].Favor.SetActive(true);
                _escMenuMgr.UpdateFavor(_npcObjects[i].Favor, tempNpc.Favor); // Favor
                _npcObjects[i].Role.SetActive(true);
            }
            else
            {
                _npcObjects[i].Unknow.SetActive(true);
                _npcObjects[i].Hp.SetActive(false);
                _npcObjects[i].Favor.SetActive(false);
                _npcObjects[i].Role.SetActive(false);
            }
        }
    }

}
