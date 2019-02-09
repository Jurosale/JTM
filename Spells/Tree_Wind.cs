using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Wind : MonoBehaviour {

    public WindSpell windSpell;
    public GameObject vivi;
    private Animator treeAnim;

	// Use this for initialization
	void Start () {
        windSpell = GameObject.Find("WindAnim").GetComponent<WindSpell>();
        vivi = GameObject.Find("Witch character");
        treeAnim = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(vivi.transform.position.x - gameObject.transform.position.x) <= 30f)
        {
            treeAnim.SetBool("wind", windSpell.treeFlag);
        }
	}
}
