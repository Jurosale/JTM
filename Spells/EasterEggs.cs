using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggs : MonoBehaviour {

    SpeechRecognition01 speech;
    GameObject Vivi;

    public GameObject sheep,whale,krill;
    float pickTimer;

    bool Cute, castCute, Cast;
    float timer;

    //Inkan: For the giggle sound effect for cute
    FMOD.Studio.EventInstance giggleEvent;
    public string giggleSoundPath = "event:/Voice Overs/VO_Vivi_Giggle";

    //Inkan: For the gasp sound effect for cuss words
    FMOD.Studio.EventInstance cussEvent;
    public string cussSoundPath = "event:/Voice Overs/VO_Vivi_Surprise01";

    // Use this for initialization
    void Start () {
        //Cute = false;
        Vivi = GameObject.Find("Witch character");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        //Inkan: Start instance for giggle sound
        giggleEvent = FMODUnity.RuntimeManager.CreateInstance(giggleSoundPath);

    }

    // Update is called once per frame
    void Update () {
        pickTimer += Time.deltaTime;
        //If player casts "Cute"
        //Vivi.GetComponent<Animator>().SetBool("Cute", Cute);
        //Cute = false;
        if (speech.word == "cute")
        {
            castCute = true;
            //Inkan:Play the sound
            FMODUnity.RuntimeManager.PlayOneShot(giggleSoundPath);

            timer = 0.0f;
        }
        if (castCute == true)
        {
            Vivi.GetComponent<Animator>().SetBool("Cute", true);
            Cute = true;
            timer += Time.deltaTime;
            Debug.Log("you casted cute: " + timer);
            if (timer > 3)
            {
                Vivi.GetComponent<Animator>().SetBool("Cute", false);
                Cute = false;
                castCute = false;
            }

        }
        //Sheep particle
        if (speech.word == "sheep")
        {
            //GameObject tmp = Instantiate(sheep, Vivi.transform.position + new Vector3 (0, -2.0f, 0), Vivi.transform.rotation);
            //Destroy(tmp, 17.0f);
            CreateSheep();
        }
        if (speech.word == "whale")
        {
            //GameObject tmp = Instantiate(whale, Vivi.transform.position + new Vector3 (-10f, -4.0f, 0), Vivi.transform.rotation);
            //Destroy(tmp, 13.0f);
            CreateWhale();
        }
        if (speech.word == "krill")
        {
            //GameObject tmp = Instantiate(krill, Vivi.transform.position + new Vector3(0, 5.0f, 0), Vivi.transform.rotation);
            //Destroy(tmp, 10.0f);
            CreateKrill();
        }
        if (speech.word == "easter")
        {
            if ((int)pickTimer % 3 == 0)
            {
                //Inkan:Play the sound
                FMODUnity.RuntimeManager.PlayOneShot(cussSoundPath);
                CreateSheep();
            }
            else if ((int)pickTimer % 3 == 1)
            {
                //Inkan:Play the sound
                FMODUnity.RuntimeManager.PlayOneShot(cussSoundPath);
                CreateWhale();
            }
            else if ((int)pickTimer % 3 == 2)
            {
                //Inkan:Play the sound
                FMODUnity.RuntimeManager.PlayOneShot(cussSoundPath);
                CreateKrill();
            }
        }
    }

    void CreateSheep()
    {
        GameObject tmp = Instantiate(sheep, Vivi.transform.position + new Vector3(0, -2.0f, 0), Vivi.transform.rotation);
        Destroy(tmp, 17.0f);
    }

    void CreateWhale()
    {
        GameObject tmp = Instantiate(whale, Vivi.transform.position + new Vector3(-10f, -4.0f, 0), Vivi.transform.rotation);
        Destroy(tmp, 13.0f);
    }

    void CreateKrill()
    {
        GameObject tmp = Instantiate(krill, Vivi.transform.position + new Vector3(0, 5.0f, 0), Vivi.transform.rotation);
        Destroy(tmp, 10.0f);
    }


}
