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
    public bool CanPickUp;
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

    public bool TriggerMenu()
    {
        return _input.menu;
    }

    private void CheckForPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(this.transform.position, this.transform.forward), out hit, 5))
        {
            CanPickUp = (hit.collider.CompareTag("Pickup")) ? true : false;
            if (CanPickUp && _input.activating2)
            {
                PickupBehaviour behaviour = hit.collider.GetComponent<PickupBehaviour>();
                behaviour.TriggerPickup();
            }
        }
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

        CheckForPickup();
    }
}
