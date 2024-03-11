using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using GameDataManagement;

public class LetterMgr : MonoBehaviour
{
    public TextAsset dialogDataFile; //csv格式文本

    public TMP_Text dialogText;

    public GameObject _LetterMgr;
    public GameObject optionButton;
    public Transform buttonGroup;
    public GameObject _talkTextUI;//对话框ui

    private GameObject _ImageFileObject;
    private GameObject _LetterButton;

    public Image _imageLetter;
    public Image _ImageBg;
    public Image _ImageLight;

    private int dialogIndex = 0;
    private string[] dialogRows;

    private Button _btnNext;
    private Button _btnWrite;
    private Button _btnReceive;
    private Button _btnBack;

    private int _light = 1;
    private string option_content = "";

    private void Awake()
    {
        _ImageBg.sprite = Resources.Load<Sprite>("Sprites/Letter/" + "背景底图");
        _LetterButton = _LetterMgr.transform.Find("RecieveOrWritter").gameObject;
        _ImageFileObject = _LetterMgr.transform.Find("ImageFile").gameObject;

        _btnNext = _talkTextUI.transform.Find("BtnNext").GetComponent<Button>();
        _btnWrite = _LetterButton.transform.Find("writeButton").GetComponent<Button>();
        _btnReceive = _LetterButton.transform.Find("receiveButton").GetComponent<Button>();

        _btnBack = _LetterMgr.transform.Find("backButton").GetComponent<Button>();

         //setImageLight();

        _btnNext.onClick.AddListener(OnBtnNextClick);
        _btnWrite.onClick.AddListener(OnBtnWriteClick);
        _btnReceive.onClick.AddListener(OnBtnReceiveClick);
        _btnBack.onClick.AddListener(OnBtnBackClick);

    }

    public void SetLight(int flag)
    {
        _light = flag;
        setImageLight();
    }

    private void OnBtnWriteClick()
    {

        if (_light == 1) // 可写信
        {
            Scene _scene = SceneManager.GetActiveScene();
            string SceneName = _scene.name;
            int TriggerTime = UserdataMgr.Instance.GetTriggerTime(SceneName, "炮弹系统");
            int LetterIndex = 0;
            if (SceneName == "一楼办公室" && TriggerTime == 1)
            {
                LetterIndex = 1;
            }
            else if (SceneName == "一楼药房" && TriggerTime == 0)
            {
                LetterIndex = 2;
            }
            else if (SceneName == "一楼药房" && TriggerTime == 2)
            {
                LetterIndex = 3;
            }
            else if (SceneName == "二楼监控室" && TriggerTime == 2)
            {
                LetterIndex = 4;
            }

            if (LetterIndex != 0)
            {
                ReadText(dialogDataFile);
                dialogIndex = findLightIndex(_light);

                _talkTextUI.SetActive(true);
                _LetterButton.SetActive(false);

                Debug.Log("Start index = " + dialogIndex);
                showDialogRow();

                // TODO 想办法判断该写哪一封 LetterIndex
                UserdataMgr.Instance.TriggerTimeAdd1(SceneName, "炮弹系统");
            }
            else
            {
                ColliderMgr _colliderMgr = transform.parent.parent.GetComponent<ColliderMgr>();
                _colliderMgr.BeginDialog("炮弹系统信写完了");
                gameObject.SetActive(false);
            }
        }
        else // 可收信，不可写信 
        {
            ColliderMgr _colliderMgr = transform.parent.parent.GetComponent<ColliderMgr>();
            _colliderMgr.BeginDialog("炮弹系统不可写信");
            gameObject.SetActive(false);
        }
    }

    private void OnBtnReceiveClick()
    {
        ColliderMgr coll = transform.parent.parent.GetComponent<ColliderMgr>();
        coll.BeginDialog("炮弹系统");
        gameObject.SetActive(false);
        //_btnBack.gameObject.SetActive(true);
        //_LetterButton.SetActive(false);
        //_imageLetter.sprite = Resources.Load<Sprite>("Sprites/Letter/" + SceneManager.GetActiveScene().name);
        //_imageLetter.gameObject.SetActive(true);
    }

    private void OnBtnBackClick()
    {
        // Destroy(_LetterMgr);
        gameObject.SetActive(false);
    }

