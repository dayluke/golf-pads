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
	int sound, vibrate;
    public Text soundText, vibrateText;
    public GameObject settingsMenu;

	int bestScore;
	public Text bestScoreText;
	int credits;
	public Text creditsText;

	public GameObject ball;

	public void Start()
	{
		sound = PlayerPrefs.GetInt ("Sound");
		if (sound == 0) {
            soundText.text = "Sound Off";
			AudioListener.volume = 0;
		} else
        {
            soundText.text = "Sound On";
            AudioListener.volume = 1;
        }

        vibrate = PlayerPrefs.GetInt("Vibrate");
        if (vibrate == 0)
        {
            vibrateText.text = "Vibrate Off";
        }
        else
        {
            vibrateText.text = "Vibrate On";
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

	public void OnRateClick()
	{
		Application.OpenURL (URL);
	}

    public void OnSettingsClick()
    {
        if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
        } else
        {
            settingsMenu.SetActive(true);
        }
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

    public void OnBackButton()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("Menu");
		}
	}
}
