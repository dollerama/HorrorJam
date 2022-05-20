using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanLogic : MonoBehaviour
{
    private NavMeshAgent _controller;
    private Animator _animator;
    private Transform _player;
    private NavMeshPath navMeshPathTmp;
    [SerializeField] float speed;
    private bool waitingToMove = false;

    float remap(float val, float f1, float t1, float f2, float t2) => (val - f1) / (t1 - f1) * (t2 - f2) + f2;
   
    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _controller.speed = speed;

        navMeshPathTmp = new NavMeshPath();
    }

    void SetSpeed()
    {
        _controller.speed = speed;
        _animator.SetFloat("Movement", _controller.velocity.magnitude);
    }

    void FindHidingSpot()
    {
        waitingToMove = true;
        int kill = 100;
        Vector3 newSpot = ScareManager.Instance.GetHidingSpot();

        do
        {
            newSpot = ScareManager.Instance.GetHidingSpot();
            _controller.CalculatePath(newSpot, navMeshPathTmp);
            kill--;
        } while (navMeshPathTmp.status != NavMeshPathStatus.PathComplete && kill > 0);

        _controller.SetDestination(newSpot);

        this.Invoke(() => {
            if (!waitingToMove) return;
            _controller.SetDestination(newSpot);
            waitingToMove = false;
        }, UnityEngine.Random.Range(7.0f, 10.0f));
    }

    // Update is called once per frame
    void Update()
    {
        SetSpeed();
        if (Utility.IsLookingAtObject(_player.forward, _player.position, this.transform.position))
        {
            _controller.velocity = Vector3.zero;
            waitingToMove = false;
        }
        else if(_controller.velocity.magnitude <= 0.1f && !waitingToMove)
        {
            FindHidingSpot();
        }
    }
}
