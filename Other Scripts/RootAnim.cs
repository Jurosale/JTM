using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAnim : MonoBehaviour {

    float initY;

    // Use this for initialization
    void Start () {
        //GameObject.Find("Witch character").GetComponent<AvatarSpells>().roots++;
        initY = transform.position.y;
    }
	
	// Update is called once per frame
    //roots sink to a certain ditance once player restores them
	void Update () {
        if (initY - 15 < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime * 2.5f), transform.position.z);
        }
    }
}
