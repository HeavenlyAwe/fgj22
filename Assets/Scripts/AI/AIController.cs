using System;
using System.Collections;
using System.Collections.Generic;
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
        Navigate,
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
    public float fightRadius = 5.0f;

    [Header("Follower Prefabs")]
    public GameObject neutralFollower;
    public GameObject fireFollower;
    public GameObject waterFollower;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _anchorPoint;
    private Transform _playerTransform;
    private Collider[] _fightRadiusColliders;
    private Transform _foundEnemy = null;
    private Vector3 _targetPosition;

    private bool hasEnemyAggro = false;

    public GameObject fightingCloudPrefab;
    public float fightCloudOffsetTowardsCamera = 2.5f;

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
        // Scan for followers of other factions to fight (unless neutral or already fighting)
        if (faction != Faction.Neutral && (state != State.Fight || state == State.Die))
        {
            ScanForEnemies();
        }
        else if (state == State.Fight)
        {
            Vector3 relativePos = Camera.main.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

            Vector3 fightPosition = transform.position;

            if (_foundEnemy != null)
            {
                float distance = (_foundEnemy.position - transform.position).magnitude;
                if (distance < 1.0f)
                {
                    fightPosition = (_foundEnemy.position + transform.position) / 2;

                    GameObject fightingCloud = Instantiate(fightingCloudPrefab, fightPosition + relativePos.normalized * fightCloudOffsetTowardsCamera, rotation);
                    ChangeState(State.Die);
                }
            }
            else
            {
                ChangeState(State.Idle);
                //GameObject fightingCloud = Instantiate(fightingCloudPrefab, fightPosition + relativePos.normalized * fightCloudOffsetTowardsCamera, rotation);
                //ChangeState(State.Die);
            }
        }

        // If the state has been set to fight but no fightable enemy is to be found, return to following
        if (state == State.Fight && _foundEnemy == null) state = State.Follow;

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
            case State.Navigate:
                NavigateToPosition();
                break;
            case State.Fight:
                foreach (Transform child in transform)
                {
                    child.GetComponent<Animator>().SetBool("isAngry", true);
                }
                Fight();
                break;
            case State.Die:
                Die();
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

        FactionSizeCounter.Remove(faction);
        FactionSizeCounter.Add(newFaction);

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
        faction = newFaction;

        // On a faction change, start immediately following new leader
        ChangeState(State.Follow);
    }

    public void ChangeState(State newState)
    {
        _navMeshAgent.ResetPath();
        switch (newState)
        {
            case State.Idle:
                break;
            case State.Roam:
                _navMeshAgent.stoppingDistance = 1.0f;
                break;
            case State.Follow:
                _navMeshAgent.stoppingDistance = 4.0f; // 2.5f;
                break;
            case State.Navigate:
                _navMeshAgent.stoppingDistance = 1.0f;
                break;
            case State.Fight:
                _navMeshAgent.stoppingDistance = 0.0f;
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

    // Function for scanning surroundings for other followers
    private void ScanForEnemies()
    {
        _fightRadiusColliders = Physics.OverlapSphere(transform.position, fightRadius);
        Transform foundFollower = null;
        foreach (var hitCollider in _fightRadiusColliders)
        {
            // Stop searching if another entity found me
            if (hasEnemyAggro)
            {
                return;
            }

            // Check if the found collider is a follower
            if (hitCollider.gameObject.CompareTag("Follower")) { foundFollower = hitCollider.transform; }
            else { continue; }

            // Get the found follower AIController
            var foundFollowerAic = foundFollower.GetComponent<AIController>();

            // If the found follower was neutral, skip it
            if (foundFollowerAic.faction == Faction.Neutral) continue;

            // If the found follower is of other faction, "fight" the follower
            if (foundFollowerAic.faction != faction)
            {
                if (!foundFollowerAic.hasEnemyAggro)
                {
                    ChangeState(State.Fight);
                    _foundEnemy = foundFollower;
                    foundFollowerAic.SetFoundEnemy(this);
                    hasEnemyAggro = true;
                }
            }
        }
    }

    private void Fight()
    {
        if (_foundEnemy == null) return;

        // Update the _navMeshAgent destination to the enemy position
        _navMeshAgent.destination = _foundEnemy.position;
    }

    private void NavigateToPosition()
    {
        _navMeshAgent.destination = _targetPosition;
    }

    IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.75f);
        FactionSizeCounter.Remove(faction);
        Destroy(this.gameObject);
    }

    // Function for killing off this follower
    private void Die()
    {
        StartCoroutine(DieAnimation());
    }

    public void SetFoundEnemy(AIController other)
    {
        _foundEnemy = other.transform;
        ChangeState(State.Fight);
        hasEnemyAggro = true;
    }


    IEnumerator MoveIntoPosition(Vector3 position)
    {
        _targetPosition = position;
        ChangeState(State.Navigate);
        yield return new WaitForSeconds(1.0f);
        ChangeState(State.Idle);
    }

    public void StayBehind(Vector3 position)
    {
        Debug.Log(gameObject.name + " staying behind");
        StartCoroutine(MoveIntoPosition(position));
    }
}
