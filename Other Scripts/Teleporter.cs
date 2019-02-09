using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour {

    GameObject goddess, avatar;
    SpeechRecognition01 speech;
    Vector3 initSize;
    Root finalRoot;

    bool check;
    bool canTeleport;

    List<GameObject> points;
    
    // Use this for initialization
    void Start () {
        goddess = GameObject.Find("NPC_Goddess");
        avatar = GameObject.Find("Witch character");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        finalRoot = GameObject.Find("Whale_Puzzle").GetComponentInChildren<Root>();
        initSize = gameObject.transform.localScale;
        gameObject.transform.localScale = Vector3.zero;
        check = true;
        points = new List<GameObject>();
        canTeleport = false;
    }
	
	// Update is called once per frame
    //turns on the spirit at the end of level 3 when player restores all roots
    //lets player teleport to goddess
	void Update () {
        if (check && finalRoot.restored)
        {
            check = false;
            canTeleport = true;
            gameObject.transform.localScale = initSize;
            gameObject.transform.position = new Vector3(avatar.transform.position.x + 1f, avatar.transform.position.y, avatar.transform.position.z);
        }

		if (speech.word == "home" && avatar.GetComponent<AvatarSpells>().roots >= 5)
        {
            speech.word = "";
            avatar.transform.position = new Vector3(goddess.transform.position.x - 2f, goddess.transform.position.y, goddess.transform.position.z);
        }

        if (speech.word == "teleport" && points.Count > 0 && canTeleport)
        {
            avatar.transform.position = points[Random.Range(0, points.Count - 1)].transform.position;
        }
    }

    //removes teleport points as player restores more roots
    public void UpdateTeleportPoints(GameObject teleportPoint)
    {
        points.Add(teleportPoint);
    }

    public void RemoveTeleportPoints(GameObject teleportPoint)
    {
        gameObject.transform.position = teleportPoint.transform.position;
        points.Remove(teleportPoint);
        if (points.Count > 0)
        {
            gameObject.GetComponent<NPCSpells>().spiritMessage[0] = "Thank you for all you've done, but you're still missing some roots.";
            gameObject.GetComponent<NPCSpells>().spiritMessage[1] = "Cast \"Teleport\" to return to another root.";
        }

        else
        {
            gameObject.GetComponent<NPCSpells>().spiritMessage[0] = "You've restored all the roots...just like the goddess knew you would.";
            gameObject.GetComponent<NPCSpells>().spiritMessage[1] = "Cast \"Home\" to return to the goddess. Hurry there's no time to waste.";
        }
    }
}
