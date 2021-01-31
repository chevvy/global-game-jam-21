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

        private List<GameObject> players;
        [SerializeField] private List<Material> podiumMaterials;

        #region Audio
        public AudioSource dropRing;
        public AudioSource digMiss;
        public AudioSource digHit;
        public FaderManager faderManager;
        #endregion

        #region Managers

        public GridManager gridManager;
        public PlayerControllerManager playerControllerManager;
        public UIScores uiScores;
        public Podium podium;
        
        #endregion

        #region Checks at initialization

        private void CheckForAllSerializedParameters() {
            // TODO faire une méthode plus clean pour hanlde ces messages
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

            if (podium == null) {
                Debug.LogError("Missing podium on GameManager");
            }
            
            if (faderManager == null) {
                Debug.LogError("Missing faderManager on GameManager");
            }
        }

        #endregion

        private void Awake() {
            SetsSingleton();
            CheckForAllSerializedParameters();
            _scoreboard = new Dictionary<int, int>();
            _targets = targetGroup.m_Targets;
            _random = new Random();
            players = new List<GameObject>();
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

        public void AddPlayer(PlayerInput playerInput) {
            _scoreboard.Add(playerInput.playerIndex, 0);
            players.Add(playerInput.gameObject);
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
                    podium.SetPlayersReference(players);
                    podium.EndGameSetup(GetOrderOfWinner());
                }
            }
        }

        public List<int> GetOrderOfWinner() {
            List<int> winningOrder = new List<int>();
            List<int> winningScores = new List<int>();
            foreach (KeyValuePair<int, int> playerAndScore in _scoreboard) {
                winningScores.Add(playerAndScore.Value);
            }
            winningScores.Sort((a,b) => b.CompareTo(a));
            foreach (var score in winningScores) {
                foreach (var playerAndScore in _scoreboard) {
                    if (playerAndScore.Value == score) {
                        winningOrder.Add(playerAndScore.Key);
                    }
                }
            }
            return winningOrder;
        }

        public void PlayerTakesDamages(int playerID, Vector3 playerPosition) {
            int amountOfnodeLost = _random.Next(1, 3);
            Debug.Log("removed " + amountOfnodeLost + " of nodes");
            if(!RemovePointFromPlayer(playerID, amountOfnodeLost)) { return;};
            gridManager.SpawnLootableDataNode(amountOfnodeLost, playerPosition);
            dropRing.Play();
        }

        public void PressedStart() {
            if (!IsGameFinished()) {
                faderManager.GoToMainLoop();
            }
        }
    }
}
