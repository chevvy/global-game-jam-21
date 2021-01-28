using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        public Material dataNodeMaterial;
        public Material defaultMaterial;
        public GameObject tilePrefab;
        
        public GameObject floor;
        private List<GameObject> _tiles;

        public int numberOfDataNode = 20;
        private HashSet<int> _indexesOfDataNode;
        public int dataNodeHeightAtSpawn = 10;
        public float dataNodeMass = 30f;

        private void OnCollisionEnter(Collision other) {
            Debug.Log(other.transform.name);
        }

        void Awake() {
            CheckForMissingComponents();
            InitializeFloorStructure();
            GenerateRandomDataNodes(numberOfDataNode);
            SpawnDataNode(_indexesOfDataNode);
        }

        private void GenerateRandomDataNodes(int numberOfNodes) {
            Random randomDataNodeBlocks = new Random();
            for (int i = 0; i < numberOfNodes; i++) {
                int tileNumber = randomDataNodeBlocks.Next(0, _tiles.Count);
                _indexesOfDataNode.Add(tileNumber);
            }
        }

        private void CheckForMissingComponents() {
            if (dataNodeMaterial == null) {
                Debug.LogError("Missing ore material on Gridmanager");
            }

            if (defaultMaterial == null) {
                Debug.LogError("Missing default material on GridManager");
            }

            if (floor == null) {
                Debug.LogError("Missing floor assignment on Gridmanager");
            }

            if (tilePrefab == null) {
                Debug.LogError("Missing tile prefab");
            }
        }


        public void SpawnDataNode(HashSet<int> indexes) {
            foreach (int index in indexes) {
                DataNode.DataNode currentNode = _tiles[index].GetComponent<DataNode.DataNode>();
                currentNode.SpawnDataNode();
            }
        }
        private void InitializeFloorStructure() {
            _tiles = new List<GameObject>();
            _indexesOfDataNode = new HashSet<int>();
            int tileIndex = 0;
            foreach (Transform child in floor.transform) {
                GameObject tile = child.gameObject;
                tile.AddComponent<DataNode.DataNode>();
                DataNode.DataNode node = tile.GetComponent<DataNode.DataNode>();

                node.IsDataNode = false;
                node.NodeID = tileIndex;
                node.dataNodeMaterial = dataNodeMaterial;
                node.nodePrefab = tilePrefab;
                node.gridManager = this;
                node.dataNodeMass = dataNodeMass;

                _tiles.Add(tile);

                tileIndex++;
            }
        }
    }
}
