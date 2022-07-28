using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;

public class FirstSceneUIManger : MonoBehaviour
{
    public static FirstSceneUIManger instance;

    public GameObject selectMenu;
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject googleLoginBtn;
    public GameObject nicknamePanel;
    public GameObject classSelectPanel;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public void ClearUI()
    {
        FirebaseManager.instance.infoTxt.text = "";
        selectMenu.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        googleLoginBtn.SetActive(false);
    }

    public void LoginBox()
    {
        FirebaseManager.instance.infoTxt.text = "";
        AudioManager.instance.PlayButtonClip();
        ClearUI();
        loginPanel.SetActive(true);
    }

    public void RegisterBox()
    {
        FirebaseManager.instance.infoTxt.text = "";
        AudioManager.instance.PlayButtonClip();
        ClearUI();
        registerPanel.SetActive(true);
    }

    public void SelectMenu()
    {
        FirebaseManager.instance.infoTxt.text = "";
        AudioManager.instance.PlayButtonClip();
        ClearUI();
        selectMenu.SetActive(true);
        googleLoginBtn.SetActive(true);
    }

    public void NickNameBox()
    {
        FirebaseManager.instance.infoTxt.text = "";
        AudioManager.instance.PlayButtonClip();
        ClearUI();
        nicknamePanel.SetActive(true);
    }

    public void ClassSet()
    {
        FirebaseManager.instance.infoTxt.text = "";
        AudioManager.instance.PlayButtonClip();
        ClearUI();
        classSelectPanel.SetActive(true);
    }
}
