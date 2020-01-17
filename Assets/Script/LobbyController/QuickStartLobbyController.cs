using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartLobbyController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject quickStartButton; //used creating and joining the game
    [SerializeField]
    private GameObject quickCancelButton; //used for stop joining the game
    [SerializeField]
    private int roomSize; // manually set players in the room 1 at a time
    

    //Callback when connection is established
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // sync whatever scene has the master have
        quickStartButton.SetActive(true);
    }

    public void QuickStart()
    {
        quickStartButton.SetActive(false);
        quickCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom(); //tries to join an existing room
        Debug.Log("Quick Start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    public void CreateRoom() //create room
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);//creating a random name for a room 
        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)roomSize
        };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOps); // attempting to create a new room
        Debug.Log("Room Number: " + randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create a room ... trying again");
        CreateRoom();
    }

    //Stop looking for a room
    public void QuickCancel()
    {
        quickCancelButton.SetActive(false);
        quickStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
    
}
