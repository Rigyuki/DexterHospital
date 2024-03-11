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

public class EscMenuMgr : MonoBehaviour
{
    // open esc menu
    private KeyCode _menuKey;
    private GameObject _menu;
    // select btn & view
    private GameObject _viewAttributes;
    private GameObject _viewNpc;
    private GameObject _viewBag;
    private GameObject _viewSettings;
    private GameObject _viewSave;
    // 内部变量
    private GameObject _viewOpen;
    private int uiLayer;

    // Popups
    private GameObject _itemPopup;
    private Text _itemPopupTxt;
    private Image _itemPopupImg;
    private GameObject _checkboxPopup;

    void Awake()
    {
        _menuKey = KeyCode.Escape;
        _menu = transform.Find("Canvas/Panel/ViewEsc").gameObject;

        // select btn & view
        GameObject _viewSelectBtns = _menu.transform.Find("SelectBtns").gameObject;
        Button _btnAttributes = _viewSelectBtns.transform.Find("Btn1").GetComponent<Button>();
        Button _btnNpc = _viewSelectBtns.transform.Find("Btn2").GetComponent<Button>();
        Button _btnBag = _viewSelectBtns.transform.Find("Btn3").GetComponent<Button>();
        Button _btnSettings = _viewSelectBtns.transform.Find("Btn4").GetComponent<Button>();
        Button _btnSave = _viewSelectBtns.transform.Find("Btn5").GetComponent<Button>();

        _viewAttributes = _menu.transform.Find("View1_Attributes").gameObject;
        _viewNpc = _menu.transform.Find("View2_Npc").gameObject;
        _viewBag = _menu.transform.Find("View3_Bag").gameObject;
        _viewSettings = _menu.transform.Find("View4_Settings").gameObject;
        _viewSave = _menu.transform.Find("View5_Save").gameObject;

        // class verify
        _viewOpen = _viewAttributes;
        uiLayer = 0;

        // select btn
        _btnAttributes.onClick.AddListener(OnBtnAttributesClick);
        _btnNpc.onClick.AddListener(OnBtnNpcClick);
        _btnBag.onClick.AddListener(OnBtnBagClick);
        _btnSettings.onClick.AddListener(OnBtnSettingsClick);
        _btnSave.onClick.AddListener(OnBtnSaveClick);

        // page content
        // Popup
        _itemPopup = _menu.transform.Find("ItemPopup").gameObject;
        _itemPopupTxt = _itemPopup.transform.Find("Txt").GetComponent<Text>();
        _itemPopupImg = _itemPopup.transform.Find("Img").GetComponent<Image>();
        _checkboxPopup = _menu.transform.Find("CheckboxPopup").gameObject;


        _viewAttributes.transform.GetComponent<AttributesMgr>().FirstLoad();
        _viewNpc.transform.GetComponent<NpcMgr>().FirstLoad();
        _viewBag.transform.GetComponent<BagMgr>().FirstLoad();
        _viewSettings.transform.GetComponent<SettingsMgr>().FirstLoad();
        _viewSave.transform.GetComponent<SaveMgr>().FirstLoad();

    }

    public void OpenSavePopup(int index) // v= 012
    {
        _checkboxPopup.SetActive(true);
        uiLayer = 2;
        // TODO Btn
    }

