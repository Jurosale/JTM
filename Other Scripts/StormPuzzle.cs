using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormPuzzle : MonoBehaviour {

    public GameObject water;
    public GameObject waterline;
    public GameObject waterTrigger;

    WaterFX waterEffect;
    BuoyancyEffector2D waterBounce;

    GameObject speechObj;
    SpeechRecognition01 speech;

    public GameObject storm;
    bool stormStarted;

	// Use this for initialization
	void Start () {

        speechObj = GameObject.Find("SpeechRecognition");
        speech = speechObj.GetComponent<SpeechRecognition01>();

        waterEffect = water.GetComponent<WaterFX>();
        waterBounce = waterline.GetComponent<BuoyancyEffector2D>();

        stormStarted = false;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (waterTrigger.GetComponent<LightningTrigger>().triggered)
        {
            if (!stormStarted)
            {
                print("Storm starting");
                StartCoroutine("MakeStormy");
                stormStarted = true;
            }
        }
        else
        {
            if (stormStarted)
            {
                print("Storm stopping");
                StartCoroutine("MakeCalm");
                stormStarted = false;
            }
        }

		
	}

    IEnumerator MakeStormy()
    {
        int stormCounter = 0;
        //waterEffect.m_waterSpeed1 = .8f;
        //waterEffect.m_waterSpeed2 = .4f;
        StartCoroutine("ChoppyWater");

        while (stormCounter < 1000)
        {
            waterEffect.m_distorsionAmount += .001f;
            stormCounter++;
            yield return null;
        }
    }

    IEnumerator MakeCalm()
    {
        int stormCounter = 0;
        //waterEffect.m_waterSpeed1 = .4f;
        //waterEffect.m_waterSpeed2 = .2f;
        StopCoroutine("ChoppyWater");

        while (stormCounter < 1000)
        {
            waterEffect.m_distorsionAmount -= .001f;

            stormCounter++;
            yield return null;
        }
    }

    IEnumerator ChoppyWater()
    {
        int waterCounter = 0;

        while (waterCounter < 10)
        {
            waterBounce.density += .1f;
            waterCounter++;
            yield return null;
        }

        if (waterCounter >= 10)
        {
            while (waterCounter > 0)
            {
                waterBounce.density -= .1f;
                waterCounter--;

                if (waterCounter == 0)
                {
                    StartCoroutine("ChoppyWater");
                }
                yield return null;
            }
        }
    }

}
