using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revivePlants : MonoBehaviour {

    //Speech Recogntion Object
    public GameObject speechRec;

    //dead plant object (1)
    public GameObject deadPlant_1;
    //live plant object (1)
    public GameObject livePlant_1;
    //dead  flower (purple) 1
    public GameObject witheredPurpleFlower1;
    //Live flower (purple) 1
    public GameObject livePurpleFlower1;
    //dead  flower (purple) 2
    public GameObject witheredPurpleFlower2;
    //Live flowe (purple) 2
    public GameObject livePurpleFlower2;

    //dead plant object (2)
    public GameObject deadPlant_2;
    //live plant object (2)
    public GameObject livePlant_2;
    //dead  flower (white) 1
    public GameObject witheredWhiteFlower1;
    //Live flowe (white) 1
    public GameObject liveWhiteFlower1;
    //dead  flower (white) 2
    public GameObject witheredWhiteFlower2;
    //Live flowe (white) 2
    public GameObject liveWhiteFlower2;

    //dead plant object (3)
    public GameObject deadPlant_3;
    //live plant object (3)
    public GameObject livePlant_3;
    //dead  flower (white) 1
    public GameObject witheredRedFlower1;
    //Live flowe (white) 1
    public GameObject liveRedFlower1;
    //dead  flower (white) 2
    public GameObject witheredRedFlower2;
    //Live flowe (white) 2
    public GameObject liveRedFlower2;

    //drop in diary page
    public GameObject diaryPage;

    public GameObject[] travelnodes;

	// Use this for initialization
	void Start () {
        //turn off all the live plants
        livePlant_1.SetActive(false);
        livePlant_2.SetActive(false);
        livePlant_3.SetActive(false);

        //turn off live flowers
        livePurpleFlower1.SetActive(false);
        livePurpleFlower2.SetActive(false);
        liveWhiteFlower1.SetActive(false);
        liveWhiteFlower2.SetActive(false);
        liveRedFlower1.SetActive(false);
        liveRedFlower2.SetActive(false);

        diaryPage.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        //for the first plant: player casts restore/water and the plant is dead and has to be near the plant
		if((speechRec.GetComponent<SpeechRecognition01>().word == "restore" || speechRec.GetComponent<SpeechRecognition01>().word == "water")
            && livePlant_1.activeInHierarchy == false && deadPlant_1.GetComponent<inPlantTrigger>().nearPlant == true)
        {
            //turn off dead Plant
            Destroy(deadPlant_1);
            //turn on live Plant
            livePlant_1.SetActive(true);

            //turn off dead flower
            Destroy(witheredPurpleFlower1);
            Destroy(witheredPurpleFlower2);

            //Turn on live flowers
            livePurpleFlower1.SetActive(true);
            livePurpleFlower2.SetActive(true);
        }

        //for the second plant: player casts restore/water and the plant is dead and has to be near the plant
        if ((speechRec.GetComponent<SpeechRecognition01>().word == "restore" || speechRec.GetComponent<SpeechRecognition01>().word == "water")
            && livePlant_2.activeInHierarchy == false && deadPlant_2.GetComponent<inPlantTrigger>().nearPlant == true)
        {
            //turn off dead Plant
            Destroy(deadPlant_2);
            //turn on live Plant
            livePlant_2.SetActive(true);

            //turn off dead flower
            Destroy(witheredWhiteFlower1);
            Destroy(witheredWhiteFlower2);

            //Turn on live flowers
            liveWhiteFlower1.SetActive(true);
            liveWhiteFlower2.SetActive(true);
        }

        //for the third plant: player casts restore/water and the plant is dead and has to be near the plant
        if ((speechRec.GetComponent<SpeechRecognition01>().word == "restore" || speechRec.GetComponent<SpeechRecognition01>().word == "water")
            && livePlant_3.activeInHierarchy == false && deadPlant_3.GetComponent<inPlantTrigger>().nearPlant == true)
        {
            //turn off dead Plant
            Destroy(deadPlant_3);
            //turn on live Plant
            livePlant_3.SetActive(true);

            //turn off dead flower
            Destroy(witheredRedFlower1);
            Destroy(witheredRedFlower2);

            //Turn on live flowers
            liveRedFlower1.SetActive(true);
            liveRedFlower2.SetActive(true);
        }

        //checs if all the plants are active. If they are drop the diary page
        if (livePlant_1.activeSelf && livePlant_2.activeSelf && livePlant_3.activeSelf)
        {
            foreach (GameObject node in travelnodes)
            {
                node.SetActive(false);
            }
            diaryPage.SetActive(true);
        }
    }
}
