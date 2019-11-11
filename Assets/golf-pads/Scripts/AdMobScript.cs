using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobScript : MonoBehaviour {

	string appId = "ca-app-pub-1696925983861520~8260368631";
	InterstitialAd interstitial;
	RewardBasedVideoAd rewardBasedVideo;
	public string BannerId;
	public string InterstitialId;
	public string RewardedId;
	int credits;

	public void Start()
	{

		if (PlayerPrefs.GetInt ("Ads Removed") == 0) {
			MobileAds.Initialize (appId);
			//Request Ads
			RequestBanner ();
			RequestInterstitial ();
		} else {
			return;
		}

		// Get singleton reward based video ad reference.
		this.rewardBasedVideo = RewardBasedVideoAd.Instance;

		// Called when the user should be rewarded for watching a video.
		rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
		
		RequestRewardedVideo();
	}
	

	public void showInterstitialAd ()
	{
		//Show Ad
		if (interstitial.IsLoaded ()) {
			interstitial.Show ();
		}
	}

	public void showRewardedAd ()
	{
		//Show Ad
		if (rewardBasedVideo.IsLoaded ()) {
			rewardBasedVideo.Show ();
		}
	}

	private void RequestBanner () {
		#if UNITY_EDITOR
			string adUnitId = "ca-app-pub-3940256099942544/6300978111"; // test ad banner id
		#elif UNITY_ANDROID
			string adUnitId = BannerId;
		#elif UNITY_IPHONE
			string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
			string adUnitId = "unexpected_platform";
		#endif

		// Create a 320x50 banner at the bottom of the screen.
		BannerView bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ().Build ();

		// Load the banner with the request.
		bannerView.LoadAd (request);
	}

	private void RequestInterstitial () {
		#if UNITY_EDITOR
		    string adUnitId = "ca-app-pub-3940256099942544/1033173712"; // test ad interstital id
		#elif UNITY_ANDROID
			string adUnitId = InterstitialId;
		#elif UNITY_IPHONE
			string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
			string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd (adUnitId);

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ().Build ();

		// Load the interstitial with the request.
		interstitial.LoadAd (request);
	}

	private void RequestRewardedVideo()
	{
		#if UNITY_EDITOR
			string adUnitId = "ca-app-pub-3940256099942544/5224354917"; // test ad rewarded id
		#elif UNITY_ANDROID
			string adUnitId = RewardedId;
		#elif UNITY_IPHONE
			string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
			string adUnitId = "unexpected_platform";
		#endif

		// Initialize an RewardedVideoAd.
		rewardBasedVideo = RewardBasedVideoAd.Instance;

		// Create am empty ad request.
		AdRequest request = new AdRequest.Builder ().Build ();

		// Load the rewarded video ad with the request.
		rewardBasedVideo.LoadAd (request, adUnitId);
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		print ("User rewarded with: " + amount.ToString () + " " + type);
		addCredits (amount);
	}

	public void addCredits(double amount)
	{
		credits	= PlayerPrefs.GetInt ("Credits");
		credits += (int)amount;
		PlayerPrefs.SetInt ("Credits", credits);
	}
}

