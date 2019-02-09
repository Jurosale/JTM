using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IvyHedge : MonoBehaviour {

    public GameObject goodie;
    public GameObject sparkle;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator AllBurning()
    {

        foreach(Transform hedge in transform)
        {
            if (hedge.tag == "Hedge")
                hedge.gameObject.GetComponent<ParticleSystem>().Play();
        }

        goodie.GetComponent<Rigidbody2D>().gravityScale = .3f;
        goodie.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, .01f, 0), ForceMode2D.Impulse);
        goodie.GetComponent<SpriteRenderer>().sortingOrder = 5;
        sparkle.GetComponent<SpriteRenderer>().sortingOrder = 6;

        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
