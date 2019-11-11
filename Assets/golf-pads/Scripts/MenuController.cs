using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MenuController : MonoBehaviour
{
	const string URL = "https://www.facebook.com/golfpads";
	bool isMute;
	int sound, vibrate;
    
	int bestScore;
	public Text bestScoreText, soundText, vibrateText;
	public Button infoButton, infoCloseButton, warningCloseButton;
	public GameObject infoMenu, settingsMenu, warning;

	public void Start()
	{
       
		if (PlayerPrefs.HasKey ("Best Score")) {
			bestScore = PlayerPrefs.GetInt ("Best Score");
		} else {
			PlayerPrefs.SetInt ("Best Score", 0);
			bestScore = PlayerPrefs.GetInt ("Best Score");
		}

		bestScoreText.text = "Best Score: " + bestScore.ToString ();

		if (PlayerPrefs.HasKey ("Sound")) { //Sound (1) means Sound is on, Sound (0) means Sound is muted.
			sound = PlayerPrefs.GetInt ("Sound");
			if (sound == 0) {
				soundText.text = "Sound Off";
				AudioListener.volume = 0;
			}
		} else {
			PlayerPrefs.SetInt ("Sound", 1);
			sound = 1;
            soundText.text = "Sound On";
			AudioListener.volume = 1;
		}

        if (PlayerPrefs.HasKey("Vibrate")) {
            vibrate = PlayerPrefs.GetInt("Vibrate");
            if (vibrate == 0)
            {
                vibrateText.text = "Vibrate Off";
            }
        }
        else
        {
            PlayerPrefs.SetInt("Vibrate", 1);
            vibrate = 1;
            vibrateText.text = "Vibrate On";
        }

        if (PlayerPrefs.HasKey ("Ads Removed")) {
			return;
		} else {
			PlayerPrefs.SetInt ("Ads Removed", 0);
		}
	}

    #region Initial Buttons
    public void OnPlayClick()
	{
		SceneManager.LoadScene ("Game");

		PlayerPrefs.SetInt ("Paused", 0);

		Time.timeScale = 1;
	}

    public void OnSettingsClick()
    {
        if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
        }
        else
        {
            settingsMenu.SetActive(true);
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
#endregion

    public void OnBackButton()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

    #region Other Buttons
    public void OnInfoButton()
	{
		infoButton.interactable = false;
		infoMenu.SetActive (true);
	}

	public void OnInfoClose()
	{
        infoButton.interactable = true;
        infoMenu.SetActive (false);
    }

    public void OnSoundClick()
    {
        sound = PlayerPrefs.GetInt("Sound");

        if (sound == 1)
        {
            soundText.text = "Sound Off";
            PlayerPrefs.SetInt("Sound", 0);
            AudioListener.volume = 0;
        }
        else
        {
            soundText.text = "Sound On";
            PlayerPrefs.SetInt("Sound", 1);
            AudioListener.volume = 1;
        }
    }

    public void OnVibrateClick()
    {
        vibrate = PlayerPrefs.GetInt("Vibrate");

        if (vibrate == 1)
        {
            vibrateText.text = "Vibrate Off";
            PlayerPrefs.SetInt("Vibrate", 0);
        }
        else
        {
            vibrateText.text = "Vibrate On";
            PlayerPrefs.SetInt("Vibrate", 1);
        }
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
#endregion
}