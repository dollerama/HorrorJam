using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLogic : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private float _timer = 7;
    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0)
        {
            _controller.Move(transform.forward * 0.7f * Time.deltaTime);
            _animator.SetFloat("Movement", 0.2f);
        }
        else
        {
            _animator.SetFloat("Movement", 0f);
        }
    }
}
