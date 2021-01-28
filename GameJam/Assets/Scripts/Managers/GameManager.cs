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
        private CinemachineTargetGroup.Target[] targets;

        public string CENTER_OF_MAP = "MapCenter";
        public float defaultPlayerWeightToAdjustCamera = 5f;
        
        #region Managers

        public GridManager gridManager;
        private Material _dataNodeMaterial;    
        
        #endregion

        private void Awake() {
            SetsSingleton();
            _dataNodeMaterial = gridManager.GetDataNodeMaterial();
            targets = targetGroup.m_Targets;
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
            for (var i = 0; i < targets.Length; i++)
            {
                if (targets[i].target.name != CENTER_OF_MAP) continue;
                targets[i].target = player.transform;
                targets[i].weight = defaultPlayerWeightToAdjustCamera;
                return;
            }
        }
        
        public Material GetDataNodeMaterial() {
            return gridManager.dataNodeMaterial;
        }
    }
}
