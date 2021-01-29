using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDetector : MonoBehaviour {
    public DataNode.Node parentDatanode;

    private void Awake() {
        parentDatanode = GetComponentInParent<DataNode.Node>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("DataNode")) {
            parentDatanode = GetComponentInParent<DataNode.Node>();
            parentDatanode.DestroyNode();
        }
    }
}
