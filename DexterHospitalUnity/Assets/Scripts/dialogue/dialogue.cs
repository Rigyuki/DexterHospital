using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dialogue : MonoBehaviour
{
    public TextAsset dialogDataFile; //csv格式文本

    public SpriteRenderer sprite1;

    public SpriteRenderer sprite2;
    public List<Sprite> sprites = new List<Sprite>();

    public TMP_Text nameText;
    public TMP_Text dialogText;

    public int dialogIndex;
    public string[] dialogRows;

    Dictionary<string, Sprite> imageDic = new Dictionary<string, Sprite>();

    private void Awake()
    {
        imageDic["0"] = sprites[0];
        imageDic["1"] = sprites[1];
    }
    void Start()
    {
        sprite1.enabled = false;
        sprite2.enabled = false;


        ReadText(dialogDataFile);
        dialogIndex = 0;
        showDialogRow(dialogIndex);
        //UpdateText("莲子", "莲子,要去哪里玩呢");
        //UpdateImage("莲子", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogIndex < dialogRows.Length - 2)
        {
            dialogIndex++;
            showDialogRow(dialogIndex);
        }
    }

    public void UpdateText(string name, string text)
    {
        Debug.Log("UpdateText ");
        nameText.text = name;
        dialogText.text = text;
    }

    public void UpdateImage(string name, int state)
    {
        if (state == 0)
        {
            sprite1.sprite = imageDic[name];
            sprite1.enabled = true;
            sprite2.enabled = false;
        }

        else if (state == 1)
        {
            sprite2.sprite = imageDic[name];
            sprite1.enabled = false;
            sprite2.enabled = true;
        }
    }

    public void ReadText(TextAsset textAsset)
    {
        dialogRows = textAsset.text.Split('\n');
    }

    public void showDialogRow(int dialogIndex)
    {
        string row = dialogRows[dialogIndex];
        string content_copy = "";

        //Debug.Log("row " + row);
        string[] cell = row.Split(',');

        if (cell[0] == "#")
        {
            if (cell[4] == "0")
            {
                UpdateImage(cell[4], 0);
            }

            else if (cell[4] == "1")
            {
                UpdateImage(cell[4], 1);
            }


            content_copy = wordCountString(cell[5]);
            UpdateText(cell[2], content_copy);
            Debug.Log("content_copy " + content_copy);
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
}
