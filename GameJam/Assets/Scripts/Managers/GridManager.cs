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
        
        public GameObject floor;
        private List<GameObject> tiles;

        public int numberOfDataNode = 20;
        private HashSet<int> indexesOfDataNode;
        public int dataNodeHeightAtSpawn = 100;
        void Awake() {
            CheckForMissingComponents();
            InitializeFloorStructure();
            GenerateRandomDataNodes(numberOfDataNode);
            SpawnDataNode(indexesOfDataNode);
        }

        private void GenerateRandomDataNodes(int numberOfNodes) {
            Random randomDataNodeBlocks = new Random();
            for (int i = 0; i < numberOfNodes; i++) {
                int tileNumber = randomDataNodeBlocks.Next(0, tiles.Count);
                indexesOfDataNode.Add(tileNumber);
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
        }


        public void SpawnDataNode(HashSet<int> indexes) {
            foreach (int index in indexes) {
                DataNode.DataNode currentNode = tiles[index].GetComponent<DataNode.DataNode>();
                currentNode.ActivateDataNode();
            }
        }
        private void InitializeFloorStructure() {
            tiles = new List<GameObject>();
            indexesOfDataNode = new HashSet<int>();
            int tileIndex = 0;
            foreach (Transform child in floor.transform) {
                GameObject tile = child.gameObject;
                tile.AddComponent<DataNode.DataNode>();
                DataNode.DataNode node = tile.GetComponent<DataNode.DataNode>();

                node.IsDataNode = false;
                node.NodeID = tileIndex;
                node.dataNodeMaterial = dataNodeMaterial;
                tiles.Add(tile);

                tileIndex++;
            }
        }
    }
}
