using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    class PlayerMoveData
    {
        public string PlayerId;
        public int PlayerSpeed;
    }
    public class LevelController : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [SerializeField] private LoadedPlayersCounter _loadedPlayersCounter;
        [SerializeField] private GameObject[] players;
        [SerializeField] private GameObject roadObject;
        [SerializeField] private int roads;
        [SerializeField] private CameraFollow cameraFollow;

        private void Start()
        {
            if (_loadedPlayersCounter.AllLoaded)
            {
                Initialize();
            }
            else
            {
                _loadedPlayersCounter.OnAllLoaded += Initialize;
            }

            for (int i = 0; i < roads; i++)
            {
                var obj = Instantiate(roadObject, roadObject.transform.position, roadObject.transform.rotation);
                var pos = obj.transform.position;
                pos.z += i;
                obj.transform.position = pos;
            }
        }

        void Initialize()
        {
            Debug.LogWarning("Initialize");
            var target = players[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            cameraFollow.SetTarget(target.transform);
            target.GetComponent<MeshRenderer>().materials[0].color = Color.green;
            if (PhotonNetwork.IsMasterClient)
            {
                // PhotonNetwork.RaiseEvent()
            }
        }

        public void OnEvent(EventData photonEvent)
        {

            var eventType = (ServerEventType) photonEvent.Code;
            if (eventType == ServerEventType.StartPhase_Move)
            {
                Debug.Log(photonEvent.ToStringFull());
                 var data = (string)photonEvent.Parameters[0];

                 var splitData = data.Split(';');
                 if (splitData.Length > 0)
                 {
                     PlayerMoveData[] playersData = new PlayerMoveData[(splitData.Length - 1) / 2];

                     for (int i = 0; i < splitData.Length - 1; i += 2)
                     {
                         PlayerMoveData playerData = new PlayerMoveData();
                         playerData.PlayerId = splitData[i];
                         playerData.PlayerSpeed = System.Convert.ToInt32(splitData[i + 1]);

                         playersData[i / 2] = playerData;
                     }
                     
                     foreach (var playerMoveData in playersData)
                     {
                         Debug.Log($"Received player with id [{playerMoveData.PlayerId}] and speed [{playerMoveData.PlayerSpeed}]");
                     }
                     
                     if (PhotonNetwork.IsMasterClient)
                     {
                         StartCoroutine(moveCars(playersData));
                     }
                     
                 }
                 else
                 {
                     Debug.LogWarning("Data is null or empty");
                 }
            }
        }

        IEnumerator moveCars(PlayerMoveData[] playersData)
        {
            foreach (var playerMoveData in playersData)
            {
                int actorNumber = System.Convert.ToInt32(playerMoveData.PlayerId);
                var transform = players[actorNumber - 1].transform;
                var pos = transform.position;
                pos.z += playerMoveData.PlayerSpeed;
                yield return transform.DOMoveZ(pos.z, 2).WaitForCompletion();
            }
        }
    }
}