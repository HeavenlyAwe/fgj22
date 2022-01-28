using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{

    // States for the follower
    public enum State
    {
        Roam,
        Follow,
        Die,
    }

    // Factions the follower can follow
    public enum Following
    {
        Neutral,
        Fire,
        Water,
    }

    [Header("States")]
    public State state;
    public Following currentFollowing = Following.Neutral;

    [Header("Movement")] 
    public float followerSpeed = 1.0f;
    public float roamingRadius = 5.0f;

    [Header("Follower Prefabs")] 
    public GameObject neutralFollower;
    public GameObject fireFollower;
    public GameObject waterFollower;
    
    private NavMeshAgent _navMeshAgent;
    private Vector3 _anchorPoint;
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _anchorPoint = transform.position;
        _navMeshAgent.speed = followerSpeed;
        changeFollowing(currentFollowing);
    }

    // Update is called once per frame
    void Update()
    {
        
        // State machine for follower
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

    // Function for updating this followers current following
    public void changeFollowing(Following newFollowing)
    {
        // This is lazy code
        neutralFollower.SetActive(false);
        fireFollower.SetActive(false);
        waterFollower.SetActive(false);
        
        if (newFollowing==Following.Neutral) neutralFollower.SetActive(true);
        if (newFollowing==Following.Fire) fireFollower.SetActive(true);
        if (newFollowing==Following.Water) waterFollower.SetActive(true);

        currentFollowing = newFollowing;

    }
    
    // Function for moving follower randomly within an "anchor" unit circle
    private void RandomMove()
    {
        // Don't generate new random position to go to, if the previous one hasn't been reached
        if (_navMeshAgent.remainingDistance > 0) return;

        Vector2 circleRandomPosition = Random.insideUnitCircle * roamingRadius;
        Vector3 randomPosition = _anchorPoint + new Vector3(circleRandomPosition.x, 0.0f, circleRandomPosition.y);

        // Check for closest position to randomPosition on navmesh. If not found, return.
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            return;
        }

        // If a random point on the navmesh was found, move follower to that position.
        _navMeshAgent.destination = hit.position;
        
        return;
    }

}
