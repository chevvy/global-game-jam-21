using System;
using Managers;
using UnityEngine;

namespace DataNode {
    public class DataNode : MonoBehaviour {
        public bool IsDataNode { set; get; }
        public int NodeID { get; set; }
        public float dataNodeMass = 5f;
        public Material dataNodeMaterial;
        public GameObject nodePrefab;

        public GridManager gridManager;

        public void SpawnDataNode() {
            var newNode = InstantiateNewNode();
            SetNodeRigidBodyProperties(newNode);
            SetNodeVisuals(newNode);
        }

        private GameObject InstantiateNewNode() {
            var position = transform.position;
            Vector3 spawnPosition = new Vector3(position.x,
                position.y + gridManager.dataNodeHeightAtSpawn, position.z);
            var newNode = Instantiate(nodePrefab, position, Quaternion.identity);
            newNode.transform.position = spawnPosition;
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
    }
}
