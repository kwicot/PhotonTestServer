using System;
using DefaultNamespace;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PhotonTest : MonoBehaviourPunCallbacks, IOnEventCallback
{
    private static PhotonTest instance;
    [SerializeField] private TMP_Text playersText;
    void Start()
    {
        if (instance && instance != this)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        
        PhotonNetwork.NickName = PlayerPrefs.GetString("Nickname",  $"Player_{Random.Range(0, 999)}");
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(this.gameObject);
    }

    #region ConnectionCallbacks

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogError($"Disconnected via: {cause}");
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Connected");
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        base.OnErrorInfo(errorInfo);
        Debug.LogError(errorInfo);
    }

    #endregion

    public override void OnConnectedToMaster()
    {
        Debug.LogWarning("Connected to server");
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        roomOption.Plugins = new[] { "CustomPlugin" };
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOption, typedLobby: TypedLobby.Default);
    }

    public void OnConnectedToServer()
    {

        Debug.Log("Connected to server");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"Fail create room: {message}");
    }

    [Serializable]
    class ClientTestEvent
    {
        public string EventName;
        public string EventText;
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.LogWarning($"Joined to room {PhotonNetwork.CurrentRoom.Name}");
        playersText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel("Game");
        playersText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Nickname");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Nickname");
    }

    public void OnEvent(EventData photonEvent)
    {
        //Debug.Log($"EventCode [{photonEvent.Code}]");
        var eventType = (ServerEventType)photonEvent.Code;
        Debug.Log($"ServerEventRecieved {eventType}");
    }
}
