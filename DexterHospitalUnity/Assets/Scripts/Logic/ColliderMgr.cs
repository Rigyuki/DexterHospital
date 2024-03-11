using GameDataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColliderMgr : MonoBehaviour
{
    private Scene _scene;

    // 场景按钮触发
    private GameObject _scroll;
    private GameObject _panelBtn;
    // scene gameobject
    private GameObject _backGround;

    // dialog
    private GameObject _talkUI;
    private TMP_Text _dialogText;
    private GameObject _showImg;

    private string _contentTotal;
    private string _dialogSpecified;
    private int _dialogIndex;
    private bool talkUI_bool;
    private int index;
    int trigger_count;
    private AudioSource _effectAudio;

    //letter mgr
    private GameObject _letterMgr;


    private SceneFadeMgr _sceneFade;



    private void Awake()
    {
        _scene = SceneManager.GetActiveScene();

        // 场景按钮 触发
        _scroll = transform.Find("Canvas/Scroll View").gameObject;
        _panelBtn = transform.Find("Canvas/Scroll View/Panel").gameObject;
        if (_scene.name != "BossFight")  // TODO 改
            _backGround = transform.parent.Find("BackGround").gameObject;

        // dialog
        _talkUI = transform.Find("Canvas/ViewDialog").gameObject;
        _dialogText = _talkUI.transform.Find("Panel/Content").GetComponent<TMP_Text>();
        Button _btnNext = _talkUI.transform.Find("BtnNext").GetComponent<Button>();
        _btnNext.onClick.AddListener(OnBtnNextClick);
        _showImg = transform.Find("Canvas/Img").gameObject;

        _effectAudio = transform.Find("Canvas").GetComponent<AudioSource>();

        string item_path = Application.dataPath + "/Resources/txt/scene/" + _scene.name + ".csv";
        _contentTotal = getCsvfile(item_path);
        _dialogIndex = 0;
        talkUI_bool = false;

        // letter mgr
        _letterMgr = transform.Find("Canvas/LetterMgr").gameObject;
        _letterMgr.SetActive(false);

        // Fade out
        _sceneFade = transform.parent.Find("SceneFadeMgr").GetComponent<SceneFadeMgr>();
    }

    private void OnBtnNextClick()
    {
        if (talkUI_bool && _dialogIndex <= index)
        {
            dialogUI(_dialogSpecified);
        }
    }

    public string getCsvfile(string fullpath)
    {
        string readedLines = "";
        Debug.Log("getcsv function");
        Encoding utf = Encoding.GetEncoding("UTF-8");

        if (!File.Exists(fullpath))
        {
            Debug.Log("File non-existant...");
        }
        else
        {
            Debug.Log("It Exists...");
            Debug.Log(fullpath);
            StreamReader reader = new StreamReader(fullpath, utf);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                readedLines += line;
                readedLines += "/n";
            }
            reader.Close();
        }
        return readedLines;

    }

    private void Update()
    {
        _scroll.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void UpdateText(string text)
    {
        _dialogText.text = text;
    }

    private void dialogUI(string _dialogSpecified)
    {
        showDialogRow(_dialogSpecified, _dialogIndex);
        _talkUI.SetActive(talkUI_bool);
        _dialogIndex++;
    }

     
    private string getSpecifiedDialog(string _contentTotal, string name, int ClickNum, bool Add1)
    {
        string[] dialogRows = _contentTotal.Split("/n");
        string[] cell = null;

        string dialogs = "";
        int i;
        for (i = 0; i < dialogRows.Length - 1; i++)
        {
            cell = dialogRows[i].Split(",");
            // 找第一条
            if (cell[2] == "1" && cell[1] == name && cell[3] == ClickNum.ToString())
            {
                if (cell[9] == "")
                {
                    break;
                }
                bool existItem = (UserdataMgr.Instance.BagItemNumber(cell[9]) > 0);
                bool existSymbol = (cell[0] == "nodialog"); 
                if (existItem != existSymbol)
                {
                    break;
                }
            }
            cell = null;
        }

        if (cell != null && cell[11] == "Door")
        {
            // 跳转场景
            UserdataMgr.Instance.SetPlayerPlaceCode(_scene.name);
            StartCoroutine(_sceneFade.LoadScene(cell[1]));
            cell[5] = "前往" + cell[1] + "。";
        }

        if (cell == null)
        {
            // 没找到
            return "";
        }
        else if (cell[0] == "dialog" && cell[11] != "Door")
        {
            if (Add1)
            {
                UserdataMgr.Instance.TriggerTimeAdd1(_scene.name, name);
            }
        }

        for (; i < dialogRows.Length - 1; i++)
        {
            cell = dialogRows[i].Split(",");
            trigger_count = int.Parse(cell[3]);
            dialogs += dialogRows[i];
            dialogs += "/n";
            if (cell[0] == "end")
                break;
        }
        return dialogs;
    }

    private void showDialogRow(string _dialogSpecified, int _dialogIndex)
    {
        string[] dialogRows = _dialogSpecified.Split("/n");
        string dialog = dialogRows[_dialogIndex];

        string[] cell = dialog.Split(",");

        // 对话显示图 cell[4]
        if (cell[4] == "")
        {
            _showImg.SetActive(false);
        }
        else
        {
            _showImg.SetActive(true);
            Texture2D img = Resources.Load<Texture2D>("DetailPicture/场景对话/" + cell[4]); // TODO 改文件名
            Debug.Log("图片名 = " + cell[4]);
            Sprite sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), Vector2.zero);
            Image imgtemp = _showImg.GetComponent<Image>();
            imgtemp.sprite = sprite;
            imgtemp.SetNativeSize();
        }

        // 场景物体替换 cell[6]
        if (cell[6] != "")
        {
            UserdataMgr.Instance.ItemObjectReplace(_scene.name, cell[1], cell[6]);
            Debug.Log("替换物品名" + cell[6]);
            GameObject temp = _backGround.transform.Find(cell[6]).gameObject;
            temp.SetActive(true);
            GameObject temp_delete = _backGround.transform.Find(cell[1]).gameObject;
            temp_delete.SetActive(false);
        }

        // 对话内容 cell[5]
        if (cell[0] == "end")
        {
            talkUI_bool = false;
        }
        else // cell[0] == "dialog" cell[0] == "dialogend" || cell[0] == "nodialog"
        {
            UpdateText(cell[5]);
        }
        // door??? TODO

        // 获得道具 cell[7]
        if (cell[7] != "")
        {
            UserdataMgr.Instance.BagAddItem(cell[7], 1);
        }

        // 音效BGM cell[8]
        if (cell[8] != "")
        {
            _effectAudio.PlayOneShot(Resources.Load<AudioClip>("Audio/音效/" + cell[8]));
        }

        // 获得信件 cell[10]
        if (cell[10] != "")
        {
            UserdataMgr.Instance.AddLetter(cell[10]);
        }

        // tag做事情 cell[11]
        if (cell[11] == "Door")
        {
            // 其他地方跳转
        }
        else if (cell[11] == "LetterSystem")
        {
            // TODO 炮弹系统怎么处理
        }
        else if (cell[11] == "Monster")
        {
            // TODO 死亡 转be场景
        }
        else if (cell[11] == "桌椅堵住的门")
        {
            // TODO 死亡 转be场景
        }
        else if (cell[11] == "HP-10")
        {
            UserdataMgr.Instance.SetHp(UserdataMgr.Instance.GetHp() - 10);
        }
        else if (cell[11] == "BossFight")
        {
            // 跳转场景 
            StartCoroutine(_sceneFade.LoadScene("BossFight"));
        }
    }



    public void BeginDialog(string name)
    {
        _dialogIndex = 0;

        int _buttonclicked = UserdataMgr.Instance.GetTriggerTime(_scene.name, name);

        talkUI_bool = true;

        _dialogSpecified = getSpecifiedDialog(_contentTotal, name, _buttonclicked, true);
        index = _dialogSpecified.Split("/n").Length;

        dialogUI(_dialogSpecified);
    }

    // ------------- Collider --------------
    private void OnBtnClick(string tag, string name, GameObject btn)
    {
        btn.GetComponent<Button>().onClick.RemoveAllListeners();
        Debug.Log("场景名 = " + _scene.name + ", 物体名 = " + name);

        if (tag == "Door")
        {
            BeginDialog(name);
        }
        else if (tag == "Item")
        {
            BeginDialog(name);
        }
        else if (tag == "LetterSystem")
        {
            // 开始炮弹系统 写信
            _letterMgr.SetActive(true);

            _dialogIndex = 0;
            int _buttonclicked = UserdataMgr.Instance.GetTriggerTime(_scene.name, name);

            Debug.Log("_buttonclicked = " + _buttonclicked);
            _dialogSpecified = getSpecifiedDialog(_contentTotal, name, _buttonclicked, false);
            string[] cell = _dialogSpecified.Split("/n")[0].Split(",");
            if (cell[0] == "dialogend") //不可收信 
            {
                _letterMgr.GetComponent<LetterMgr>().SetLight(1);
            }
            else
            {
                _letterMgr.GetComponent<LetterMgr>().SetLight(0);
            }

        }

        Destroy(btn);
    }
    private void OnEnter(string tag, string name)
    {
        if (tag == "Monster")
        {
            // TODO be结局
        }
        if (tag == "Item" || tag == "Npc" || tag == "LetterSystem" || tag == "Door")
        {
            GameObject newBtnObject = Instantiate( Resources.Load<GameObject>("Button"), _panelBtn.transform);
            newBtnObject.name = name;
            newBtnObject.transform.Find("Text").GetComponent<Text>().text = name;

            if (tag == "Item")
            {
                newBtnObject.transform.Find("TextBegin").GetComponent<Text>().text = "查看";
            }
            else if (tag == "Npc")
            {
                newBtnObject.transform.Find("TextBegin").GetComponent<Text>().text = "对话";
            }
            else if (tag == "Door")
            {
                newBtnObject.transform.Find("TextBegin").GetComponent<Text>().text = "前往";
            }
            else if (tag == "LetterSystem")
            {
                newBtnObject.transform.Find("TextBegin").GetComponent<Text>().text = "写信";
            }

            // listener
            newBtnObject.GetComponent<Button>().onClick.AddListener(() => 
            { 
                OnBtnClick(tag, name, newBtnObject);
            });
        }
    }
    private void OnExit(string tag, string name)
    {
        if (tag == "Item" || tag == "Npc" || tag == "Door" || tag == "LetterSystem")
        {
            Transform deleteBtn = _panelBtn.transform.Find(name);
            if (deleteBtn != null)
            {
                deleteBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(deleteBtn.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnEnter(collision.gameObject.tag, collision.gameObject.name);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnEnter(collision.gameObject.tag, collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnExit(collision.gameObject.tag, collision.gameObject.name);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        OnExit(collision.gameObject.tag, collision.gameObject.name);
    }

}
