using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour {

    public bool canRestore;
    public bool restored;
    bool notified;
    bool check;
    bool timerCheck;
    float timer;

    GameObject speechObj;
    SpeechRecognition01 speech;
    Teleporter tp;

    public GameObject travelNode;
    public GameObject teleportPoint;

	// Use this for initialization
	void Start () {

        speechObj = GameObject.Find("SpeechRecognition");
        tp = GameObject.Find("NPC_Teleporter").GetComponent<Teleporter>();
        speech = speechObj.GetComponent<SpeechRecognition01>();
        GetComponent<RootAnim>().enabled = false;
        notified = false;
        check = true;
        timerCheck = true;
        //canRestore = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (timerCheck)
        {
            timer += Time.unscaledDeltaTime;
            if (timer >= 2f)
            {
                tp.UpdateTeleportPoints(teleportPoint);
                timerCheck = false;
            }
        }
        if (restored)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.green;
            GetComponent<RootAnim>().enabled = true;

            if (!notified)
            {
                GameObject.Find("Witch character").GetComponent<AvatarSpells>().roots++;
                Debug.Log("Root Restored");
                travelNode.transform.parent = transform;
                notified = true;
            }

        }
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (canRestore)
            {
                if (speech.word == "restore" && check)
                {
                    restored = true;
                    tp.RemoveTeleportPoints(teleportPoint);
                    Destroy(teleportPoint);
                    //GameObject.Find("Avatar").GetComponent<RemoveTeleport>().destroyObject(teleportPoint);
                    check = false;
                }
            }
        }
    }
}
