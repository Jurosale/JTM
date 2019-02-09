using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Checks for spells that require 
 *      creating a new object
 *      Current Spells: Earth, Water, Ice
*********************************************** */

public class SummonSpells : MonoBehaviour
{

    public GameObject avatar;
    public GameObject earthPar;
    public ParticleSystem snowParticles, bubbleParticles;

    //If true then this object is affected by that spell otherwise it isn't

    public bool canEarth;
    public bool canWater;
    public bool canIce;


    //these values are used to individualize the objects for specific use
    public float waterSpeed;
    public float waterHeight;

    private float posX, posY, posZ, dimX, dimY, dimZ, earthY, earthTimer, earthHeight, timer, earthFlagTimer, iceFlagTimer, wftimer, particleTimer;
    private bool earthCreate, iceFlag, earthFlag, waterFlag;
    private Vector4 colorObject;

    SpeechRecognition01 speech;
    GameObject vivi, iceAnim, iceDrops, rainAnim, rainDrops, sparkles, mist;
    UIHistory uiH;

    // Use this for initialization
    void Start()
    {
        posX = transform.localPosition.x;
        posY = transform.localPosition.y;
        posZ = transform.localPosition.z;
        dimX = transform.localScale.x;
        dimY = transform.localScale.y;
        dimZ = transform.localScale.z;

        //height determines how high earth moves and timer determines how long it stays out
        earthHeight = 8f;
        timer = 5f;
        earthCreate = true;
        earthFlagTimer = 0.0f;

        //fire animation stuff
        colorObject = new Vector4(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g,
            GetComponent<SpriteRenderer>().color.b, GetComponent<SpriteRenderer>().color.a);
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        vivi = GameObject.Find("Witch character");
        iceAnim = GameObject.Find("IceAnim");
        iceDrops = GameObject.Find("IceDrops");
        rainAnim = GameObject.Find("WaterAnim");
        rainDrops = GameObject.Find("RainDrops");
        snowParticles = GameObject.Find("SnowParticles").GetComponent<ParticleSystem>();
        bubbleParticles = GameObject.Find("BubbleParticles").GetComponent<ParticleSystem>();
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        wftimer = 0.0f;
    }

