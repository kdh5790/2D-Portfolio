using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class PVPLobbyManager : MonoBehaviourPunCallbacks
{
    public static PVPLobbyManager instance;

    public Text topInfoText;
    public Text bottomLeftInfoText;
    public Button randomMatchingButton;

    private int roomNumber;
    string nickname;

    void Awake()
    {
        PhotonNetwork.SendRate = 60;

        PhotonNetwork.SerializationRate = 30;

        StartCoroutine(LoadingText("���� ������..."));
    }

    private void Start()
    {
        var player = FindObjectOfType<Character>();
        nickname = player.playerName;
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        PhotonNetwork.ConnectUsingSettings();
        randomMatchingButton.interactable = false;
    }


    // Photon Online Server�� �����ϸ� �Ҹ��� �ݹ� �Լ�
    // PhotonNetwork.ConnectUsingSettings() �Լ��� �����ϸ� �Ҹ���.
    public override void OnConnectedToMaster()
    {
        StopAllCoroutines();
        PhotonNetwork.LocalPlayer.NickName = nickname;
        topInfoText.text = $"���� ���� �Ϸ� \n{PhotonNetwork.LocalPlayer.NickName}\n {PhotonNetwork.NetworkClientState}";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        topInfoText.text = $"�κ� ���� �Ϸ�";
        bottomLeftInfoText.text = $"�г��� : {PhotonNetwork.LocalPlayer.NickName}";
        randomMatchingButton.interactable = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        StopAllCoroutines();
        topInfoText.text = $"{message}\nError Code : {returnCode}";

        roomNumber = Random.Range(10000, 99999);
        PhotonNetwork.CreateRoom(roomNumber.ToString(), new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        var room = PhotonNetwork.CurrentRoom;
        if (room.PlayerCount < 2)
        {
            StartCoroutine(LoadingText("��Ī ��..."));
        }
    }

    public override void OnJoinedRoom()
    {
        var room = PhotonNetwork.CurrentRoom;
        StartCoroutine(LoadingText("��Ī ��..."));
        if (room.PlayerCount < 2)
        {
            StartCoroutine(LoadingText("��Ī ��..."));
            StartCoroutine(CheckPlayerCount());
        }
        else
        {
            Invoke("MatchingSuccess", 1f);
        }
    }

    public void ConnectLobby()
    {
        AudioManager.instance.PlayButtonClip();
        randomMatchingButton.interactable = false;
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public void LeaveLobbyButton()
    {
        AudioManager.instance.PlayButtonClip();
        Destroy(FindObjectOfType<Character>().gameObject);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(1);
    }

    public void MatchingSuccess()
    {
        topInfoText.text = "��Ī ����!";
        PhotonNetwork.LoadLevel("PvpScene");
    }

    IEnumerator LoadingText(string _text)
    {
        string result = "";

        for (int i = 0; i < _text.Length; i++)
        {
            result += _text[i];
            topInfoText.text = result;
            yield return new WaitForSeconds(0.2f);
        }

        StartCoroutine(LoadingText(_text));
    }

    IEnumerator CheckPlayerCount()
    {
        yield return new WaitForSeconds(0.2f);
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;


        bottomLeftInfoText.text = $"\nPlayer Count : {playerCount}";

        if (playerCount == 2)
        {
            StopAllCoroutines();
            MatchingSuccess();
        }
        else if (playerCount < 2)
        {
            StartCoroutine(CheckPlayerCount());
        }
    }
}
