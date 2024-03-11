using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountDown : MonoBehaviour
{
    private int CountDownTime;
    private Text _txt;
    // Start is called before the first frame update
    void Start()
    {
        CountDownTime = 300;
        _txt = transform.Find("Canvas/Panel/Text").GetComponent<Text>();

        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        _txt.text = CountDownTime.ToString();
    }

    IEnumerator CountDown()
    {
        while (CountDownTime > 0)
        {
            CountDownTime = CountDownTime - 1;
            yield return new WaitForSeconds(1);
        }
    }
}
