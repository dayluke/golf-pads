using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	const string URL = "https://www.facebook.com/golfpads";
	bool isMute;
	int sound;

	public Sprite soundOn, soundMute;
	public Button soundButton;

	int bestScore;
	public Text bestScoreText;

	public Button infoButton, infoCloseButton, warningCloseButton;
	public GameObject infoMenu, warning;

	public void Start()
	{
		if (PlayerPrefs.HasKey ("Best Score")) {
			bestScore = PlayerPrefs.GetInt ("Best Score");
		} else {
			PlayerPrefs.SetInt ("Best Score", 0);
			bestScore = PlayerPrefs.GetInt ("Best Score");
		}

		if (PlayerPrefs.HasKey ("Sound")) { //Sound (1) means Sound is on, Sound (0) means Sound is muted.
			sound = PlayerPrefs.GetInt ("Sound");
			if (sound == 0) {
				soundButton.image.sprite = soundMute;
				AudioListener.volume = 0;
			}
		} else {
			PlayerPrefs.SetInt ("Sound", 1);
			sound = 1;
			AudioListener.volume = 1;
		}

		if (PlayerPrefs.HasKey ("Ads Removed")) {
			return;
		} else {
			PlayerPrefs.SetInt ("Ads Removed", 0);
		}

		bestScoreText.text = "Best Score: " + bestScore.ToString ();
	}

	public void OnPlayClick()
	{
		SceneManager.LoadScene ("Game");

		PlayerPrefs.SetInt ("Paused", 0);

		Time.timeScale = 1;
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
		
	public void OnShopClick()
	{
		SceneManager.LoadScene ("Shop");
	}

	public void OnRateClick()
	{
		Application.OpenURL (URL);
	}

	public void OnBackButton()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

	public void OnInfoButton()
	{
		infoButton.interactable = false;
		infoMenu.SetActive (true);
	}

	public void OnCloseButton()
	{
		infoButton.interactable = true;
		infoMenu.SetActive (false);
	}

	public void OnResetButton()
	{
		infoCloseButton.interactable = false;
		warning.SetActive (true);
	}

	public void OnConfirmResetButton()
	{
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.SetInt ("Best Score", 0);
		PlayerPrefs.SetInt("Credits", 0);
		PlayerPrefs.SetInt ("Current Ball", 0);

		warning.SetActive (false);
		infoMenu.SetActive (false);
		bestScore = PlayerPrefs.GetInt ("Best Score");
		bestScoreText.text = "Best Score: " + bestScore.ToString ();
	}

	public void OnWarningCloseButton()
	{
		warning.SetActive (false);
		infoCloseButton.interactable = true;
	}
}