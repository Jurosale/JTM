//tell pat to comment this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HedgeSection : MonoBehaviour {

    SpeechRecognition01 speech;
    ParticleSystem particles;
    Level2Hints lv2H;

    ParticleSystem torch;

    public GameObject[] adjacentHedge;

    public bool shitsOnFire;

	// Use this for initialization
	void Start () {

        particles = GetComponent<ParticleSystem>();
        particles.Stop();

        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        lv2H = GameObject.Find("Puzzle and Textures").GetComponent<Level2Hints>();
        torch = GameObject.Find("torch_fire").GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {

            if (speech.word == "fire")
            {
                torch.Play();
                particles.Play();
                shitsOnFire = true;
                StartCoroutine("Burning");
                lv2H.ShitsOnFire = true;
            }

            if (speech.word == "wind" && shitsOnFire)
            {
                StopCoroutine("Burning");
                shitsOnFire = true;
                lv2H.ShitsOnFire = true;
                transform.parent.gameObject.GetComponent<IvyHedge>().StartCoroutine("AllBurning");
            }

        }
    }

    public IEnumerator SmallBurn()
    {
        print("smallburn");
        shitsOnFire = true;
        particles.startSize = .5f;
        particles.Play();
        yield return new WaitForSeconds(15f);
        particles.startSize = 1f;
        particles.Stop();
        shitsOnFire = false;
        lv2H.ShitsOnFire = false;
    }

    IEnumerator Burning()
    {
        foreach(GameObject hedge in adjacentHedge)
        {
            print("starting");
            hedge.GetComponent<HedgeSection>().StartCoroutine("SmallBurn");
        }

        yield return new WaitForSeconds(5f);
        particles.startSize = .5f;
        yield return new WaitForSeconds(5f);
        particles.startSize = 1f;
        shitsOnFire = false;
        lv2H.ShitsOnFire = false;
        particles.Stop();

    }
}
