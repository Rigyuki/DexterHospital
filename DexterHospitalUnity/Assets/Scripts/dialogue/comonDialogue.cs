using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using GameDataManagement;
using JetBrains.Annotations;
using System.Threading;

public class comonDialogue : MonoBehaviour
{
    public TextAsset dialogDataFile; //csv格式文本
    public GameObject BackGround;

    public TMP_Text nameText;
    public TMP_Text dialogText;

    private GameObject _dialogUI;
    public GameObject deleted;
    public GameObject change;

    public GameObject optionButton;
    public Transform buttonGroup;

    public Image image1;
    public Image image2;
    public Image _image;

    public float textSpeed;

    private int dialogIndex = 0;
    public string[] dialogRows;

    private Button _btnNext;
    public GameObject _talkTextUI;//对话框ui

    bool textfinished;

    private void Awake()
    {
        _dialogUI = transform.Find("Canvas/Dialog").gameObject;
        _btnNext = _dialogUI.transform.Find("BtnNext").GetComponent<Button>();
        _btnNext.onClick.AddListener(OnBtnNextClick);
    }
    void Start()
    {
        textfinished = true;

        ReadText(dialogDataFile);
        showDialogRow();
    }

    private void OnBtnNextClick()
    {
        //Debug.Log(dialogIndex);
        if (textfinished)
        {
            showDialogRow();
        }
    }

    public void UpdateText(string name, string text)
    {
        //Debug.Log("UpdateText ");
        nameText.text = name;
        dialogText.text = "";

        StartCoroutine(func(text));
    }

    IEnumerator func(string text)
    {
        textfinished = false;
        for (int i = 0; i < text.Length; i++)
        {
            dialogText.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }

        textfinished = true;
    }

    public void setUI(bool charcterBoolen1, bool charcterBoolen2,bool imageFileBoolen)
    {
        image1.gameObject.SetActive(charcterBoolen1);
        image2.gameObject.SetActive(charcterBoolen2);
        _image.gameObject.SetActive(imageFileBoolen);
    }

    public void UpdateImage(string name1, string name2)
    {
        image1.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name1);
        image2.sprite = Resources.Load<Sprite>("Sprites/Characters/" + name2);
        if (name1 != "" && name2 != "")
        {
            setUI(true, true, false);
        }

        if (name1 == "" && name2 != "")
        {
            setUI(false, true, false);
        }
        if (name1 != "" && name2 == "")
        {
            setUI(true, false, false);
        }

        if (name1 == "" && name2 == "")
        {
            setUI(false, false, false);
        }

    }

    public void ReadText(TextAsset textAsset)
    {
        dialogRows = textAsset.text.Split('\n');
    }

    public void PlaySound(string name, int soundType, string soundName)
    {
        switch (soundType)
        {
            case 1:
                AudioMgr.Instance.playDialogue(
                     name + "/" + soundName);
                break;
            case 2:
                AudioMgr.Instance.PlayEffect(soundName);
                break;
            case 3:
                AudioMgr.Instance.PlayMusic(soundName);
                break;
            default:
                break;
        }
    }

    public void showDialogRow()
    {
        string content_copy = "";
        for (int i = 0; i < dialogRows.Length; i++)
        {
            
            //Debug.Log("row " + row);
            string[] cell = dialogRows[i].Split(',');

            if (cell[0] == "start" && int.Parse(cell[1]) == dialogIndex)
            {
                UpdateImage(cell[4], cell[5]);
                content_copy = wordCountString(cell[6]);
                UpdateText(cell[2], content_copy);
                //Debug.Log("content_copy " + content_copy);

                if (cell[9] != "")
                {
                    Debug.Log("soudname " + cell[9]);
                    PlaySound(name, 3, cell[9]);
                }
                dialogIndex = int.Parse(cell[7]);//跳转到指定index

                Debug.Log("跳转到  " + dialogIndex);
                break;
            }
            else if (cell[0] == "dialog" && int.Parse(cell[1]) == dialogIndex)
            {
                UpdateImage(cell[4], cell[5]);
                content_copy = wordCountString(cell[6]);
                UpdateText(cell[2], content_copy);
                Debug.Log("dialog " + content_copy);
                _btnNext.gameObject.SetActive(true);
                _talkTextUI.SetActive(true);

                if (cell[8] != "")
                {
                    PlaySound(name, 2, cell[8]);
                }

                if (cell[10] != "")
                {
                    deleted.SetActive(false);
                    change.SetActive(true);
                }

                dialogIndex = int.Parse(cell[7]);
                break;
            }

            else if (cell[0] == "option" && int.Parse(cell[1]) == dialogIndex)
            {
                _btnNext.gameObject.SetActive(false);
                GenerateOption(i);
                Debug.Log("分支选项");
                //dialogIndex = int.Parse(cell[7]);
                break;
            }

            else if (cell[0] == "image" && int.Parse(cell[1]) == dialogIndex)
            {
                Debug.Log("image");
                _talkTextUI.SetActive(false);


                setUI(false, false, true);
                _image.sprite = Resources.Load<Sprite>("Sprites/FileImage/" + cell[3]);

                dialogIndex = int.Parse(cell[7]);
                break;
            }

            else if (cell[0] == "END")
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Destroy(_dialogUI);
            }
            else if (cell[0] == "BE" && int.Parse(cell[1]) == dialogIndex)
            {
                int code = int.Parse(cell[2]);
                UserdataMgr.Instance.SetBeCode(code);
                SceneManager.LoadScene("BE");
            }
        }


    }


    public string wordCountString(string content)
    {
        string content_copy = "";
        int count = 0;
        for (int i = 0; i < content.Length; i++)
        {
            content_copy += content[i];
            if (count == 28)
            {
                content_copy += "\n";
                count = 0;
            }
            count++;
        }
        return content_copy;
    }


    public void GenerateOption(int _index)
    {
        string[] cell = dialogRows[_index].Split(',');
        if (cell[0] == "option")
        {
            GameObject btn = Instantiate(optionButton, buttonGroup);  //生成分支的对话框
            btn.SetActive(true);
            
            btn.GetComponentInChildren<TMP_Text>().text = cell[6]; //找到按钮下面的文本并且修改   

            btn.GetComponent<Button>().onClick.AddListener(     //给生成的按钮添加事件
                delegate 
                {
                    Debug.Log("跳转index = " + cell[7]);
                    OnOptionClick(int.Parse(cell[7])); 
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
