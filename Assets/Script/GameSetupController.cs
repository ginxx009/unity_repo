using Photon.Pun;
using UnityEngine;
using System.IO;

public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer(); //create a networked player object for each player that loads on the multiplayer
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        PhotonNetwork.Instantiate(Path.Combine("DemoPrefab", "DemoPlayer"), Vector3.zero, Quaternion.identity);
    }
}
