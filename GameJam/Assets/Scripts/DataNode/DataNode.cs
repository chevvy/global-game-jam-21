using System;
using Managers;
using UnityEngine;

namespace DataNode {
    public class DataNode : MonoBehaviour {
        public bool IsDataNode { set; get; }
        public int NodeID { get; set; }
        public Material dataNodeMaterial;
        
        void Awake() {

        }

        public void ActivateDataNode() {
            // SetNodeRigidBodyProperties();
            SetNodeVisuals();
        }

        private void SetNodeVisuals() {
            GetComponent<MeshRenderer>().material = dataNodeMaterial;
        }

        private void SetNodeRigidBodyProperties() {
            gameObject.AddComponent<Rigidbody>();
            Rigidbody nodeRigidbody = GetComponent<Rigidbody>();
            RigidbodyConstraints constraints =
                RigidbodyConstraints.FreezeRotation |
                RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionZ;
            nodeRigidbody.constraints = constraints;
        }
    }
}
