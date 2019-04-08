using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//This class has been adapted from Lab 8
public class ConnectionController : MonoBehaviour
{
    public Text photonStatusText;
    public Text playerNameText;
    public Text photonCurrentRoomText;
    public InputField photonRoomToJoinText;
    public const string VERSION = "v1.0";

    void Awake()
    {
        // Connect to the main photon server
        if (!PhotonNetwork.connectedAndReady) PhotonNetwork.ConnectUsingSettings(VERSION);

        // create and seta random  player name
        PhotonNetwork.playerName = "Player" + Random.Range(1000, 9999);
        playerNameText.text = "PlayerName: " + PhotonNetwork.playerName;
        photonCurrentRoomText.text = "Room: (no room)";
    }

    void UpdateRoomInfo()
    {
        if (PhotonNetwork.room == null) // not in a room
        {
            photonCurrentRoomText.text = "Room: (no room)";
        }
        else // in a room
        {
            photonCurrentRoomText.text = "Room: " + PhotonNetwork.room.Name + " (" + PhotonNetwork.room.PlayerCount + ")";
        }
    }

    // BUTTON HANDLERS

    public void ButtonHandlerCreateRoom()
    {
        if (PhotonNetwork.connectedAndReady && photonRoomToJoinText.text.Length > 0) // check there is a name entered before creating
        {
            PhotonNetwork.CreateRoom(photonRoomToJoinText.text);
        }
    }

    public void ButtonHandlerJoinRoom()
    {
        if (PhotonNetwork.connectedAndReady && photonRoomToJoinText.text.Length > 0) // check there is a name entered before joining
        {
            PhotonNetwork.JoinRoom(photonRoomToJoinText.text);
        }
    }

    public void ButtonHandlerLeaveRoom()
    {
        if (PhotonNetwork.connectedAndReady)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    // EVENT CALLBACKS

    void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster!");
        photonStatusText.text = "Status: Connected";
        UpdateRoomInfo();
    }

    void OnFailedToConnectToPhoton()
    {
        Debug.Log("OnFailedToConnectToPhoton");
        photonStatusText.text = "Status: Connection Failed";
        UpdateRoomInfo();
    }

    void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom: " + PhotonNetwork.room.Name);
        UpdateRoomInfo();
    }

    void OnPhotonCreateRoomFailed()
    {
        Debug.Log("OnPhotonCreateRoomFailed");
        UpdateRoomInfo();
    }

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom: " + PhotonNetwork.room.Name);
        UpdateRoomInfo();

           Vector3 position = new Vector3(-735, -496, 0);
        Quaternion rotation = Quaternion.identity;
        //   PhotonNetwork.Instantiate("NetworkedGran", position, rotation, 0);

        GameObject grandma = GameObject.FindGameObjectWithTag("Grandma");
        grandma.SetActive(false);
        PhotonNetwork.Instantiate("NetworkedGran", position, rotation, 0);
        GameObject networkedGran = GameObject.FindGameObjectWithTag("Grandma");
   
        networkedGran.transform.parent = GameObject.FindGameObjectWithTag("Main menu canvas").transform;
        networkedGran.transform.localScale = grandma.transform.localScale;
        networkedGran.transform.position = grandma.transform.position;
    }

    void OnPhotonPlayerConnected()
    {
        Debug.Log("OnPhotonPlayerConnected");
        UpdateRoomInfo();
    }

    void OnPhotonPlayerDisconnected()
    {
        Debug.Log("OnPhotonPlayerDisconnected");
        UpdateRoomInfo();
    }

    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("OnPhotonJoinRoomFailed");
        photonStatusText.text = "Status: Join Room Failed!";
        UpdateRoomInfo();
    }

    void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
        GameObject grandma = GameObject.FindGameObjectWithTag("Grandma");
        grandma.SetActive(true);
        photonStatusText.text = "Status: Left Room!";
        UpdateRoomInfo();
    }

}