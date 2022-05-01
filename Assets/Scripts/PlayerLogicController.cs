using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerLogicController : MonoBehaviour
{
    private GameObject _player;
    private GameObject _flashlight;
    private StarterAssetsInputs _input;
    private bool _flashlightOn;
    private float _flashActivateCooldown = 0.25f;
    private Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
        _flashlightOn = true;
        _flashActivateCooldown = 0.25f;

        _player = GameObject.FindGameObjectWithTag("Player");
        _input = _player.GetComponent<StarterAssetsInputs>();
        _flashlight = GameObject.FindGameObjectWithTag("Flashlight");
    }

    // Update is called once per frame
    void Update()
    {
        _flashActivateCooldown -= Time.deltaTime;
        if(_input.activating && _flashActivateCooldown < 0)
        {
            _flashActivateCooldown = 0.25f;
            _flashlight.GetComponentInChildren<Light>().enabled = !_flashlight.GetComponentInChildren<Light>().enabled;
            _animator.SetBool("Activated", _flashlight.GetComponentInChildren<Light>().enabled);
            _animator.SetTrigger("Switch");
        }
    }
}
