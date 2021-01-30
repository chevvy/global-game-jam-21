using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public Material dataNodeMaterial;
        [FormerlySerializedAs("defaultMaterial")] public Material nodeMaterial;
        [FormerlySerializedAs("tilePrefab")] public GameObject nodePrefab;
        
        // The floor contains:
        // -> the nodes (aka : floor tiles), theses nodes can be converted to dataNodes
        // -> dataNodes = The ore that the player must mine!
        // -> floorHolder = placed below the tiles, it will be used to handle respawn / collapsing node
        public GameObject floor;
        private List<DataNode.Node> _floorNodes;
        public FloorSystemManager floorSystemManager;

        public GameObject LootableDataNodePrefab;
        public float LootableNodePositionRadius = 2f;
        public float LootableDataNodeHeight = 1f;

        public int numberOfDataNode = 20; // Number of dataNodes that can be mined by players
        private HashSet<int> _indexesOfDataNode; // All the floor node indexes that are dataNode
        public int dataNodeHeightAtSpawn = 10;
        public float dataNodeMass = 30f;
        [FormerlySerializedAs("dataNodeDelayBetweenEachSpawn")]
        public int dataNodeMaxDelayBetweenSpawn = 5;

        void Awake() {
            CheckForMissingComponents();
            InitializeFloorStructure();
            GenerateRandomDataNodes(numberOfDataNode);
            SpawnDataNodes(_indexesOfDataNode);
        }
        
        private void CheckForMissingComponents() {
            if (dataNodeMaterial == null) {
                Debug.LogError("Missing ore material on Gridmanager");
            }

            if (nodeMaterial == null) {
                Debug.LogError("Missing default material on GridManager");
            }

            if (floor == null) {
                Debug.LogError("Missing floor assignment on Gridmanager");
            }

            if (nodePrefab == null) {
                Debug.LogError("Missing tile prefab on GridManager");
            }

            if (floorSystemManager == null) {
                Debug.Log("Missing Floor system manager on GridManager");
            }

            if (LootableDataNodePrefab == null) {
                Debug.Log("Missing LootableDataNodePrefab on GridManager");
            }
        }
        
        private void InitializeFloorStructure() {
            _floorNodes = new List<DataNode.Node>();
            _indexesOfDataNode = new HashSet<int>();
            int tileIndex = 0;
            foreach (Transform child in floor.transform) {
                GameObject childGameObject = child.gameObject;
                DataNode.Node node = childGameObject.AddComponent<DataNode.Node>();

                SetupNewNode(node, tileIndex);
                _floorNodes.Add(node);

                tileIndex++;
            }
        }

        private void SetupNewNode(DataNode.Node node, int tileIndex) {
            node.IsDataNode = false;
            node.NodeID = tileIndex;
            node.dataNodeMaterial = dataNodeMaterial;
            node.nodeMaterial = nodeMaterial;
            node.nodePrefab = nodePrefab;
            node.gridManager = this;
            node.dataNodeMass = dataNodeMass;
            node.lootableDataNodePrefab = LootableDataNodePrefab;
            node.LootableDataNodeHeight = LootableDataNodeHeight;
            node.LootableDataNodeRadius = LootableNodePositionRadius;
        }

        private void GenerateRandomDataNodes(int numberOfNodes) {
            Random random = new Random();
            for (int i = 0; i < numberOfNodes; i++) {
                int dataNodeIndex = random.Next(0, _floorNodes.Count);
                _indexesOfDataNode.Add(dataNodeIndex);
            }
        }
        /// <summary>
        /// Spawns all data nodes received in param
        /// </summary>
        /// <param name="indexes">HashSet of indexes to spawn new DataNode</param>
        public void SpawnDataNodes(HashSet<int> indexes) {
            Random randomRange = new Random();
            foreach (int index in indexes) {
                DataNode.Node currentNode = _floorNodes[index].GetComponent<DataNode.Node>();
                StartCoroutine(delayNodeSpawn());
                IEnumerator delayNodeSpawn() {
                    int secondsBeforeSpawn = randomRange.Next(0, dataNodeMaxDelayBetweenSpawn);
                    yield return new WaitForSeconds(secondsBeforeSpawn);
                    currentNode.SpawnDataNode(index, this);
                }
                
            }
        }
        
        

        public void ReplaceNodeInNodesList(DataNode.Node node, int nodeID) {
            // Memory protection 
            if (_floorNodes[nodeID] != null) {
                Destroy(node.gameObject);
            }
            _floorNodes[nodeID] = node;
        }
    }
}
