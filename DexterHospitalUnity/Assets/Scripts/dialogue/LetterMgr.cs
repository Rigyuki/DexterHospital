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
    public TextAsset dialogDataFile; //csv��ʽ�ı�

    public TMP_Text dialogText;

    public GameObject _LetterMgr;
    public GameObject optionButton;
    public Transform buttonGroup;
    public GameObject _talkTextUI;//�Ի���ui

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
        _ImageBg.sprite = Resources.Load<Sprite>("Sprites/Letter/" + "������ͼ");
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

        if (_light == 1) // ��д��
        {
            Scene _scene = SceneManager.GetActiveScene();
            string SceneName = _scene.name;
            int TriggerTime = UserdataMgr.Instance.GetTriggerTime(SceneName, "�ڵ�ϵͳ");
            int LetterIndex = 0;
            if (SceneName == "һ¥�칫��" && TriggerTime == 1)
            {
                LetterIndex = 1;
            }
            else if (SceneName == "һ¥ҩ��" && TriggerTime == 0)
            {
                LetterIndex = 2;
            }
            else if (SceneName == "һ¥ҩ��" && TriggerTime == 2)
            {
                LetterIndex = 3;
            }
            else if (SceneName == "��¥�����" && TriggerTime == 2)
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

                // TODO ��취�жϸ�д��һ�� LetterIndex
                UserdataMgr.Instance.TriggerTimeAdd1(SceneName, "�ڵ�ϵͳ");
            }
            else
            {
                ColliderMgr _colliderMgr = transform.parent.parent.GetComponent<ColliderMgr>();
                _colliderMgr.BeginDialog("�ڵ�ϵͳ��д����");
                gameObject.SetActive(false);
            }
        }
        else // �����ţ�����д�� 
        {
            ColliderMgr _colliderMgr = transform.parent.parent.GetComponent<ColliderMgr>();
            _colliderMgr.BeginDialog("�ڵ�ϵͳ����д��");
            gameObject.SetActive(false);
        }
    }

    private void OnBtnReceiveClick()
    {
        ColliderMgr coll = transform.parent.parent.GetComponent<ColliderMgr>();
        coll.BeginDialog("�ڵ�ϵͳ");
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
            _ImageLight.sprite = Resources.Load<Sprite>("Sprites/Letter/" + "�ڵ�ϵͳ����");
        }
        else if (_light == 0)
        {
            _ImageLight.sprite = Resources.Load<Sprite>("Sprites/Letter/" + "�ڵ�ϵͳ���");
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
                Debug.Log("��֧ѡ��");
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
            GameObject btn = Instantiate(optionButton, buttonGroup);  //���ɷ�֧�ĶԻ���
            btn.SetActive(true);
            
            btn.GetComponentInChildren<TMP_Text>().text = cell[5]; //�ҵ���ť������ı������޸�   

            btn.GetComponent<Button>().onClick.AddListener(     //�����ɵİ�ť����¼�
                delegate 
                {
                    Debug.Log("��תindex = " + cell[9]);
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
                });  ////ʹ��һ��ί��������¼�

            GenerateOption(_index + 1);  //ʹ�õݹ飬ֱ���б�ı�ʾ����&Ϊֹ  �Ӷ���ȡ���еĶԻ�����
        }

    }

    public void OnOptionClick(int index)
    {
        dialogIndex = index;  //����ͬ�ķ�֧���벻ͬ����תID����Index
        showDialogRow(); //������ʼ����˵��

        for (int i = 0; i < buttonGroup.childCount; i++) //�����ť֮��ͽ����е����ɵĶԻ�����ɾ��
        {
            Destroy(buttonGroup.GetChild(i).gameObject);
        }

        
    }

}
