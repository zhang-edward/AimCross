using UnityEngine;
using System.Collections;

using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

	public static AdManager instance;
	

	InterstitialAd interstitial;
	private const string INTERSTITIAL_ID = "ca-app-pub-1010781108315903/2216275874";
	BannerView bannerView;
	private const string BANNER_ID = "ca-app-pub-1010781108315903/3941857877";

	//private bool bannerHidden = true;

	void Awake()
	{
		// Make this a singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
		RequestBanner();
		RequestInterstitial();
	}

	void Update()
	{
		/*if (bannerHidden)
		{
			bannerView.Show ();
		}*/
	}

	public void ShowInterstitial()
	{
		// Every 6th game show an ad
		if (interstitial.IsLoaded())
		{
			interstitial.Show();
		}
	}

	public void RequestBanner()
	{
		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(
			BANNER_ID, AdSize.SmartBanner, AdPosition.Top);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
			//.AddTestDevice("A4D94C6CCCC78F95136843C5B0579088")
			.Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
	}
	
	public void RequestInterstitial()
	{
		#if UNITY_ANDROID
		string adUnitId = INTERSTITIAL_ID;
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		// ADD TEST DEVICE HERE
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice("A4D94C6CCCC78F95136843C5B0579088")
			.Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}
}
