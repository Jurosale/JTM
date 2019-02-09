using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledWater_SpiritPuzzle : MonoBehaviour {

     
    private bool waterFlag;
    SpeechRecognition01 speech;
    Rigidbody2D debris1, debris2, debris; 
    GameObject house;
    float timer;
    // Use this for initialization
    void Start () {
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        debris = GameObject.Find("Debris").GetComponent<Rigidbody2D>();
        debris1 = GameObject.Find("Debris_1").GetComponent<Rigidbody2D>();
        debris2 = GameObject.Find("Debris_2").GetComponent<Rigidbody2D>();
        house = GameObject.Find("House");
        GameObject.Find("water4").GetComponent<UniversalSpells>().restoreOverride = true;
        GetComponent<SummonSpells>().canWater = false;
        debris.simulated = false;
        debris1.simulated = false;
        debris2.simulated = false;

        GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        GetComponent<BoxCollider2D>().enabled = false;
        waterFlag = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(house.transform.localScale.y == 0 && !waterFlag)
        {
            GetComponent<SummonSpells>().canWater = true;
            //GetComponent<UniversalSpells>().canFire = true;
            //TURN ON DEBRIS STIMULATED
            debris.simulated = true;
            debris1.simulated = true;
            debris2.simulated = true;
            GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GetComponent<BoxCollider2D>().enabled = true;
            waterFlag = true;
        }

        else if (house.transform.localScale.y != 0 && waterFlag)
        {
            Start();
        }
	}

    // When Vivi gets close enough to object...
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //...If the player casted water, will rise the water up
            if (GetComponent<SummonSpells>().canWater && speech.word == "water")
            {
                speech.word = "water";
                timer += Time.deltaTime;
                if (timer > 4f)
                {
                    timer = 0f;
                    speech.word = "";
                }
            }
        }
    }
}
