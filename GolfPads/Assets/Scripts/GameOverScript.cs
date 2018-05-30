using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {
	
	const string URL = "https://www.facebook.com/golfpads";
	int score, bestScore;
	public Text scoreText, bestScoreText;
	public GameObject ball;

	void Start () {
		if (PlayerPrefs.GetInt ("Ads Removed") == 0)
			GameObject.Find ("AdMobController").GetComponent<AdMobScript> ().SendMessage ("showInterstitialAd");
		else
			return;
	}
	
	void Update () {
		
	}

	public void StartGameOverSequence()
	{
		Time.timeScale = 0;
		score = PlayerPrefs.GetInt ("Score");
		scoreText.text = "Score: " + score.ToString ();
		bestScore = PlayerPrefs.GetInt ("Best Score");
		bestScoreText.text = "Best Score: " + bestScore.ToString ();
		ball.SetActive (false);
	}

	public void OnHomeClick()
	{
		SceneManager.LoadScene ("Menu");
	}

	public void OnRestartClick()
	{
		this.gameObject.SetActive (false);
		SceneManager.LoadScene ("Game");
		ball.SetActive (true);
	}

	public void OnRateClick()
	{
		Application.OpenURL (URL);
	}
}
