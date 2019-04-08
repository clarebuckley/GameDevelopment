using System;
using UnityEngine;
using static AgentController;

public class AgentStateController : MonoBehaviour
{
    [Serializable]
    public struct AgentState
    {
        public Vector3 position;
        public Quaternion rotation;
        public string[] waypoints;

        public AgentState(Vector3 position, Quaternion rotation, string[] waypoints)
        {
            this.position = position;
            this.rotation = rotation;
            this.waypoints = waypoints;
        }
    }

    public AgentState ToRecord()
    {
        return new AgentState(transform.position, transform.rotation, transform.GetComponent<AgentController>().GetWaypoints());
    }
}
