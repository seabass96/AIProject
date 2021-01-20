using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSencing : MonoBehaviour
{
    public float x;
    public float z;

    public bool HasRedFlag()
    {
        Collider[] stuffOnTeamBase = Physics.OverlapBox(transform.position, new Vector3(x/2, 5, z/2));
        foreach (Collider hit in stuffOnTeamBase)
        {
            if (hit.transform.gameObject.name == "Red Flag")
            {
                return true;
            }
        }
        return false;
    }
    
    public bool HasBlueFlag()
    {
        Collider[] stuffOnTeamBase = Physics.OverlapBox(transform.position, new Vector3(x/2, 5, z/2));
        foreach (Collider hit in stuffOnTeamBase)
        {
            if (hit.transform.gameObject.name == "Blue Flag") return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(x, 5, z));
    }

}
