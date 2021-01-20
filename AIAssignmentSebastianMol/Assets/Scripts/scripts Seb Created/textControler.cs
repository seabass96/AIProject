using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textControler : MonoBehaviour
{
    private void Start()
    {
        SetAgentTypeText();
    }

    /// <summary>
    /// sets the test above the agent to show what decision tree he is using
    /// </summary>
    private void SetAgentTypeText()
    {

        switch (transform.parent.parent.GetComponent<AI>().enemyType)
        {
            case enemyType.COMMANDO:
                GetComponent<Text>().text = "COMMANDO";
                break;
            case enemyType.HEALER:
                GetComponent<Text>().text = "HEALER";
                break;
            case enemyType.ASSASIN:
                GetComponent<Text>().text = "ASSASIN";
                break;

        }
    }
}
