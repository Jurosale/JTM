using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour {

    public GameObject lightningOne;
    public GameObject lightningTwo;

    bool flashingOn;

    public GameObject[] clouds;
    public GameObject[] waves;

    public GameObject lightningTrigger;
    LightningTrigger trigger;

    int lightningCounter;

    string thunderPath = "event:/Other SFX/Thunder";
    FMOD.Studio.EventInstance thunder;

    string stormPath = "event:/Other SFX/Storm";
    FMOD.Studio.EventInstance storm;

    bool soundOn;

    // Use this for initialization
    void Start () {

        lightningCounter = 0;
        thunder = FMODUnity.RuntimeManager.CreateInstance(thunderPath);
        storm = FMODUnity.RuntimeManager.CreateInstance(stormPath);

        trigger = lightningTrigger.GetComponent<LightningTrigger>();

        flashingOn = true;
        soundOn = false;
	}
	
	// Update is called once per frame
	void Update () {

        lightningCounter++;

        if (flashingOn)
        {
            if (lightningCounter % 240 == 0)
            {
                StartCoroutine("Flicker", lightningOne);
            }

            if (lightningCounter % 500 == 0)
            {
                StartCoroutine("Flicker", lightningTwo);

                if (trigger.triggered)
                {
                    if (!soundOn)
                    {
                        StartCoroutine("StormUp");
                        soundOn = true;
                    }
                }
                else if (!trigger.triggered)
                {
                    if (soundOn)
                    {
                        ClearUp();
                        soundOn = false;
                    }
                }
                
            }
        }

    }

    public void ClearUp()
    {
        flashingOn = false;
        lightningOne.SetActive(false);
        lightningTwo.SetActive(false);
        StartCoroutine("Clear");

    }

    IEnumerator Flicker(GameObject lightning)
    {

        lightning.SetActive(true);
        yield return new WaitForSeconds(.05f);
        lightning.SetActive(false);
        yield return new WaitForSeconds(.05f);

        lightning.SetActive(true);
        yield return new WaitForSeconds(.05f);
        lightning.SetActive(false);
        yield return new WaitForSeconds(.05f);

    }

    IEnumerator Clear()
    {
        float clearCounter = 1;
        while (clearCounter > 0)
        {
            storm.setVolume(clearCounter);
            clearCounter -= .01f;

            yield return null;
        }

        storm.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    IEnumerator StormUp()
    {
        float clearCounter = 0;
        while (clearCounter < 1)
        {
            storm.setVolume(clearCounter);
            clearCounter += .01f;

            yield return null;
        }


    }
}

