/*
Author: Victor Corvalan @pescadon
pescode.wordpress.com

Roshka Studios
roshkastudios.com
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Heyzap;
using System;
using UnityEngine.Analytics;

public class RshkAds : MonoBehaviour {
	static RshkAds instance;

	static int InterstitialMinNextAdShow = 0;
	static int InterstitialMaxNextAdShow = 2;
	static int InterstitialNextAdShow = 1;
	static int InterstitialAdCount = 0;

	static int VideoMinNextAdShow = 0;
	static int VideoMaxNextAdShow = 2;
	static int VideoNextAdShow = 0;
	static int VideoAdCount = 0;

	public static bool HasWatchedRewardedAds = false;
	public delegate void actionRewardedCompleted();
	public static event actionRewardedCompleted OnRewardedCompleted;

	//
	static bool isRewardedLoaded = false;
	static bool isInterstitialLoaded = false;
	static bool isVideoLoaded = false;
	static bool isBannerLoaded = false;
	static bool isBannerShowing = false;
	static bool canShowBanner = false;

	// Use this for initialization
	void Start () {
		if (instance) {
			Destroy (gameObject);
		} else {
			Debug.Log ("**********************\n**********************\nINITIALIZING AD SYSTEM");
			instance = this;
			DontDestroyOnLoad (gameObject);
			InterstitialAdCount = PlayerPrefs.GetInt ("InterstitialAdCount", InterstitialAdCount);
			InterstitialNextAdShow = PlayerPrefs.GetInt ("InterstitialNextAdShow", InterstitialNextAdShow);
			VideoAdCount = PlayerPrefs.GetInt ("VideoAdCount", VideoAdCount);
			VideoNextAdShow = PlayerPrefs.GetInt ("VideoNextAdShow", VideoNextAdShow);
			HeyzapAds.Start("ENTER_YOUR_PUBLISHER_ID_HERE", HeyzapAds.FLAG_NO_OPTIONS);
			HeyzapAds.ShowMediationTestSuite ();

			SetupListeners ();
		}
	}

	static void SetupListeners()
	{
		HZInterstitialAd.AdDisplayListener InterstitialListener = delegate(string adState, string adTag){
			if ( adState.Equals("show") ) {
				InterstitialAdOpening();
				Debug.Log ("**********************\n**********************\nINTERSTITIAL AD OPEN \n ");
			}
			if ( adState.Equals("hide") ) {
				InterstitialAdClose();
				Debug.Log ("**********************\n**********************\nINTERSTITIAL AD CLOSED \n ");
			}
			if ( adState.Equals("failed") ) {
				Debug.Log ("**********************\n**********************\nINTERSTITIAL AD FAILED \n ");
			}
			if ( adState.Equals("available") ) {
				Debug.Log ("**********************\n**********************\nINTERSTITIAL AVAILABLE \n ");
			}
			if ( adState.Equals("fetch_failed") ) {
				Debug.Log ("**********************\n**********************\nINTERSTITIAL FETCH FAILED \n ");
			}
		};
		HZInterstitialAd.SetDisplayListener(InterstitialListener);

		HZIncentivizedAd.AdDisplayListener RewardedListener = delegate(string adState, string adTag){
			if ( adState.Equals("show") ) {
				RewardedAdOpening();
			}
			if ( adState.Equals("hide") ) {
				RewardedAdClose();
			}
			if ( adState.Equals("failed") ) {
				Debug.Log ("**********************\n**********************\nREWARDED AD FAILED \n ");
			}
			if ( adState.Equals("available") ) {
				Debug.Log ("**********************\n**********************\nREWARDED AVAILABLE \n ");
			}
			if ( adState.Equals("fetch_failed") ) {
				Debug.Log ("**********************\n**********************\nREWARDED FETCH FAILED \n ");
			}
			if ( adState.Equals("incentivized_result_complete") ) {
				Debug.Log ("**********************\n**********************\nUSER WATCHED FULL REWARDED \n ");
			}
			if ( adState.Equals("incentivized_result_incomplete") ) {
				Debug.Log ("**********************\n**********************\nUSER CANCELLED REWARDED VIDEO \n ");
			}
		};

		HZIncentivizedAd.SetDisplayListener(RewardedListener);

		HZBannerAd.AdDisplayListener BannerListener = delegate(string adState, string adTag){
			if (adState == "loaded") {
				isBannerLoaded = true;
				Debug.Log ("**********************\n**********************\nBANNER AD LOADED \n ");
				// Do something when the banner ad is loaded
			}
			if (adState == "error") {
				isBannerLoaded = false;
				isBannerShowing = false;
				Debug.Log ("**********************\n**********************\nBANNER AD ERROR \n ");
				// Do something when the banner ad fails to load (they can fail when refreshing after successfully loading)
			}
			if (adState == "click") {
				Debug.Log ("**********************\n**********************\nBANNER AD CLICKED \n ");
				// Do something when the banner ad is clicked, like pause your game
			}
		};

		HZBannerAd.SetDisplayListener(BannerListener);
	}



	public static void ShowInterstitial(string tag = "interstitial")
	{
		Debug.Log ("**********************\n**********************\nSHOW INTERSTITIAL! \n " + (InterstitialAdCount+1) + "=" + InterstitialNextAdShow);
		//if (!IAP.IsAdsRemoved ()) {
		InterstitialAdCount++;
		PlayerPrefs.SetInt ("InterstitialAdCount", InterstitialAdCount);

		if (HZInterstitialAd.IsAvailable()) {
			if (InterstitialAdCount >= InterstitialNextAdShow) {
				if (!HasWatchedRewardedAds) {
					InterstitialAdCount = 0;
					InterstitialNextAdShow = UnityEngine.Random.Range (InterstitialMinNextAdShow, InterstitialMaxNextAdShow);
					VideoAdCount = 0;
					PlayerPrefs.SetInt ("InterstitialNextAdShow", InterstitialNextAdShow);
					HZInterstitialAd.Show ();
					Analytics.CustomEvent ("ADS Interstitial", new Dictionary<string, object> {
						{ "Tag", tag }
					});
				}
			}
		}
		//}
	}

	public static void ShowRewarded(string tag = "rewarded")
	{
		Debug.Log ("**********************\n**********************\nSHOW REWARDED! \n ");
		Analytics.CustomEvent("ADS Rewarded", new Dictionary<string, object>
			{
				{ "Tag", tag }
			});
		HasWatchedRewardedAds = true;
		InterstitialAdCount = 0;
		VideoAdCount = 0;
		HZIncentivizedAd.Show ();
	}

	public static void ShowVideo(string tag = "video")
	{
		Debug.Log ("**********************\n**********************\nSHOW VIDEO! \n " + (VideoAdCount+1) + "=" + VideoNextAdShow);

		//if (!IAP.IsAdsRemoved ()) {
			VideoAdCount++;
			PlayerPrefs.SetInt ("VideoAdCount", VideoAdCount);
			if (HZInterstitialAd.IsAvailable()) {
				if (VideoAdCount >= VideoNextAdShow) {
					InterstitialAdCount = 0;
					VideoAdCount = 0;
					VideoNextAdShow = UnityEngine.Random.Range (VideoMinNextAdShow, VideoMaxNextAdShow);
					PlayerPrefs.SetInt ("VideoNextAdShow", VideoNextAdShow);
					HZInterstitialAd.Show ();
					Analytics.CustomEvent ("ADS Video", new Dictionary<string, object> {
						{ "Tag", tag }
					});

				}	
			}
		//}
	}

	public static void ShowBanner()
	{
		Debug.Log ("**********************\n**********************\nSHOW BANNER \n ");
		//if (!IAP.IsAdsRemoved ()) {
		if (!isBannerLoaded) {
			if (!isBannerShowing) {
				Debug.Log ("**********************\n**********************\nBANNER NOT LOADED BUT NO SHOWING \n ");
				CreateOrShowBanner ();
			}
		} else {
			Debug.Log ("**********************\n**********************\nBANNER LOADED \n ");
			CreateOrShowBanner ();
		}
		//}
	}

	static void CreateOrShowBanner()
	{
		Debug.Log ("**********************\n**********************\nCREATING BANNER \n ");
		HZBannerShowOptions showOptions = new HZBannerShowOptions ();
		showOptions.Position = HZBannerShowOptions.POSITION_BOTTOM;
		showOptions.SelectedAdMobSize = HZBannerShowOptions.AdMobSize.BANNER; // optional, android only
		showOptions.SelectedFacebookSize = HZBannerShowOptions.FacebookSize.BANNER_HEIGHT_50; // optional, android only
		HZBannerAd.ShowWithOptions (showOptions);
		isBannerShowing = true;
	}

	public static void HideBanner()
	{
		if (isBannerLoaded && isBannerShowing) {
			isBannerShowing = false;
			HZBannerAd.Hide ();
		}
	}

	public static void DestroyBanner()
	{
		if (isBannerLoaded) {
			isBannerLoaded = false;
			isBannerShowing = false;
			HZBannerAd.Destroy ();
		}
	}

	static void InterstitialAdOpening()
	{
		Debug.Log ("**********************\n**********************\nInterstitial opening");
		AudioListener.pause = true;
		Time.timeScale = 0.01f;
	}

	static void InterstitialAdClose()
	{
		Debug.Log ("**********************\n**********************\nInterstitial close");
		instance.StartCoroutine (InterstitialDone ());
	}

	static IEnumerator InterstitialDone()
	{
		yield return new WaitForSeconds (1f);
		AudioListener.pause = false;
		Time.timeScale = 1;
	}

	public static bool IsRewardedAdsAvailable()
	{
		return HZIncentivizedAd.IsAvailable();
	}

	static void RewardedAdOpening()
	{
		Debug.Log ("**********************\n**********************\nRewarded Ad Opening");
		AudioListener.pause = true;
	}
	static void RewardedAdClose()
	{
		Debug.Log ("**********************\n**********************\nRewarded Ad Close");
		instance.StartCoroutine (RewardedDone ());
	}
	// We need this to avoid errors on Android
	static IEnumerator RewardedDone()
	{
		yield return new WaitForSeconds (1f);
		AudioListener.pause = false;
		OnRewardedCompleted();
	}
		
}
