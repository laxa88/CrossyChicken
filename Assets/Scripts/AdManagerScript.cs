using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class AdManagerScript : MonoBehaviour {

	// For more info and documentation, check out the admob page:
	// https://firebase.google.com/docs/admob/android/games#unity_plugin_api

	// Declare variables here so we can re-use them later (for hiding/showing)
	BannerView bannerView;
	InterstitialAd interstitial;

	void Start ()
	{
		// Immediately create ads when game starts
		RequestBanner();
		RequestInterstitial();
	}

	void RequestBanner ()
	{
	    #if UNITY_ANDROID
		string adUnitId = "ca-app-pub-4334550757239277/9796943146"; // banner ad android ID
	    #elif UNITY_IPHONE
	    string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
	    #else
	    string adUnitId = "unexpected_platform";
	    #endif

	    // Create a 320x50 banner at the top of the screen.
	    BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

	    // Create an empty ad request.
	    AdRequest request = new AdRequest.Builder().Build();

	    // Load the banner with the request.
	    bannerView.LoadAd(request);
	}

	void RequestInterstitial()
	{
	    #if UNITY_ANDROID
		string adUnitId = "ca-app-pub-4334550757239277/5087541944"; // interstitial ad android ID
	    #elif UNITY_IPHONE
	    string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
	    #else
	    string adUnitId = "unexpected_platform";
	    #endif

	    // Initialize an InterstitialAd.
	    interstitial = new InterstitialAd(adUnitId);

	    // Create an empty ad request.
	    AdRequest request = new AdRequest.Builder().Build();

	    // Load the interstitial with the request.
	    interstitial.LoadAd(request);
	}

	public void showInterstitial ()
	{
		interstitial.OnAdClosed += OnInterstitialComplete;

		if (interstitial.IsLoaded()) {
			interstitial.Show();
		}
	}

	void OnInterstitialComplete (object o, System.EventArgs e)
	{
		// interstitials need to be re-created after it is shown
		RequestInterstitial();
	}

	public void showBanner ()
	{
		if (bannerView != null)
			bannerView.Show();
	}

	public void hideBanner ()
	{
		if (bannerView != null)
			bannerView.Hide();
	}
}
