using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRestore : MonoBehaviour {

    public bool canRestore;
    public float distance;
    private GameObject Lantern0;
    private GameObject Lantern1;
    public GameObject Root;
    private GameObject bb2;
    public GameObject lights;

    public GameObject[] travelnodes;

    private float posX, posY, posZ, dimX, dimY, dimZ;

    private Vector4 colorObject;

    SpeechRecognition01 speech;
    GameObject vivi;

    // Use this for initialization
    void Start () {
        posX = transform.localPosition.x;
        posY = transform.localPosition.y;
        posZ = transform.localPosition.z;
        dimX = transform.localScale.x;
        dimY = transform.localScale.y;
        dimZ = transform.localScale.z;

        transform.localScale = new Vector3(dimX, 0, dimY);
        colorObject = new Vector4(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g,
            GetComponent<SpriteRenderer>().color.b, GetComponent<SpriteRenderer>().color.a);

        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        vivi = GameObject.Find("Witch character");
        Lantern0 = GameObject.Find("lamp_full_noVines_1").transform.GetChild(0).GetChild(0).gameObject;
        Lantern1 = GameObject.Find("lamp_full_noVines_2").transform.GetChild(0).GetChild(0).gameObject;
        Root = GameObject.Find("RootBridge");
        bb2 = GameObject.Find("BrokenBridge2");
        Root.transform.localScale = new Vector3(Root.transform.localScale.x, 0f, Root.transform.localScale.z);
    }
	
	// Update is called once per frame
	void Update () {

        //if player is close enough to bridge, it can be restored
        distance = Mathf.Pow((Camera.main.transform.position.x - gameObject.transform.position.x), 2);
        
        if (distance <= 130)
        {
            canRestore = true;
        }
        else
        {
            canRestore = false;
        }

        //root appears after both laterns are lit up
        if (Lantern0.GetComponent<ParticleSystem>().isPlaying && Lantern1.GetComponent<ParticleSystem>().isPlaying)
        {
            Root.transform.localScale = new Vector3(Root.transform.localScale.x, 0.3f, Root.transform.localScale.z);

            foreach(GameObject node in travelnodes)
            {
                node.SetActive(false);
            }

            lights.SetActive(true);
        }

        //restores bridge is laterns are lit up and player casts restore
        if (canRestore && speech.word == "restore" && Lantern0.GetComponent<ParticleSystem>().isPlaying && Lantern1.GetComponent<ParticleSystem>().isPlaying)
        {
            transform.localScale = new Vector3(dimX, dimY, dimZ);
            transform.localPosition = new Vector3(posX, posY, posZ);
            GetComponent<SpriteRenderer>().color = colorObject;
            bb2.transform.localScale = new Vector3(bb2.transform.localScale.x,0f, bb2.transform.localScale.z);
        }
    }


}
