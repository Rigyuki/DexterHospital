using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DelayClick : MonoBehaviour
{

	//private int CountDownTime ;

	public Text propTxt;
	private int propAmount = 2;//抑制剂个数

	//public Image cooldownIamge;

	//bool isReady = true;



	private float currentTime;
	public Image coolingImage;
	private float coolingTimer;



	public GameObject enemy1;
	public GameObject enemy2;

	
	
	void Start()
	{
		Button btn = this.GetComponent<Button>();
		 
		btn.onClick.AddListener(OnClick);
		/*CountDownTime = 3;

        cooldownIamge.fillAmount = 1f;*/






		coolingImage.raycastTarget = false;
	}

	private void Update()
	{
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


		//UpdateImage();
	}

	private void OnClick()
	{


		/*health -= 20;

		propAmount--;
		propTxt.text = propAmount.ToString();*/
		propAmount--;
		propTxt.text = propAmount.ToString();

		coolingTimer = 4f; //技能冷却时间
		currentTime = 0.0f;
		coolingImage.fillAmount = 1.0f;


        switch (propAmount)
        {
			case 1:
				Destroy(enemy1);
				break;
			case 0:
				
				Destroy(enemy2);
				break;

            default:
                break;
        }


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