    // When Vivi gets close enough to object...
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //...If the player casted water, will rise the water up
            if (canWater && (speech.word == "water" || uiH.isWater))
            {
                rainDrops.GetComponent<ParticleSystem>().Play();
                rainAnim.transform.position = new Vector3(GetComponent<Collider2D>().bounds.center.x, GetComponent<Collider2D>().bounds.max.y + 4f, gameObject.transform.position.z);
                waterFlag = false;
                if (transform.localPosition.y < posY + waterHeight)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x,
						transform.localPosition.y + 2.5f, transform.localPosition.z), Time.deltaTime * waterSpeed * speech.pitch);
                    if (transform.localPosition.y > posY + waterHeight) {
                        transform.localPosition = new Vector3(transform.localPosition.x, posY + waterHeight, transform.localPosition.z);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (!waterFlag)
        {
            wftimer += Time.deltaTime;
            if (wftimer > 1f)
            {
                wftimer = 0f;
                uiH.isWater = false;
            }
        }
    }

    void FixedUpdate()
    {
        particleTimer += Time.deltaTime;
        //Animation for Water Spell
        if ((speech.word == "water" || uiH.isWater) && waterFlag)
        {
            bubbleParticles.Clear();
            bubbleParticles.Play();
            rainDrops.GetComponent<ParticleSystem>().Play();
            rainAnim.transform.position = new Vector3(vivi.transform.position.x + (3.4f * vivi.GetComponent<Movement>().facingRight),
                vivi.transform.position.y + 2.5f, gameObject.transform.position.z);
            
            waterFlag = false;
        }

        //This section creates, raises, and then destroys the earth platform
        if (canEarth) { earthTimer += Time.deltaTime; }
        if (canEarth && (speech.word == "earth" || uiH.isEarth) &&
            (Mathf.Abs(vivi.GetComponent<Rigidbody2D>().velocity.y)) <= .1f)
        {
            if (earthCreate) {
                createEarth();
            }
			if (transform.localPosition.y < earthY + earthHeight) {
                if (particleTimer >= 3.0f)
                {
                    GameObject ep = Instantiate(earthPar, vivi.transform.position - new Vector3(0,1,0), vivi.transform.rotation);
                    Destroy(ep, 2f);
                    particleTimer = 0;
                }
                growEarth(speech.pitch);
            }
            earthTimer = 0f;
            earthFlag = false;
        }

        if (timer <= earthTimer)
        {
            destroyEarth();
        }

        //Will freeze the water if there is any
        if (canIce && (speech.word == "ice" || uiH.isIce))
        {
            uiH.isIce = false;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            GetComponent<SpriteRenderer>().color = colorObject;
            if (Vector2.Distance(avatar.transform.position, transform.position) > 15f) {
                transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
            }
            else if (avatar.GetComponent<Collider2D>().bounds.min.y < transform.position.y)
            {
                if (iceFlag)
                {
                    GetComponent<FMODUnity.StudioEventEmitter>().Play();
                }
                iceDrops.GetComponent<ParticleSystem>().Play();
                iceAnim.transform.position = new Vector3(GetComponent<Collider2D>().bounds.center.x, GetComponent<Collider2D>().bounds.max.y + 4f, gameObject.transform.position.z);
                avatar.transform.position = new Vector3(avatar.transform.position.x, GetComponent<Collider2D>().bounds.max.y, avatar.transform.position.z);
                iceFlag = false;
            }
            else {
                if (iceFlag)
                {
                    GetComponent<FMODUnity.StudioEventEmitter>().Play();
                }
                iceDrops.GetComponent<ParticleSystem>().Play();
                iceAnim.transform.position = new Vector3(GetComponent<Collider2D>().bounds.center.x, GetComponent<Collider2D>().bounds.max.y + 4f, gameObject.transform.position.z);
                iceFlag = false;
            }
        }

        if ((speech.word == "ice" || uiH.isIce) && iceFlag)
        {
            uiH.isIce = false;
            snowParticles.Clear();
            snowParticles.Play();
            if (iceAnim.transform.position.z == 0f)
            {
                iceDrops.GetComponent<ParticleSystem>().Play();
                iceAnim.transform.position = new Vector3(vivi.transform.position.x + (3.4f * vivi.GetComponent<Movement>().facingRight), 
                vivi.transform.position.y + 3f, gameObject.transform.position.z);
                iceFlag = false;
            }
        }

        if (!uiH.isWater && !waterFlag) { waterFlag = true; }
        
        if (speech.word != "ice" && iceFlagTimer > 1.5f) {
            iceFlag = true;
            iceFlagTimer = 0f;
            uiH.isIce = false;
        }
        else if (!iceFlag) { iceFlagTimer += Time.deltaTime; }
        if (speech.word != "earth" && earthFlagTimer > 1.5f) {
            earthFlag = true;
            earthFlagTimer = 0f;
            uiH.isEarth = false;
        }
        else if (!earthFlag) { earthFlagTimer += Time.deltaTime; }
    }

    //spawns the earth object
    void createEarth()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        transform.localPosition = new Vector3(avatar.transform.localPosition.x, avatar.transform.localPosition.y - 12f, avatar.transform.localPosition.z);
        earthY = transform.localPosition.y;
        earthCreate = false;
    }

    //makes the earth object rise as the player casts earth
	void growEarth(float a)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y + earthHeight, transform.localPosition.z), Time.deltaTime * a * 15f);
        if (earthFlag) {
            GetComponent<FMODUnity.StudioEventEmitter>().Play();
        }
    }

    //after a certain time period, earth will gradually fall to the ground and then disappear
    void destroyEarth()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y - earthHeight, transform.localPosition.z), Time.deltaTime * 5f);
        if (transform.localPosition.y <= earthY)
        {
            earthTimer = 0f;
            earthCreate = true;
            transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
        }
        else {
            if (earthFlag) { GetComponent<FMODUnity.StudioEventEmitter>().Play(); }
            earthFlag = false;
        }
    }
}