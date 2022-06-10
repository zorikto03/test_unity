using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cylinderAgent : MonoBehaviour
{
    public Transform goal;
    public Transform goal2;

    // Start is called before the first frame update
    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    private void Update()
    {
        if (agent.isPathStale)
        {
            agent.SetDestination(goal2.position);
        }
    }
}
