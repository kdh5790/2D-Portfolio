using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class RaidLobbyManager : MonoBehaviourPunCallbacks
{
    public Button matchBtn;
    public GameObject matchUI;

    public Text countText;
    public Text matchText;

    Coroutine countCoroutine;

    void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        matchBtn.interactable = false;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        var player = FindObjectOfType<Character>();
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("asd");
        //PhotonNetwork.LocalPlayer.NickName = FirebaseManager.instance.user.DisplayName;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // 로비 접속 성공
        matchBtn.interactable = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if (returnCode == 32760)
        {
            int roomNumber = Random.Range(10000, 99999);
            PhotonNetwork.CreateRoom(roomNumber.ToString(), new RoomOptions { MaxPlayers = 2 });
        }
        else
            Debug.Log($"알 수 없는 오류 발생 Code : {returnCode}");
    }

    public override void OnCreatedRoom()
    {
        matchUI.SetActive(true);
        var room = PhotonNetwork.CurrentRoom;
    }

    public override void OnJoinedRoom()
    {
        matchUI.SetActive(true);
        var room = PhotonNetwork.CurrentRoom;
        countCoroutine = StartCoroutine(CheckPlayerCount());
    }

    public void JoinRoom() // Button
    {
        AudioManager.instance.PlayButtonClip();
        matchBtn.interactable = false;
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public void Cancel() // Button
    {
        AudioManager.instance.PlayButtonClip();
        PhotonNetwork.LeaveRoom();
        matchUI.SetActive(false);
        matchBtn.interactable = true;
        StopCoroutine(countCoroutine);
    }

    public void LeaveLobby() // Button
    {
        AudioManager.instance.PlayButtonClip();
        PhotonNetwork.Disconnect();
        Destroy(FindObjectOfType<Character>().gameObject);
        SceneManager.LoadScene(1);
    }

    private IEnumerator CheckPlayerCount()
    {
        var room = PhotonNetwork.CurrentRoom;
        string text = matchText.text;
        string result = "";

        Debug.Log(text.Length);

        while (room.PlayerCount < 2)
        {
            for (int i = 0; i < text.Length; i++)
            {
                result += text[i];
                matchText.text = result;
                countText.text = $"현재 플레이어 {room.PlayerCount} / {room.MaxPlayers}";
                yield return new WaitForSeconds(0.2f);
            }
            result = "";
        }
        matchText.text = text;


        if (room.PlayerCount >= 2)
            PhotonNetwork.LoadLevel("RaidScene");

        yield return null;
    }
}
