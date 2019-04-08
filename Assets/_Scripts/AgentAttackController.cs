using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAttackController : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            collider.GetComponent<PlayerHealthController>().Impact();
        }
    }
}
