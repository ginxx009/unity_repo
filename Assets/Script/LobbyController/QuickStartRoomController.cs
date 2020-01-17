using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QuickStartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int waitingSceneIndex; //build index for the multiplayer scene


    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom() //callback for successfully creating or joining a room
    {
        Debug.Log("Joined the room");
        StartGame();
    }

    private void StartGame() //loading into multiplayer scene
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Joining Waiting Area");
            PhotonNetwork.LoadLevel(waitingSceneIndex);
        }
    }
}
