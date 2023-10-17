using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadFinisher : MonoBehaviour
{
    private Collider col;
    private void Start()
    {
        col = GetComponent<Collider>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out PlayerManager playerManager))
        {
            col.enabled = false;
            playerManager.OnRoadFinish();
        }
    }
}
