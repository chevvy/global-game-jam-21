using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace DataNode {
    public class Node : MonoBehaviour {
        public bool IsDataNode { set; get; }
        public int NodeID { get; set; }
        public float dataNodeMass = 5f;
        public Material dataNodeMaterial;
        [FormerlySerializedAs("defaultNodeMaterial")] public Material nodeMaterial;
        public GameObject nodePrefab;
        public GameObject lootableDataNodePrefab;
        public float LootableDataNodeRadius;
        public float LootableDataNodeHeight;

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
            dataNode.lootableDataNodePrefab = lootableDataNodePrefab;
            dataNode.LootableDataNodeHeight = LootableDataNodeHeight;
            dataNode.LootableDataNodeRadius = LootableDataNodeRadius;
        }

        /// <summary>
        /// Change the status of the nodes and sends info to the game manager
        /// </summary>
        /// <param name="playerID">the player ID</param>
        public void DigDataNode(int playerID = 0) {
            // Breaks node 
            // spawn random ore ? 
            ResetNodeStatus();
            SpawnLootableDataNode();
        }
        
        private void SpawnLootableDataNode() {
            Random random = new Random();
            Random randomAngle = new Random();
            int numberOfNewDataNode = random.Next(0, 5);
            for (int i = 0; i < numberOfNewDataNode; i++) {
                Vector3 newPosition = GetRandomPositionAroundNode(randomAngle);
                Instantiate(lootableDataNodePrefab, newPosition, Quaternion.identity);    
            }
        }

        private Vector3 GetRandomPositionAroundNode(Random randomGenerator) {
            float radius = LootableDataNodeRadius;
            float angle = randomGenerator.Next(0, 360);
            Vector3 newPosition;
            newPosition.x = transform.position.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            newPosition.y = transform.position.y + LootableDataNodeHeight;
            newPosition.z = transform.position.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            return newPosition;
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
