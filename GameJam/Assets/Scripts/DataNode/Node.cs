using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace DataNode {
    public class Node : MonoBehaviour {
        public bool IsDataNode { set; get; }
        public int NodeID { get; set; }
        public float dataNodeMass = 5f;
        public Material dataNodeMaterial;
        [FormerlySerializedAs("defaultNodeMaterial")] public Material nodeMaterial;
        public GameObject nodePrefab;

        public GridManager gridManager;
        
        private bool _isCheckingForFloor;

        private void OnCollisionEnter(Collision other) {
            if (!_isCheckingForFloor) return;
            if (other.gameObject.CompareTag("FloorHolder")) {
                StartCoroutine(WaitForNodeToStabilize());
            }
        }
        /// <summary>
        /// Waits 1 seconds before setting to kinematic the rigidbody to allow it to stabilize
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitForNodeToStabilize() {
            yield return new WaitForSeconds(1);
            GetComponent<Rigidbody>().isKinematic = true;
            _isCheckingForFloor = false;
        }

        public void SpawnDataNode(int nodeID, GridManager gridManagerInstance) {
            var newNode = InstantiateNewNode();
            SetNodeRigidBodyProperties(newNode);
            SetNodeVisuals(newNode);
            SetNodeSpecificOptions(nodeID, gridManagerInstance, newNode);
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
        
        private void SetNodeSpecificOptions(int nodeID, GridManager gridManagerInstance, GameObject newNode) {
            var dataNode = newNode.AddComponent<Node>();
            dataNode.nodeMaterial = nodeMaterial;
            dataNode._isCheckingForFloor = true;
            dataNode.NodeID = nodeID;
            dataNode.gridManager = gridManagerInstance;
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
        private void ResetNodeStatus() {
            if(TryGetComponent(out Rigidbody nodeRigidbody)) {
                Destroy(nodeRigidbody);
            }

            IsDataNode = false;
            gridManager.ReplaceNodeInNodesList(this, NodeID);
            GetComponent<MeshRenderer>().material = nodeMaterial;
        }
        
        public void DestroyNode() {
            Destroy(gameObject);
        }
    }
}
