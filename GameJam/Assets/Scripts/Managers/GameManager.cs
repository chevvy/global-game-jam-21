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

        public string centerOfMapObjectName = "MapCenter";
        public float defaultPlayerWeightToAdjustCamera = 5f;

        public Dictionary<int, int> scoreboard;

        #region Managers

        public GridManager gridManager;
        public PlayerControllerManager playerControllerManager;

        #endregion

        private void Awake() {
            SetsSingleton();
            scoreboard = new Dictionary<int, int>();
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
            scoreboard.Add(playerID, 0);
        }

        public void RemovePlayerFromScoreboard(int playerID) {
            scoreboard.Remove(playerID);
        }

        public void AddPointToPlayer(int playerID) {
            scoreboard[playerID]++;
            Debug.Log("Player " + playerID + " , current score = " + scoreboard[playerID]);
        }
    }
}
