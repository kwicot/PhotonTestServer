using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using EventType = DefaultNamespace.EventType;

public class LoadedPlayersCounter : MonoBehaviourPunCallbacks
{
    private int loaded = 0;

    public UnityAction OnAllLoaded;
    public bool AllLoaded { get; private set; }
    private void Start()
    {
        photonView.RPC("Loaded", RpcTarget.All);
    }

    [PunRPC]
    void Loaded()
    {
        loaded++;
        if (loaded == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            AllLoaded = true;
            OnAllLoaded?.Invoke();
            if (PhotonNetwork.IsMasterClient)
            {
                byte eventCode = (byte)EventType.PlayersLoaded;
                List<string> players = new List<string>();
                var roomPlayers = PhotonNetwork.CurrentRoom.Players.Values;
                Debug.Log($"Players in room {roomPlayers.Count}");
                foreach (var playerValue in roomPlayers)
                {
                    Debug.Log($"Add player with nickname: {playerValue.NickName}  id: {playerValue.UserId}");
                    players.Add(playerValue.ActorNumber.ToString());
                }

                string[] playersArray = players.ToArray();
                object playerCompressed = playersArray;
                Debug.Log(playersArray.Length + " Players");
                foreach (var s in playersArray)
                {
                    Debug.Log(s);
                }
                PhotonNetwork.RaiseEvent(eventCode, playerCompressed, RaiseEventOptions.Default,
                    SendOptions.SendReliable);
            }
        }
    }
}
