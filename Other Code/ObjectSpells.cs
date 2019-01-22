using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Checks for spells that directly
 *      affect the objects in scene
 *      Current Spells: Wind
*********************************************** */

public class ObjectSpells : MonoBehaviour {

    public GameObject WindParticlesL, WindParticlesR;
    //If true then this object is affected by that spell otherwise it isn't
    public bool canWind;

    //these values are used to individualize the objects for specific use
    public float windSpeed;

    private float posX, posY, posZ, dimX, dimY, dimZ;
    private bool animFlag;

    SpeechRecognition01 speech;
    GameObject vivi, wind1, wind2;
    UIHistory uiH;

    //Wind Animation
    //public GameObject windAni;

    //timer
    public float timer;

    // Keeps track of initial position and dimensions for revival purposes
    void Start () {
        posX = transform.localPosition.x;
        posY = transform.localPosition.y;
        posZ = transform.localPosition.z;
        dimX = transform.localScale.x;
        dimY = transform.localScale.y;
        dimZ = transform.localScale.z;
        animFlag = false;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        vivi = GameObject.Find("Witch character");
        wind1 = GameObject.Find("WindAnim");
        wind2 = GameObject.Find("WindAnim2");
        WindParticlesL = GameObject.Find("WindParticlesLeft");
        WindParticlesR = GameObject.Find("WindParticlesRight");
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
    }
	
	// When Vivi gets close enough to object...
	void OnTriggerStay2D (Collider2D other) {
        if (other.tag == "Player")
        {
            //...If it can be affected by wind and the player casted wind, will move object
            if (canWind && (speech.word == "wind"|| uiH.isWind))
            {
                if (timer > .5f) { uiH.isWind = false; }
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), 
                    Time.deltaTime * (windSpeed * vivi.GetComponent<Movement>().facingRight) * speech.pitch);

            }
        }
    }

    void FixedUpdate() {
        if((speech.word == "wind" || uiH.isWind))
        {
            if (timer > .5f) { uiH.isWind = false; }
            //Plays animation either left or right depending on what direction she is facing
            if (vivi.GetComponent<Movement>().facingRight >= 0) {
                wind1.transform.position = new Vector3(vivi.transform.position.x + (4f * vivi.GetComponent<Movement>().facingRight),
                    vivi.transform.position.y, vivi.transform.position.z);
                WindParticlesR.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                WindParticlesR.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
            else
            {
                wind2.transform.position = new Vector3(vivi.transform.position.x + (4f * vivi.GetComponent<Movement>().facingRight),
                    vivi.transform.position.y, vivi.transform.position.z);
                WindParticlesL.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                WindParticlesL.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
            //timer = 0;
            animFlag = true;
            //Debug.Log("1111111");
        }

        //Times how long the game object will appear
        if (animFlag) { timer += Time.deltaTime; }

        //once timer passes than move the object out of the way
        if (timer > 2.5f)
        {
            //Reset position and timer
            wind1.transform.position = new Vector3(transform.position.x + 500.0f, transform.position.y + 500.0f, transform.position.z + 500.0f);
            wind2.transform.position = new Vector3(transform.position.x + 500.0f, transform.position.y + 500.0f, transform.position.z + 500.0f);
            timer = 0;
            animFlag = false;    
        }
    }

}
