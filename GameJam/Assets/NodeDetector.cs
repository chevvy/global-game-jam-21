using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDetector : MonoBehaviour {
    public DataNode.DataNode parentDatanode;

    private void Awake() {
        parentDatanode = GetComponentInParent<DataNode.DataNode>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("DataNode")) {
            parentDatanode = GetComponentInParent<DataNode.DataNode>();
            parentDatanode.DestroyNode();
        }
        
    }
}
