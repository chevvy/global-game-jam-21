using System;
using System.Collections;
using Managers;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DataNode {
    public class DataNode : MonoBehaviour {
        public bool IsDataNode { set; get; }
        public int NodeID { get; set; }
        public float dataNodeMass = 5f;
        public Material dataNodeMaterial;
        public GameObject nodePrefab;

        public GridManager gridManager;

        public float targetYPosition;
        private bool _isCheckingForFloor;

        private void Update() {
            if (!_isCheckingForFloor) return;
            // Debug.Log(_initialYPosition - transform.position.y);
            if (transform.position.y - 0.2f <= targetYPosition) {
                GetComponent<Rigidbody>().isKinematic = true;
                Destroy(GetComponent<Rigidbody>());
                _isCheckingForFloor = false;
            }
        }

        public void SpawnDataNode() {
            var newNode = InstantiateNewNode();
            SetNodeRigidBodyProperties(newNode);
            SetNodeVisuals(newNode);
            var dataNode = newNode.AddComponent<DataNode>();
            dataNode.StartCheckingForFloor(transform.position.y);
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

        public void DestroyNode() {
            Destroy(gameObject);
        }
    }
}
