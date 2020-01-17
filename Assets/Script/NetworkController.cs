/*
 * This 
 * https://doc.photonengine.com/en-us/pun/v2/demos-and-tutorials/pun-basics-tutorial/lobby
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkController : MonoBehaviourPunCallbacks //needs to change the monobehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Connects to the photon master server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " server!");
    }
}
