using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;



public class DiagalUIMgr : MonoBehaviour
{
    public static DiagalUIMgr Instance { get; private set; }
    public Image _imageBg;
    public Image _imageCharacter1;
    public Image _imageCharacter2;

    private GameObject _imageFile;
    private GameObject _image;
    private GameObject _imageRole;
    private GameObject _videoFile;


    public TMP_Text _textName;
    public TMP_Text _textContent;
    public GameObject _talkTextUI;//对话框ui
    private GameObject _btnNextMgr;//选择ui

    public VideoPlayer videoPlayer;
    public RawImage rawimage;

    public GameObject empChoiceUIGo;//多项选择框父对象
    public GameObject[] choiceUIGos;
    public TMP_Text[] textChoiceUIs;
    private Button _btnNext;

    bool _textfinished;
    int dialogCnt;


    private void Awake()
    {
        Instance = this;
        _imageFile = transform.Find("ImageFile").gameObject;
        _videoFile = transform.Find("videoFile").gameObject;
        _btnNextMgr = transform.Find("BtnNextMgr").gameObject;
        _btnNext = _btnNextMgr.transform.Find("Btn_LoadNext").GetComponent<Button>();
        _imageRole = transform.Find("roleAnimate").gameObject;
        _textfinished = true;
        dialogCnt = 0;
    }

    public Button getButton() {
        return _btnNext;
    }

    public Button getOptionButton()
    {
        return empChoiceUIGo.transform.Find("Button").GetComponent<Button>();
    }

    private void setUI(bool backgroundBoolen, bool charcterBoolen, bool text_UI_boolen, bool imageRoleBoolen, bool imageFileBoolen)
    {
        _imageBg.gameObject.SetActive(backgroundBoolen);
        _imageCharacter1.gameObject.SetActive(charcterBoolen);
        _imageCharacter2.gameObject.SetActive(charcterBoolen);
        _talkTextUI.SetActive(text_UI_boolen);
        _imageRole.SetActive(imageRoleBoolen);
        _imageFile.gameObject.SetActive(imageFileBoolen);
    }

    public void setvideo(string filename)
    {
        setUI(false, false, false, false, false);
        _btnNextMgr.SetActive(false);
        _videoFile.SetActive(true);
        videoPlayer.url = Application.dataPath + "/Resources/Video/" + filename + ".mp4";
        videoPlayer.Play();
        StartCoroutine("WaitForMovieEnd");
    }

    public IEnumerator WaitForMovieEnd()
    {
        while (videoPlayer.isPlaying)
        {
            _btnNextMgr.SetActive(false);
            yield return new WaitForEndOfFrame();

        }
        OnMovieEnded();
    }

    void OnMovieEnded()
    {
        _btnNextMgr.SetActive(true);
    }

    public void setBGImageSpriteOnly(string _spriteBGName)
    {
        setUI(true, false, true, false, false);
        Debug.Log("BG name = " + _spriteBGName);
        _imageBg.sprite = Resources.Load<Sprite>("Sprites/BG/" + _spriteBGName);
    }

    public void setRoleAnimate(string _spriteBGName, string _filename)
    {    
        setUI(true, false, false, true, false);
        _imageBg.sprite = Resources.Load<Sprite>("Sprites/BG/" + _spriteBGName);
        _image = _imageRole.transform.Find(_filename).gameObject;
        _image.SetActive(true);
        setFileRoleImage(_filename);
    }

    public void setBgImageAndFile(string _spriteBGName, string _filename)
    {
        Debug.Log(_spriteBGName + " " + _filename);
        _imageBg.sprite = Resources.Load<Sprite>("Sprites/BG/" + _spriteBGName);
        setUI(true, false, false, false, true);
        setFileImage(_filename);
    }

    public void setFileRoleImage(string _filename)
    {
        foreach (Transform t in _imageRole.transform)
        {
            if (t.name == _filename)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }

        }
    }

    public void setFileImage(string _filename)
    {
        foreach (Transform t in _imageFile.transform)
        {
            if (t.name == _filename)
            {
                t.gameObject.SetActive(true);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
            
        }
    }


    public void ShowChoiceUI(int choiceNum, string[] choiceContent)
    {
        //Debug.Log("choiceNum = " + choiceNum);
        empChoiceUIGo.SetActive(true);
        setUI(true, false, false, false, false);
        _btnNextMgr.SetActive(false);
        for (int i = 0; i < 1; i++)
        {
            choiceUIGos[i].SetActive(false);
        }
        for (int i = 0; i < 1; i++)
        {
            choiceUIGos[i].SetActive(true);
            Debug.Log("choiceNum = " + choiceContent[i]);
            textChoiceUIs[i].text = choiceContent[i];
        }

    }

    public void setCharacterUI(string _spriteBGName, string _npcName1, string _npcName2, string _npcTextName)
    {
        setUI(true, true, true, false, false);
        _imageBg.sprite = Resources.Load<Sprite>("Sprites/BG/" + _spriteBGName);
        _textName.text = _npcTextName;
        if (_npcName2 == "" && _npcName1 != "")
        {
            Debug.Log("_npcName1 = " + _npcName1);
            _imageCharacter1.sprite = Resources.Load<Sprite>("Sprites/Characters/" + _npcName1);
            _imageCharacter1.gameObject.SetActive(true);
            _imageCharacter2.gameObject.SetActive(false);
        }
        else if (_npcName1 == "" && _npcName2 != "")
        {
            Debug.Log("_npcName2 = " + _npcName2);
            _imageCharacter2.sprite = Resources.Load<Sprite>("Sprites/Characters/" + _npcName2);
            _imageCharacter1.gameObject.SetActive(false);
            _imageCharacter2.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("_npcName1 = " + _npcName1 + "_npcName2 = " + _npcName2);
            _imageCharacter1.sprite = Resources.Load<Sprite>("Sprites/Characters/" + _npcName1);
            _imageCharacter1.gameObject.SetActive(true);
            _imageCharacter2.sprite = Resources.Load<Sprite>("Sprites/Characters/" + _npcName2);
            _imageCharacter2.gameObject.SetActive(true);
        }
    }


    public void UpdateDialogContent(string dialogContent)
    {
        _textContent.text = "";
        dialogCnt++;
        StartCoroutine(func(dialogContent, dialogCnt));
    }
    IEnumerator func(string text, int dialogIndex)
    {
        _textfinished = false;
        float speed = 0.1F;
        for (int i = 0; i < text.Length; i++)
        {
            _textContent.text += text[i];
            yield return new WaitForSeconds(speed);
            if (dialogIndex != dialogCnt)
                break;
        }

        _textfinished = true;
    }
}