    public void OpenPicturePopup(string path, string name, bool isNpc) // for picture popup
    {
        _itemPopup.SetActive(true);
        Texture2D img = Resources.Load<Texture2D>(path);
        Sprite sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
        _itemPopupImg.sprite = sprite;
        _itemPopupImg.SetNativeSize();
        _itemPopupTxt.text = name;
        uiLayer = 2;

        if (isNpc)
            _itemPopupImg.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        else
            _itemPopupImg.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25);
    }

    void Update()
    {
        if (Input.GetKeyDown(_menuKey))
        {
            if (uiLayer == 0)
            {
                OpenEscMenu();
                uiLayer = 1;
            }
            else if (uiLayer == 1)
            {
                CloseEscMenu();
                uiLayer = 0;
            }
            else // 2
            {
                _itemPopup.SetActive(false);
                _checkboxPopup.SetActive(false);
                uiLayer = 1;
            }
        }
    }

    private void OnBtnAttributesClick()
    {
        if (uiLayer == 2)
        {
            _itemPopup.SetActive(false);
            _checkboxPopup.SetActive(false);
            uiLayer = 1;
        }
        if (_viewOpen != _viewAttributes)
        {
            _viewOpen.SetActive(false);
            _viewOpen = _viewAttributes;
            _viewOpen.SetActive(true);
        }
    }
    private void OnBtnNpcClick()
    {
        if (uiLayer == 2)
        {
            _itemPopup.SetActive(false);
            _checkboxPopup.SetActive(false);
            uiLayer = 1;
        }
        if (_viewOpen != _viewNpc)
        {
            _viewOpen.SetActive(false);
            _viewOpen = _viewNpc;
            _viewOpen.SetActive(true);
        }
    }
    private void OnBtnBagClick()
    {
        if (uiLayer == 2)
        {
            _itemPopup.SetActive(false);
            _checkboxPopup.SetActive(false);
            uiLayer = 1;
        }
        if (_viewOpen != _viewBag)
        {
            _viewOpen.SetActive(false);
            _viewOpen = _viewBag;
            _viewOpen.SetActive(true);
        }
    }
    private void OnBtnSettingsClick()
    {
        if (uiLayer == 2)
        {
            _itemPopup.SetActive(false);
            _checkboxPopup.SetActive(false);
            uiLayer = 1;
        }
        if (_viewOpen != _viewSettings)
        {
            _viewOpen.SetActive(false);
            _viewOpen = _viewSettings;
            _viewOpen.SetActive(true);
        }
    }
    private void OnBtnSaveClick()
    {
        if (uiLayer == 2)
        {
            _itemPopup.SetActive(false);
            _checkboxPopup.SetActive(false);
            uiLayer = 1;
        }
        if (_viewOpen != _viewSave)
        {
            _viewOpen.SetActive(false);
            _viewOpen = _viewSave;
            _viewOpen.SetActive(true);
        }
    }

    
    private void CloseEscMenu()
    {
        if (_viewOpen != _viewAttributes)
        {
            _viewOpen.SetActive(false);
            _viewOpen = _viewAttributes;
            _viewOpen.SetActive(true);
        }
        _itemPopup.SetActive(false);
        _checkboxPopup.SetActive(false);
        _menu.SetActive(false);
    }

    private void OpenEscMenu()
    {
        _menu.SetActive(true);
        _itemPopup.SetActive(false);
        _checkboxPopup.SetActive(false);


        // reload
        _viewAttributes.transform.GetComponent<AttributesMgr>().reUpdate();
        _viewNpc.transform.GetComponent<NpcMgr>().reUpdate();
        _viewBag.transform.GetComponent<BagMgr>().reUpdate();
        _viewSettings.transform.GetComponent<SettingsMgr>().reUpdate();
        _viewSave.transform.GetComponent<SaveMgr>().reUpdate();
    }

    public void UpdateFavor(GameObject TargetObject, int value)
    {
        GameObject _favorImg;
        for (int i = 0; i < 5; ++i)
        {
            _favorImg = TargetObject.transform.Find(i.ToString()).gameObject;
            if (value == i) _favorImg.SetActive(true);
            else _favorImg.SetActive(false);
        }
    }

    public void UpdateHp(GameObject TargetObject, int value)
    {
        GameObject Img100 = TargetObject.transform.Find("100").gameObject;
        GameObject Img75 = TargetObject.transform.Find("75").gameObject;
        GameObject Img50 = TargetObject.transform.Find("50").gameObject;
        GameObject Img25 = TargetObject.transform.Find("25").gameObject;
        Img100.SetActive(false);
        Img75.SetActive(false);
        Img50.SetActive(false);
        Img25.SetActive(false);
        if (value >= 90) Img100.SetActive(true);
        else if (value >= 60) Img75.SetActive(true);
        else if (value >= 30) Img50.SetActive(true);
        else Img25.SetActive(true);
    }
}
