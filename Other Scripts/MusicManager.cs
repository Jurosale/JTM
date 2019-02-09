//tell pat to comment this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public GameObject speechRecObj;
    public GameObject soundEffects;
    public bool levelThree;
    SpeechRecognition01 speechRecognition;

    //Background music
    FMOD.Studio.EventInstance backgroundMusicEvent;
    public string backgroundMusicPath;

    //For the prayer song
    FMOD.Studio.EventInstance prayerMusicEvent;
    public string prayerMusicPath = "event:/BGM/MUS_SpiritPrayer";

    //For the Casting SFX
    FMOD.Studio.EventInstance castingEvent;
    public string castingPath = "event:/Other SFX/Casting";

    //For polling the state of the current track
    private FMOD.Studio.PLAYBACK_STATE musicPlaybackState;

    //For polling the state of castingSFX
    private FMOD.Studio.PLAYBACK_STATE castingPlaybackState;

    public bool playingPrayerMusic;
    bool prayerMusicOneShot;
    float volumeCounter;

    bool casting;

    // Use this for initialization
    void Start () {

        speechRecObj = GameObject.Find("SpeechRecognition");
        speechRecognition = speechRecObj.GetComponent<SpeechRecognition01>();

        backgroundMusicEvent = FMODUnity.RuntimeManager.CreateInstance(backgroundMusicPath);
        prayerMusicEvent = FMODUnity.RuntimeManager.CreateInstance(prayerMusicPath);
        castingEvent = FMODUnity.RuntimeManager.CreateInstance(castingPath);

        backgroundMusicEvent.setVolume(1);
        backgroundMusicEvent.start();

        playingPrayerMusic = false;
        prayerMusicOneShot = false;
        volumeCounter = 1;

        casting = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (speechRecognition.castOn)
        {
            //print("Casting???");
            if (!casting)
            {
                StartCoroutine("FadeMusic", false);
                StartCoroutine("CastingSFX");
                casting = true;
            }
        }
    
        if (casting)
        {

            if (!speechRecognition.castOn)
            {
                castingEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                StopCoroutine("FadeMusic");
                StopCoroutine("CastingSFX");
                StartCoroutine("FadeMusic", true);

                casting = false;
            }

        }

        if (playingPrayerMusic)
        {
            if (!prayerMusicOneShot)
            {
                PrayerMusic();
                prayerMusicOneShot = true;
            }

            backgroundMusicEvent.setVolume(0f);
        }
	}

    public void FadeWaves()
    {
        soundEffects.GetComponent<SoundManager>().FadeOutWaves();
    }

    public void PrayerMusic()
    {
        if (!prayerMusicOneShot)
        {
            print("Prayer Music begin");
            playingPrayerMusic = true;

            StartCoroutine("PrayerMusicStart");
            prayerMusicOneShot = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //StartCoroutine("FadeMusic", false);
            StartCoroutine("KillMusic");
        }
    }

    IEnumerator PrayerMusicStart()
    {
        prayerMusicEvent.start();
        //StartCoroutine("FadeMusic", false);
        yield return new WaitForSeconds(18f);
        playingPrayerMusic = false;
        prayerMusicOneShot = false;
        StartCoroutine("FadeMusic", true);


    }

    IEnumerator FadeMusic(bool fadeIn)
    {
        if (fadeIn)
        {
            int counter = 0;
            float parameterValue = 0;
            while (counter <= 100)
            {

                parameterValue += .01f;
                //backgroundMusicEvent.setParameterValue("BackgroundMusicFadeOut", parameterValue);
                backgroundMusicEvent.setVolume(parameterValue);
                counter++;
                yield return null;

            }

        }
        else
        {
            print("Fading Out");
            int counter = 100;
            float parameterValue = 1;
            while (counter >= 0)
            {

                parameterValue -= .01f;
                //backgroundMusicEvent.setParameterValue("BackgroundMusicFadeOut", parameterValue);
                backgroundMusicEvent.setVolume(parameterValue);
                counter--;
                yield return null;

            }

        }
    }
    
    IEnumerator KillMusic()
    {
        backgroundMusicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        yield return null;
    }

    IEnumerator CastingSFX()
    {

        castingEvent.start();

        yield return null;


    }
}
