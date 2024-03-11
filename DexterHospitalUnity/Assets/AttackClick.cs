using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AttackClick : MonoBehaviour
{
	public Image hp;
	float health = 100f;
	//private int CountDownTime ;
 
	public Text propTxt;
	private int propAmount=5;

	//public Image cooldownIamge;

	//bool isReady = true;

	public AudioSource bgm;
	public AudioSource bossDamageAudio;


	private float currentTime;  
	public Image coolingImage;
	private float coolingTimer;

	public  SceneFadeMgr _secneFadeMgr;

	void Start()
    {
        Button btn = this.GetComponent<Button>();
        hp.fillAmount = 1f;
        btn.onClick.AddListener(OnClick);

		bgm.Play();
 
		coolingImage.raycastTarget = false;

		_secneFadeMgr = transform.parent.Find("SceneFadeMgr").GetComponent<SceneFadeMgr>();
	}

	private void Update()
	{
		hp.fillAmount = health / 100;        

		if (propAmount > 0)
        {
			UpdateImage();
		}
        else
        {
			coolingImage.fillAmount = 1.0f;
			coolingImage.raycastTarget = true;
			this.GetComponent<Button>().enabled = false;
		}

        if (health == 0)
        {
            // StartCoroutine(_secneFadeMgr.LoadScene("BE"));
            SceneManager.LoadScene("Õæ½á¾Ö");

        }
    }

    private void OnClick()
	{
  

		health -= 20;
		bossDamageAudio.Play();

		propAmount--;
		propTxt.text = propAmount.ToString();

        coolingTimer = 2f;
		currentTime = 0.0f;
		coolingImage.fillAmount = 1.0f;

		Debug.Log(health);

		/*if (health == 0)
		{
			//TODO jump he scene;
			StartCoroutine(_secneFadeMgr.LoadScene("BE"));

			bgm.Pause();
		}*/

	}

	private void UpdateImage()
	{
		if (currentTime < coolingTimer)
		{
			currentTime += Time.deltaTime;
			coolingImage.fillAmount = 1 - currentTime / coolingTimer;
			if (coolingImage.fillAmount != 0)
			{
				coolingImage.raycastTarget = true;
				this.GetComponent<Button>().enabled = false;
			}
			else
			{
				coolingImage.raycastTarget = false;
				this.GetComponent<Button>().enabled = true;
			}
		}
	}
 
  
}
