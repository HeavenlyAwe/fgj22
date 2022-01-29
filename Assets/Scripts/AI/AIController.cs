using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Experimental.TerrainAPI;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{

    // States for the follower
    public enum State
    {
        Idle,
        Roam,
        Follow,
        Die,
        Fight,
    }

    // Factions that the follower can be part of
    public enum Faction
    {
        Neutral,
        Fire,
        Water,
    }

    [Header("States")]
    public State state = State.Roam;

    public Faction faction = Faction.Neutral;

    [Header("Movement")]
    [Tooltip("Speed with which follower moves")]
    public float followerSpeed = 4.0f;
    [Tooltip("How far the follower can move from its anchor point when roaming")]
    public float roamingRadius = 5.0f;
    [Tooltip("Determines how close followers of other factions can come before getting attacked by this follower")]
    public float fightRadius = 2.0f;

    [Header("Follower Prefabs")]
    public GameObject neutralFollower;
    public GameObject fireFollower;
    public GameObject waterFollower;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _anchorPoint;
    private Transform _playerTransform = null;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _anchorPoint = transform.position;
        _navMeshAgent.speed = followerSpeed;
        ChangeFaction(faction);
        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Scan for followers of other factions
        //Physics.OverlapSphere(transform.position, fightRadius, )
        
        // Movement behaviour for follower
        switch (state)
        {
            case State.Idle:
                break;
            case State.Roam:
                RandomMove();
                break;
            case State.Follow:
                FollowPlayerMove();
                break;
            case State.Die:
                break;
        }
        
    }

    // Function for updating this followers current faction
    public void ChangeFaction(Faction newFaction)
    {
        // Disable old follower faction (lazy code)
        neutralFollower.SetActive(false);
        fireFollower.SetActive(false);
        waterFollower.SetActive(false);

        // Activate the corresponding graphics for the new faction
        switch (newFaction)
        {
            case Faction.Neutral:
                neutralFollower.SetActive(true);
                break;
            case Faction.Fire:
                fireFollower.SetActive(true);
                break;
            case Faction.Water:
                waterFollower.SetActive(true);
                break;
        }
        
        ChangeState(State.Follow);
        faction = newFaction;
    }

    public void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Idle:
                break;
            case State.Roam:
                _navMeshAgent.stoppingDistance = 1.0f;
                break;
            case State.Follow:
                _navMeshAgent.stoppingDistance = 2.5f;
                break;
            case State.Die:
                break;
        }

        state = newState;
    } 

    // Function for moving follower randomly within an "anchor" unit circle
    private void RandomMove()
    {
        // Don't generate new random position to go to, if the previous one hasn't been reached
        if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return;

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
    }

    // Function for moving follower to current faction leader
    private void FollowPlayerMove()
    {
        
        

        // If follower would still try to FollowMove() when neutral, set the state to roam,
        // else find player to follow
        switch (faction)
        {
            case Faction.Neutral:
                _playerTransform = null;
                break;
            case Faction.Fire:
                _playerTransform = GameObject.FindWithTag("Fire").transform;
                break;
            case Faction.Water:
                _playerTransform = GameObject.FindWithTag("Water").transform;
                break;
        }

        // If no player transform was found, return to roaming
        if (_playerTransform == null)
        {
            state = State.Roam;
            return;
        }

        // If within the stopping distance of player look at player (follower stops moving)
        if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance) transform.LookAt(_playerTransform);

            // Update the _navMeshAgent destination to the player position
        _navMeshAgent.destination = _playerTransform.position;
        
        
    }


}
