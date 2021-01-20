using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSensing : MonoBehaviour
{
    public float x;
    public float z;

    public bool HasHealthKit()
    {
        Collider[] stuffOnTeamBase = Physics.OverlapBox(transform.position, new Vector3(x / 2, 5, z / 2));
        foreach (Collider hit in stuffOnTeamBase)
        {
            if (hit.transform.gameObject.name == "Health Kit")
            {
                return true;
            }
        }
        return false;
    }

    //private void Update()
    //{
    //    if (HasHealthKit()) Debug.Log(gameObject.name + " has health kit ");
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(x, 5, z));
    }
}
