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

public class BagMgr : MonoBehaviour
{
    private GameObject _escMenuMgrOnject;
    private EscMenuMgr _escMenuMgr;

    private BagItem[] BagItemArray;
    private int CurrentPage;
    private Button _btnPageUp;
    private Button _btnPageDown;
    private GameObject[] _bagSlotsObject = new GameObject[8];
    private Image[] _bagSlotsImg = new Image[8];
    private Text[] _bagSlotsTxt = new Text[8];
    private Button[] _bagSlotsBtn = new Button[8];

    public void FirstLoad()
    {
        _escMenuMgrOnject = transform.parent.parent.parent.parent.gameObject;
        _escMenuMgr = _escMenuMgrOnject.GetComponent<EscMenuMgr>();

        _btnPageUp = transform.Find("BtnPageUp").GetComponent<Button>();
        _btnPageDown = transform.Find("BtnPageDown").GetComponent<Button>();
        _btnPageUp.onClick.AddListener(BtnPageUpOnclick);
        _btnPageDown.onClick.AddListener(BtnPageDownOnclick);
        for (int i = 0; i < 8; i++)
        {
            _bagSlotsObject[i] = transform.Find("Slots/" + (i + 1).ToString()).gameObject;
            _bagSlotsImg[i] = _bagSlotsObject[i].transform.Find("Img").GetComponent<Image>();
            _bagSlotsBtn[i] = _bagSlotsObject[i].transform.Find("Btn").GetComponent<Button>();
            _bagSlotsTxt[i] = _bagSlotsObject[i].transform.Find("Txt").GetComponent<Text>();
            int temp = i;
            _bagSlotsBtn[i].onClick.AddListener(() => { OnBagSlotsBtnClick(temp); });
        }

        // update
        //reUpdate();
    }

    private void BtnPageUpOnclick()
    {
        // TODO
    }
    private void BtnPageDownOnclick()
    {

    }
    private void BagShowPage() // page0
    {
        // item index*8 - index*8-1
        BagItemArray = UserdataMgr.Instance.GetBag().ToArray();
        int bagSize = BagItemArray.Length;
        bagSize = Math.Min(bagSize, CurrentPage * 8 + 8);
        for (int i = 0; i < 8; ++i)
        {
            if (CurrentPage * 8 + i >= bagSize)
            {
                // not show
                _bagSlotsObject[i].SetActive(false);
            }
            else
            {
                // show BagItemArray[CurrentPage * 8 + i]
                _bagSlotsObject[i].SetActive(true);

                string path = "DetailPicture/场景对话/" + BagItemArray[CurrentPage * 8 + i].Name;
                Texture2D img = Resources.Load<Texture2D>(path);
                Sprite sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
                _bagSlotsImg[i].sprite = sprite;
                _bagSlotsImg[i].SetNativeSize();

                _bagSlotsTxt[i].text = BagItemArray[CurrentPage * 8 + i].Count.ToString();
            }
        }
    }

    private void OnBagSlotsBtnClick(int index) // 012
    {
        // OpenPicturePopup("DetailPicture/RoleDetail/" + tempNpc.Name, tempNpc.Name, false);
        // show BagItemArray[CurrentPage * 8 + i]
        string name = BagItemArray[CurrentPage * 8 + index].Name;
        _escMenuMgr.OpenPicturePopup("DetailPicture/场景对话/" + name, name, false);
    }

    public void reUpdate()
    {
        CurrentPage = 0;
        BagShowPage();
    }
}
