using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{

    public enum State
    {
        Roam,
        Follow,
        Die,
    }

    [Header("States")]
    public State state;

    [Header("Movement")] 
    public float followerSpeed = 1.0f;
    public float roamingRadius = 5.0f;

    private NavMeshAgent navMeshAgent;
    private Vector3 anchorPoint;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anchorPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Roam:
                RandomMove();
                break;
            case State.Follow:
                break;
            case State.Die:
                break;
        }
    }

    // Function for moving follower randomly within an "anchor" unit circle
    private void RandomMove()
    {
        
        Vector3 randomPosition = anchorPoint + (Vector3) Random.insideUnitCircle * roamingRadius;

        // Check for closest position to randomPosition on navmesh. If not found, return.
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            return;
        }
        
        // If a random point on the navmesh was found, move follower to that position.
        navMeshAgent.destination = hit.position;
        
    }
    
}
