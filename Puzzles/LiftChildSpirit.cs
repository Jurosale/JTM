using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftChildSpirit : MonoBehaviour {

    public GameObject water, parent;
    private Vector3 pos;
    private bool followFlag;
    private float posP;

    public Sprite happyImage;
    public RuntimeAnimatorController happySpirit;

    SpeechRecognition01 speech;
    UIHistory uiH;
    BurnedHouse_SpiritPuzzle bhsp;

	// Use this for initialization
	void Start () {
        pos = transform.position;
        posP = parent.transform.position.y;
        followFlag = true;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        bhsp = GameObject.Find("House").GetComponent<BurnedHouse_SpiritPuzzle>();
    }
	
	// Update is called once per frame
	void Update () {
        //makes the child spirit float right above the water in sinkhole
        if (followFlag)
        {
            if (water.transform.localScale.y == 0) { transform.position = pos; }
            else { transform.position = new Vector3(transform.position.x, water.GetComponent<Collider2D>().bounds.max.y + 2f, transform.position.z); }
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if player makes child spirit followthem, parent gets happy and can now be talked to
        if (speech.word == "follow" || uiH.isFollow)
        {
            parent.GetComponent<Animator>().runtimeAnimatorController = happySpirit;
            parent.GetComponent<SpriteRenderer>().sprite = happyImage;
            followFlag = false;
            parent.GetComponent<Spirit_Interaction>().enabled = false;
            parent.GetComponent<NPCSpells>().enabled = true;
            bhsp.moveParent = true;
            uiH.isFollow = false;
        }

        //ignore this
        if (speech.word == "prayer")
        {
            //parent.GetComponentInChildren<TextMesh>().text = "Bless your soul!";
        }
    }
}
