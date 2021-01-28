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
        
        public GameObject floor;
        private List<GameObject> tiles;

        public int numberOfDataNode = 20;
        private HashSet<int> indexesOfDataNode;
        public int dataNodeHeightAtSpawn = 100;
        void Awake() {
            if (dataNodeMaterial == null) {
                Debug.LogError("Missing ore material on Gridmanager");
            }

            if (floor == null) {
                Debug.LogError("Missing floor assignment on Gridmanager");
            }

            InitializeFloorStructure();

            indexesOfDataNode = new HashSet<int>();
            Random randomDataNodeBlocks = new Random();
            for (int i = 0; i < numberOfDataNode; i++) {
                int tileNumber = randomDataNodeBlocks.Next(0, tiles.Count);
                indexesOfDataNode.Add(tileNumber);
                Debug.Log("added tile = " + tileNumber);
            }
            foreach (int index in indexesOfDataNode) {
                tiles[index].GetComponent<DataNode.DataNode>().ActivateDataNode();
            }
            
        }

        private void InitializeFloorStructure() {
            tiles = new List<GameObject>();
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

        public Material GetDataNodeMaterial() {
            return dataNodeMaterial;
        }
    }
}
