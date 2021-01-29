using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace DataNode {
    public class DataNode : MonoBehaviour {
        public bool IsDataNode { set; get; }
        public int NodeID { get; set; }
        public float dataNodeMass = 5f;
        public Material dataNodeMaterial;
        public Material defaultNodeMaterial;
        public GameObject nodePrefab;

        public GridManager gridManager;

        public float targetYPosition;
        private bool _isCheckingForFloor;

        private void Update() {
            if (!_isCheckingForFloor) return;
            // Debug.Log(_initialYPosition - transform.position.y);
            // if (transform.position.y - 0.2f <= targetYPosition) {
            //     GetComponent<Rigidbody>().isKinematic = true;
            //     Destroy(GetComponent<Rigidbody>());
            //     _isCheckingForFloor = false;
            // }
        }

        public void SpawnDataNode(int nodeID, GridManager gridManager) {
            var newNode = InstantiateNewNode();
            SetNodeRigidBodyProperties(newNode);
            SetNodeVisuals(newNode);
            var dataNode = newNode.AddComponent<DataNode>();
            dataNode.defaultNodeMaterial = defaultNodeMaterial;
            dataNode.StartCheckingForFloor(transform.position.y);
            dataNode.NodeID = nodeID;
            dataNode.gridManager = gridManager;
        }
        
        

        public void StartCheckingForFloor(float targetYposition) {
            targetYPosition = targetYposition;
            StartCoroutine(TimerBeforeCheckingForFloor());
            IEnumerator TimerBeforeCheckingForFloor() {
                yield return new WaitForSeconds(2);
                _isCheckingForFloor = true;
            }
        }

        private GameObject InstantiateNewNode() {
            var position = transform.position;
            Vector3 spawnPosition = new Vector3(position.x,
                position.y + gridManager.dataNodeHeightAtSpawn, position.z);
            var newNode = Instantiate(nodePrefab, position, Quaternion.identity);
            newNode.transform.position = spawnPosition;
            newNode.tag = "DataNode";
            return newNode;
        }

        private void SetNodeRigidBodyProperties(GameObject node) {
            node.AddComponent<Rigidbody>();
            Rigidbody nodeRigidbody = node.GetComponent<Rigidbody>();
            RigidbodyConstraints constraints =
                RigidbodyConstraints.FreezeRotation |
                RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionZ;
            nodeRigidbody.constraints = constraints;
            nodeRigidbody.mass = dataNodeMass;
        }
        
        private void SetNodeVisuals(GameObject node) {
            node.GetComponent<MeshRenderer>().material = dataNodeMaterial;
        }
        
        /// <summary>
        /// Change the status of the nodes and sends info to the game manager
        /// </summary>
        /// <param name="playerID">the player ID</param>
        public void DigDataNode(int playerID = 0) {
            // Breaks node 
            // spawn random ore ? 
            ResetNodeStatus();
        }

        /// <summary>
        /// Reset to a normal node
        /// </summary>
        public void ResetNodeStatus() {
            if(TryGetComponent(out Rigidbody nodeRigidbody)) {
                Destroy(nodeRigidbody);
            }

            IsDataNode = false;
            gridManager.ReplaceNodeInNodesList(this, NodeID);
            GetComponent<MeshRenderer>().material = defaultNodeMaterial;
        }
        
        public void DestroyNode() {
            Destroy(gameObject);
        }
    }
}
