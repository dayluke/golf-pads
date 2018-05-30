using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadScript : MonoBehaviour {

	float ballVelocity;
	bool shallWeDeductALife;
	Collider2D ballCollider;
	public Collider2D padCollider;
	bool gamePaused;

	// Use this for initialization
	void Start () {
		ballCollider = GameObject.FindGameObjectWithTag ("Ball").GetComponent<Collider2D> ();
		shallWeDeductALife = false;
	}
	
	// Update is called once per frame
	void Update () {

		gamePaused = GameObject.Find ("GameController").GetComponent<GameController> ().paused;

		if (!gamePaused) {
			ballVelocity = GameObject.FindGameObjectWithTag ("Ball").GetComponent<Rigidbody2D> ().velocity.magnitude;

			if (ballVelocity > 0.05f)
				shallWeDeductALife = true;

			if (ballCollider.IsTouching (padCollider) && ballVelocity < 0.05f) {
				Destroy (this.gameObject);
				GameObject.FindGameObjectWithTag ("Ball").GetComponent<BallScript> ().SendMessage ("Scored");
				shallWeDeductALife = true;
			} else if (!ballCollider.IsTouching (padCollider) && ballVelocity < 0.05f && shallWeDeductALife) {
				GameObject.FindGameObjectWithTag ("Ball").GetComponent<BallScript> ().SendMessage ("DeductLives");
				shallWeDeductALife = false;
			}
		}
	}
}
