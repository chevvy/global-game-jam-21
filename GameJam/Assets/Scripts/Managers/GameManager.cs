using System;
using System.Collections.Generic;
using Cinemachine;
using NUnit.Framework;
using PlayerController;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

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

        public int ScoreToWin = 10;

        private Random _random;

        public AudioSource dropRing;

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
            _random = new Random();
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
            if (IsGameFinished()) {
                FinishGame();
                return;
            }
            uiScores.SetPlayerScore(playerID, _scoreboard[playerID]);
        }
        
        public bool RemovePointFromPlayer(int playerID, int amountOfPoints) {
            int newScore = _scoreboard[playerID] - amountOfPoints;
            if (newScore < 0) { return false; }
            _scoreboard[playerID] = newScore;
            uiScores.SetPlayerScore(playerID, _scoreboard[playerID]);
            return true;
        }

        public bool IsGameFinished() {
            foreach (KeyValuePair<int,int> playerAndScore in _scoreboard) {
                if((playerAndScore.Value >= ScoreToWin)) {
                    return true;
                }
            }

            return false;
        }

        private void FinishGame() {
            foreach (KeyValuePair<int,int> playerAndScore in _scoreboard) {
                if((playerAndScore.Value >= ScoreToWin)) {
                    uiScores.OnEndGame(playerAndScore.Key);
                    cameraBrain.GetComponent<CinemachineBlendListCamera>().Priority = 10;
                }
            }
        }

        public void PlayerTakesDamages(int playerID, Vector3 playerPosition) {
            int amountOfnodeLost = _random.Next(1, 3);
            Debug.Log("removed " + amountOfnodeLost + " of nodes");
            if(!RemovePointFromPlayer(playerID, amountOfnodeLost)) { return;};
            gridManager.SpawnLootableDataNode(amountOfnodeLost, playerPosition);
            dropRing.Play();
        }
    }
}
