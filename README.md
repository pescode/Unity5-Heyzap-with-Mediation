# Unity5-Heyzap-with-Mediation
Heyzap Ads for iOS & Android with mediation.
Interstitial, Rewarded and Banner Ads

Show Admob ads with mediation (interstitial &amp; rewarded) from different providers like AdColony, AdMob, AppLovin, Chartboost, Facebook Audience Network, UnityADS &amp; Vungle

### Current version 1.0.0 ###


### HOW TO USE ###

* Download and import the unity package here https://goo.gl/MQdo4Y
* You can write/use your own code to display ads or you can optionally use RshkAds.cs .
* Drag and drop RshkAds prefab in you first scene (when the game launch, in your loading screen or in the first scene so it can start to fetch interstitials and rewarded ads ASAP).
* Enter your publisher ID in RshkAds.cs line 53.
* To setup your APPLOVIN SDK KEY for Android OPEN Plugins/Android/RshkAds/AndroidManifest.xml . Add your SDK KEY on line 28.
* To display interstitial Ads just call RshkAds.ShowInterstitial() anywhere in your code. As soon as the ads closes in will fetch automatically another ad for you.
* To use rewarded Ads you must do the following
In your game logic, in the part where you want to know when the user has closed the rewarded ads and do something else (like give virtual coins or another revive) you must setup a listener like this
* If you want to test, on iOS is pretty straighforward, you can open Test.scene and run. On Android you will have to setup Google Play Games Services first, the plugin version compatible and tested with this is available in releases folder.

void OnEnable()
{
 RshkAds.OnRewardedCompleted += YourFunctionToDoSomething;
}

void OnDisable()
{
 RshkAds.OnRewardedCompleted -= YourFunctionToDoSomething;
}

void YourFunctionToDoSomething()
{
 // reward the player HERE!
}

Before showing your special button like WATCH AD FOR 50 COINS or CONTINUE GAME you probably want to know if there is a RewardedAD ready to show... to know that just call the function that returns a boolean 

 RshkAds.IsRewardedAdsAvailable()

To show the rewarded AD call

 RshkAds.ShowRewarded()

If you want to display banner just call
RshkAds.ShowBanner();

To hide banner
RshkAds.HideBanner();

To destroy a banner (if you want Heyzap to try to load from another network)
RshkAds.DestroyBanner();

### Plugins ###

* HeyZap (Unity) v10.2.2

### iOS ###

* AdColony 2.6.2
* AdMob 7.13.0
* AppLovin 3.4.3
* Chartboost 6.5.2
* Facebook Audience Network 4.10.1
* UnityAds 2.0.5
* Vungle 4.0.9

### ANDROID ###

* AdColony 2.3.6
* AdMob 9.8.0
* AppLovin 6.3.0
* Chartboost 6.6.0
* Facebook Audience Network 4.10.0
* UnityAds 2.0.5
* Vungle 4.0.3

You can find the libs in https://developers.heyzap.com/docs/unity_sdk_setup_and_requirements

### ABOUT ###
Used by Roshka Studios http://roshkastudios.com