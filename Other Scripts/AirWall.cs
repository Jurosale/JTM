using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWall : MonoBehaviour {

    public GameObject restoreBlock;
    public GameObject prayerBlock;

	// Use this for initialization
	void Start () {
        restoreBlock = GameObject.Find("RestoreBlock");
        prayerBlock = GameObject.Find("PrayerBlock");
	}
	
	// Update is called once per frame
	void Update () {
        //turns off the invisble walls in intro scene once player completes the restore and follow task respectively
		if (gameObject.GetComponent<NPCSpells>().followAnim == true && restoreBlock.activeInHierarchy)
            restoreBlock.SetActive(false);
        if (gameObject.GetComponent<NPCSpells>().prayAnim == true && prayerBlock.activeInHierarchy)
            prayerBlock.SetActive(false);

    }
}