    public void setImageLight()
    {
        if (_light == 1)
        {
            _ImageLight.sprite = Resources.Load<Sprite>("Sprites/Letter/" + "炮弹系统蓝灯");
        }
        else if (_light == 0)
        {
            _ImageLight.sprite = Resources.Load<Sprite>("Sprites/Letter/" + "炮弹系统红灯");
        }
        _imageLetter.gameObject.SetActive(false);
        _btnBack.gameObject.SetActive(false);
        _LetterButton.SetActive(true);
        _talkTextUI.gameObject.SetActive(false);
    }

    private void OnBtnNextClick()
    {
        Debug.Log(dialogIndex);
        showDialogRow();
    }

    public void UpdateText(string text)
    {
        //Debug.Log("UpdateText ");
        dialogText.text = text;
    }


    public void ReadText(TextAsset textAsset)
    {
        dialogRows = textAsset.text.Split('\n');
    }

    public void PlaySound(string soundName)
    {
        AudioMgr.Instance.PlayEffect(soundName);  
    }

    public int findLightIndex(int _light)
    {
        Debug.Log("_light = " + _light);

        int i = 0;
        Debug.Log("length = " + dialogRows.Length);
        
        for (i = 1; i < dialogRows.Length - 2; i++)
        {
            string[] cell = dialogRows[i].Split(',');
            Debug.Log(cell[11] + "*" + _light);

            if (int.Parse(cell[11]) == _light)
            {
                return i;
            }
        }
        return i;
    }

    public void showUI(bool talkUIboolen, bool optionButonboolen)
    {
        _talkTextUI.SetActive(talkUIboolen);
        optionButton.SetActive(optionButonboolen);
    }

    public void showDialogRow()
    {
        for (int i = dialogIndex; i < dialogRows.Length; i++)
        {
            //_imageFile.SetActive(false);
            //Debug.Log("row = " + dialogRows[i]);
            string[] cell = dialogRows[i].Split(',');

            if (cell[0] == "dialog" && int.Parse(cell[1]) == dialogIndex)
            {
                //Debug.Log("cell content = " + cell[5]);
                showUI(true, false);

                if (cell[8] != "")
                {
                    PlaySound(cell[8]);
                }
                if (option_content != "")
                {
                    UpdateText(option_content);
                }
                else
                {
                    UpdateText(cell[5]);
                }
                dialogIndex = int.Parse(cell[9]);
                break;
            }

            else if (cell[0] == "option" && int.Parse(cell[1]) == dialogIndex)
            {
                showUI(false, true);
                GenerateOption(i);
                Debug.Log("分支选项");
                break;
            }

            else if (cell[0] == "optionend" && int.Parse(cell[1]) == dialogIndex)
            {
                showUI(true, false);
                UpdateText(cell[5]);
                option_content = "";
                dialogIndex = int.Parse(cell[9]);
            }
            else if (cell[0] == "end")
            {
                // Destroy(_LetterMgr);
                gameObject.SetActive(false);
            }
        }
    }


    public void GenerateOption(int _index)
    {
        string[] cell = dialogRows[_index].Split(',');
        if (cell[0] == "option")
        {
            GameObject btn = Instantiate(optionButton, buttonGroup);  //生成分支的对话框
            btn.SetActive(true);
            
            btn.GetComponentInChildren<TMP_Text>().text = cell[5]; //找到按钮下面的文本并且修改   

            btn.GetComponent<Button>().onClick.AddListener(     //给生成的按钮添加事件
                delegate 
                {
                    Debug.Log("跳转index = " + cell[9]);
                    option_content = cell[5];
                    //if (option_content[0] == 'A')
                    //{
                    //    int npc = int.Parse(cell[12]);
                    //    int favor = UserdataMgr.Instance.GetNpcs(npc).Favor;
                    //    favor += 10;
                    //    UserdataMgr.Instance.SetNpcs(npc).Favor;
                    //}


                    option_content = option_content.Substring(1, option_content.Length - 1);
                    OnOptionClick(int.Parse(cell[9])); 
                });  ////使用一个委托来添加事件

            GenerateOption(_index + 1);  //使用递归，直到列表的表示不是&为止  从而获取所有的对话语言
        }

    }

    public void OnOptionClick(int index)
    {
        dialogIndex = index;  //给不同的分支给与不同的跳转ID就是Index
        showDialogRow(); //继续开始语言说明

        for (int i = 0; i < buttonGroup.childCount; i++) //点击按钮之后就将所有的生成的对话语言删除
        {
            Destroy(buttonGroup.GetChild(i).gameObject);
        }

        
    }

}
