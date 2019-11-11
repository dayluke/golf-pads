using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour {

	new AudioSource audio;
	public AudioClip creditCollect;

	void Start()
	{
		audio = GameObject.Find("Ball").GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D(Collider2D coll){
		coll = GameObject.FindGameObjectWithTag ("Ball").GetComponent<BallScript> ().GetComponent<Collider2D> ();
		Destroy (this.gameObject);
		audio.clip = creditCollect;
		audio.Play ();
		GameObject.Find("Ball").GetComponent<BallScript> ().SendMessage("AddCredits");
	}
}
