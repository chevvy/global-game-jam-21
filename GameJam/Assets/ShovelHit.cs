using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelHit : MonoBehaviour {
    public Transform ShovelTransform;
    public float detectorRadius = 0.5f;
    
    public void CheckForDataNode() {
        Vector3 adjustedPosition = GetAdjustedPosition();
        Collider[] hitColliders = Physics.OverlapSphere(adjustedPosition, detectorRadius);
        foreach (var hitCollider in hitColliders) {
            if (!hitCollider.CompareTag("DataNode")) { continue; }
            Debug.Log("Dig datanode");
            hitCollider.GetComponent<DataNode.DataNode>().DigDataNode();
        }
    }

    Vector3 GetAdjustedPosition() {
        var position = ShovelTransform.position;
        return new Vector3(position.x, position.y, position.z);
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetAdjustedPosition(), detectorRadius);
    }
}
