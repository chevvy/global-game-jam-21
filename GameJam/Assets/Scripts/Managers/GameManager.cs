using System;
using Cinemachine;
using NUnit.Framework;
using UnityEngine;

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
        
        #region Managers

        public GridManager gridManager;

        #endregion

        private void Awake() {
            SetsSingleton();

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
    }
}
