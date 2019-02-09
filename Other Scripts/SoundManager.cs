using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public GameObject movementObject;
    Movement movement;

    public string wavePath;
    public string stormPath;
    public string splashPath;

    FMOD.Studio.EventInstance waves;
    FMOD.Studio.EventInstance storm;
    FMOD.Studio.EventInstance splash;



    // Use this for initialization
    void Start () {

        movement = movementObject.GetComponent<Movement>();

        wavePath = "event:/Other SFX/LappingWaves";
        stormPath = "event:/Other SFX/Storm";
        splashPath = "event:/Other SFX/Splash";

        waves = FMODUnity.RuntimeManager.CreateInstance(wavePath);
        storm = FMODUnity.RuntimeManager.CreateInstance(stormPath);
        splash = FMODUnity.RuntimeManager.CreateInstance(splashPath);

        waves.start();


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeFootsteps(string path)
    {
        movement.ResetFootstepSound(path);
    }

    public void FadeOutWaves()
    {
        StartCoroutine("FadeOut", waves);
    }

    public void FadeInWaves()
    {
        StartCoroutine("FadeIn", waves);
    }

    public void FadeOutStorm()
    {
        StartCoroutine("FadeOut", storm);
    }

    public void FadeInStorm()
    {
        StartCoroutine("FadeIn", storm);
    }

    IEnumerator FadeOut(FMOD.Studio.EventInstance sound)
    {
        float volumeControl = 1;

        while (volumeControl > 0)
        {
            volumeControl -= .01f;
            volumeControl = 0f;
            sound.setVolume(volumeControl);

            yield return null;
        }
    }

    IEnumerator FadeIn(FMOD.Studio.EventInstance sound)
    {
        float volumeControl = 0;

        while (volumeControl <= 1)
        {
            volumeControl += .01f;
            sound.setVolume(volumeControl);

            yield return null;
        }
    }

}
