using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	const string URL = "https://www.facebook.com/golfpads";

	public bool paused;
	int pausedString;
	public GameObject pauseMenu;
	public Button pauseButton;

	bool isMute;
	int sound;
	public Sprite soundOn, soundMute;
	public Button soundButton;

	int bestScore;
	public Text bestScoreText;
	int credits;
	public Text creditsText;

	public GameObject ball;

	public void Start()
	{
		sound = PlayerPrefs.GetInt ("Sound");
		if (sound == 0) {
			soundButton.image.sprite = soundMute;
			AudioListener.volume = 0;
		}
	}
	public void OnPauseClick()
	{
		PlayerPrefs.SetInt ("Paused", 1);
		Time.timeScale = 0;
		paused = true;
		pauseMenu.SetActive (true);
		bestScore = PlayerPrefs.GetInt ("Best Score");
		bestScoreText.text = "Best Score: " + bestScore.ToString ();
		pauseButton.interactable = false;
		ball.SetActive (false);
	}

	public void OnCloseClick()
	{
		PlayerPrefs.SetInt ("Paused", 0);
		Time.timeScale = 1;
		paused = false;
		pauseMenu.SetActive (false);
		pauseButton.interactable = true;
		ball.SetActive (true);
	}

	public void OnShopClick()
	{
		SceneManager.LoadScene ("Shop");
	}

	public void OnMenuClick()
	{
		SceneManager.LoadScene ("Menu");
	}

	public void OnSoundClick()
	{
		sound = PlayerPrefs.GetInt ("Sound");

		if (sound == 1) 
			isMute = true;
		else
			isMute = false;

		if (isMute) {
			soundButton.image.sprite = soundMute;
			PlayerPrefs.SetInt ("Sound", 0);
			AudioListener.volume = 0;
		} else {
			soundButton.image.sprite = soundOn;
			PlayerPrefs.SetInt ("Sound", 1);
			AudioListener.volume = 1;
		}
	}

	public void OnRateClick()
	{
		Application.OpenURL (URL);
	}

	public void OnBackButton()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("Menu");
		}
	}
}
