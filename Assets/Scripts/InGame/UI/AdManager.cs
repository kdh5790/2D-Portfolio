using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    UIManager ui;
    public Button adBtn;

    string androidAdID = "4821741";
    string rewardedID = "Rewarded_Android";

    string adUnitID = null;

    private void Awake()
    {
        adUnitID = androidAdID;
        Advertisement.Initialize(adUnitID, true, this);

        ui = FindObjectOfType<UIManager>();
    }

    // 광고 Load 성공 시 호출
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Load Complete");

        if(placementId.Equals(rewardedID))
        {
            adBtn.onClick.AddListener(ShowAd);
        }
    }

    public void ShowAd()
    {
        Advertisement.Show(rewardedID, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("광고 시청 완료");
        GameManager.instance.AdRevive();
        Advertisement.Load(rewardedID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Initialize Complete");
        Advertisement.Load(rewardedID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }
}
