using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    // Photon view for sending rpc
    private new PhotonView photonView;

    [SerializeField]
    private int multiplayerSceneIndex;
    [SerializeField]
    private int menuSceneIndex;
    [SerializeField]
    private int minPlayerToStart;
    [SerializeField]
    private Text roomCountDisplay;
    [SerializeField]
    private Text timerToStartDisplay;
    [SerializeField]
    private Text roomNameDisplay;
    [SerializeField]
    private float maxWaitTime;
    [SerializeField]
    private float maxFullGameTime;

    private int playerCount;
    private int roomSize;

    private bool readyToCountDown;
    private bool readyToStart;
    private bool startingGame;

    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
    }

    void PlayerCountUpdate()
    {
        // gets player count
        playerCount = PhotonNetwork.PlayerList.Length;
        // gets current room maximum size
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomCountDisplay.text = playerCount + ":" + roomSize;
        roomNameDisplay.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name.Replace("Room","");
        
        if(playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if(playerCount >= minPlayerToStart)
        {
            readyToCountDown = true;
        }
        else
        {
            readyToCountDown = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        //send master client countdown in order to sync time
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
        }
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;

        if(timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }
    // Update is called once per frame
    void Update()
    {
        WaitingForMorePlayers();
    }

    void WaitingForMorePlayers()
    {
        if(playerCount <= 1)
        {
            ResetTimer();
        }

        if(readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if(readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if(timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
        }
    }

    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameTime;
    }

    void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
