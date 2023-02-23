using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace DefaultNamespace
{
    
    public class PlayerInputController : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [SerializeField] private Button _addSpeedButton;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _phaseText;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _firstPlayerNickName;
        [SerializeField] private TMP_Text _secondPlayerNickName;

        private void Start()
        {
            var players = PhotonNetwork.CurrentRoom.Players;
            _firstPlayerNickName.text = $"Player 1- {players[0].NickName}";
            _secondPlayerNickName.text = $"Player 2- {players[1].NickName}";
        }

        public void AddSpeed(int speed)
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            byte code = (byte)EventType.Change_MoveSpeed;

            var dict = new Dictionary<string, int>();
            dict.Add(actorNumber.ToString(),speed);
            _addSpeedButton.interactable = false;
            var curSpeed = Convert.ToInt32(_speedText.text);
            _speedText.text = $"{curSpeed+speed}";
            PhotonNetwork.RaiseEvent(code, (object)dict, RaiseEventOptions.Default, SendOptions.SendReliable);

           
            
        }

        public void OnEvent(EventData photonEvent)
        {
            var eventType = (ServerEventType)photonEvent.Code;
            switch (eventType)
            {
                case ServerEventType.StartPhase_Prepare:
                {
                    _addSpeedButton.interactable = true;
                    _speedText.text = photonEvent.Parameters[(byte)PhotonNetwork.LocalPlayer.ActorNumber].ToString();
                    _phaseText.text = "Phase: Prepare";
                    break;
                }
                case ServerEventType.StopPhase_Prepare:
                {
                    _addSpeedButton.interactable = false;
                    break;
                }
                case ServerEventType.StartPhase_Move:
                {
                    _phaseText.text = "Phase: Moving";
                    break;
                }
                case ServerEventType.StopPhase_Move:
                {
                    
                    break;
                }
                case ServerEventType.TimerTick:
                {
                    var time = photonEvent.Parameters[0].ToString();
                    _timerText.text = $"Time left: {time}";
                    break;
                }
                default:
                    break;
            }
        }
    }
}