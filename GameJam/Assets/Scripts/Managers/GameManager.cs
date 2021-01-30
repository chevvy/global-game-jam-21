using System;
using System.Collections.Generic;
using Cinemachine;
using NUnit.Framework;
using PlayerController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _gameManager;
        public static GameManager Instance { get { return _gameManager; } }

        public CinemachineTargetGroup targetGroup;
        private CinemachineTargetGroup.Target[] _targets;
        public CinemachineBrain cameraBrain;

        public string centerOfMapObjectName = "MapCenter";
        public float defaultPlayerWeightToAdjustCamera = 5f;

        private Dictionary<int, int> _scoreboard;

        #region Managers

        public GridManager gridManager;
        public PlayerControllerManager playerControllerManager;
        public UIScores uiScores;
        
        #endregion

        #region Checks at initialization

        private void CheckForAllSerializedParameters() {
            if (targetGroup == null) {
                Debug.LogError("Missing targetgroup on GameManager");
            }
            
            if (cameraBrain == null) {
                Debug.LogError("Missing cinemachineBrain (cameraBrain) on GameManager");
            }
            
            if (gridManager == null) {
                Debug.LogError("Missing gridManager on GameManager");
            }
            
            if (playerControllerManager == null) {
                Debug.LogError("Missing playerControllerManager on GameManager");
            }
            
            if (uiScores == null) {
                Debug.LogError("Missing uiScores on GameManager");
            }
        }

        #endregion

        private void Awake() {
            SetsSingleton();
            CheckForAllSerializedParameters();
            _scoreboard = new Dictionary<int, int>();
            _targets = targetGroup.m_Targets;
        }

        private void SetsSingleton() {
            if (_gameManager != null && _gameManager != this) {
                Destroy(this.gameObject);
            }
            else {
                _gameManager = this;
            }
        }

        public void AddCameraTarget(PlayerController.PlayerController player)
        {
            // TODO Extract in its own function if more logic is added here
            for (var i = 0; i < _targets.Length; i++)
            {
                if (_targets[i].target.name != centerOfMapObjectName) continue;
                _targets[i].target = player.transform;
                _targets[i].weight = defaultPlayerWeightToAdjustCamera;
                return;
            }
        }

        public void AddPlayerToScoreboard(int playerID) {
            _scoreboard.Add(playerID, 0);
        }

        public void RemovePlayerFromScoreboard(int playerID) {
            _scoreboard.Remove(playerID);
        }

        public void AddPointToPlayer(int playerID) {
            _scoreboard[playerID]++;
            uiScores.SetPlayerScore(playerID, _scoreboard[playerID]);
            Debug.Log("Player " + playerID + " , current score = " + _scoreboard[playerID]);
        }
    }
}
