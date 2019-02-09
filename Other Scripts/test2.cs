using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour {

    //Ambience
    FMOD.Studio.EventInstance ambienceEvent;
    public string ambiencePath = "event:/Ambience/AMB_Crow_HumanCity";
    int ambienceGoal;
    int ambienceCounter;

    public GameObject fire_obj;
	public GameObject water_obj;
	public GameObject wind_obj;
	public GameObject shrink_obj;
	public GameObject grow_obj;
	public GameObject astral_obj;
	public GameObject restore_obj;
    public GameObject hint_obj;
    public GameObject earth_obj;
    public GameObject ice_obj;

	public float timer;

    SpeechRecognition01 speech;
    UIHistory uiH;
    AvatarSpells ava;

	// Use this for initialization
	void Start () {
		timer = 0;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        ava = GameObject.Find("Witch character").GetComponent<AvatarSpells>();

        ambienceEvent = FMODUnity.RuntimeManager.CreateInstance(ambiencePath);

        ambienceCounter = 0;
        ambienceGoal = (int)Random.Range(3600f, 4800f);
        ambienceEvent.start();

    }
	
	// Update is called once per frame
    //turns on sounds for specific spell
	void Update () {

        ambienceCounter++;
        if (ambienceCounter >= ambienceGoal)
        {
            ambienceEvent.start();
            ambienceGoal = (int)Random.Range(3600f, 4800f);
            ambienceCounter = 0;
        }

        if (uiH.isSound)
        {
            //Fire
            if (fire_obj.activeInHierarchy == true)
                timer += Time.deltaTime;
            if (uiH.isFire && fire_obj.activeInHierarchy == false)
            {
                fire_obj.SetActive(true);
            }
            //Water
            if (water_obj.activeInHierarchy == true)
                timer += Time.deltaTime;
            if (uiH.isWater && water_obj.activeInHierarchy == false)
            {
                water_obj.SetActive(true);
            }
            //Wind
            if (wind_obj.activeInHierarchy == true)
                timer += Time.deltaTime;
            if (uiH.isWind && wind_obj.activeInHierarchy == false)
            {
                wind_obj.SetActive(true);
            }
            //Shrink
            if (shrink_obj.activeInHierarchy == true)
                timer += Time.deltaTime;
            if (ava.isShrink && shrink_obj.activeInHierarchy == false)
            {

                shrink_obj.SetActive(true);
            }
            //Grow
            if (grow_obj.activeInHierarchy == true)
                timer += Time.deltaTime;
            if (ava.isGrow && grow_obj.activeInHierarchy == false)
            {
                grow_obj.SetActive(true);
            }
            //Astral
            if (astral_obj.activeInHierarchy == true)
                timer += Time.deltaTime;
            if (speech.word == "astral" && astral_obj.activeInHierarchy == false)
            {
                astral_obj.SetActive(true);
            }
            //Restore
            if (restore_obj.activeInHierarchy == true)
                timer += Time.deltaTime;
            if (uiH.isRestore && restore_obj.activeInHierarchy == false)
            {
                restore_obj.SetActive(true);
            }
            //Hint (Inky add)
            if (hint_obj.activeInHierarchy == true)
            {
                timer += Time.deltaTime;
            }
            if (uiH.isHint && hint_obj.activeInHierarchy == false)
            {
                hint_obj.SetActive(true);
            }
            //Earth (Inky add)
            if (earth_obj.activeInHierarchy == true)
            {
                timer += Time.deltaTime;
            }
            if (uiH.isEarth && earth_obj.activeInHierarchy == false)
            {
                earth_obj.SetActive(true);
            }
            //Ice (Inky add)
            if (ice_obj.activeInHierarchy == true)
            {
                timer += Time.deltaTime;
            }
            if (uiH.isIce && ice_obj.activeInHierarchy == false)
            {
                ice_obj.SetActive(true);
            }

            ////Fire
            //if (fire_obj.activeInHierarchy == true)
            //    timer += Time.deltaTime;
            //if (speech.word == "fire" && fire_obj.activeInHierarchy == false)
            //{
            //    fire_obj.SetActive(true);
            //}
            ////Water
            //if (water_obj.activeInHierarchy == true)
            //    timer += Time.deltaTime;
            //if (speech.word == "water" && water_obj.activeInHierarchy == false)
            //{
            //    water_obj.SetActive(true);
            //}
            ////Wind
            //if (wind_obj.activeInHierarchy == true)
            //    timer += Time.deltaTime;
            //if (speech.word == "wind" && wind_obj.activeInHierarchy == false)
            //{
            //    wind_obj.SetActive(true);
            //}
            ////Shrink
            //if (shrink_obj.activeInHierarchy == true)
            //    timer += Time.deltaTime;
            //if (speech.word == "shrink" && shrink_obj.activeInHierarchy == false)
            //{

            //    shrink_obj.SetActive(true);
            //}
            ////Grow
            //if (grow_obj.activeInHierarchy == true)
            //    timer += Time.deltaTime;
            //if (speech.word == "grow" && grow_obj.activeInHierarchy == false)
            //{
            //    grow_obj.SetActive(true);
            //}
            ////Astral
            //if (astral_obj.activeInHierarchy == true)
            //    timer += Time.deltaTime;
            //if (speech.word == "astral" && astral_obj.activeInHierarchy == false)
            //{
            //    astral_obj.SetActive(true);
            //}
            ////Restore
            //if (restore_obj.activeInHierarchy == true)
            //    timer += Time.deltaTime;
            //if (speech.word == "restore" && restore_obj.activeInHierarchy == false)
            //{
            //    restore_obj.SetActive(true);
            //}
            ////Hint (Inky add)
            //if (hint_obj.activeInHierarchy == true)
            //{
            //    timer += Time.deltaTime;
            //}
            //if (speech.word == "hint" && hint_obj.activeInHierarchy == false)
            //{
            //    hint_obj.SetActive(true);
            //}
            ////Earth (Inky add)
            //if (earth_obj.activeInHierarchy == true)
            //{
            //    timer += Time.deltaTime;
            //}
            //if (speech.word == "earth" && earth_obj.activeInHierarchy == false)
            //{
            //    earth_obj.SetActive(true);
            //}
            ////Ice (Inky add)
            //if (ice_obj.activeInHierarchy == true)
            //{
            //    timer += Time.deltaTime;
            //}
            //if (speech.word == "ice" && ice_obj.activeInHierarchy == false)
            //{
            //    ice_obj.SetActive(true);
            //}
        }

        //Reset
        if (timer > 0.5) {
			fire_obj.SetActive (false);
			water_obj.SetActive (false);
			wind_obj.SetActive (false);
			shrink_obj.SetActive (false);
			grow_obj.SetActive (false);
			astral_obj.SetActive (false);
			restore_obj.SetActive (false);
            hint_obj.SetActive(false);
            earth_obj.SetActive(false);
            ice_obj.SetActive(false);
            uiH.isSound = false;
			timer = 0;

            //fixes a restore sound bug
            if (uiH.isRestore)
            {
                uiH.isRestore = false;
            }
		}
	}
}
