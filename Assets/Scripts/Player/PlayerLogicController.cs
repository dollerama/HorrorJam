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
    public string PickUpAction;

    private List<string> ItemsHeld;

    public void AddItem(string NameID) => ItemsHeld.Add(NameID);
    public void RemoveItem(string NameID) => ItemsHeld.Remove(NameID);
    public bool HasItem(string NameID) => ItemsHeld.Contains(NameID);

    // Start is called before the first frame update
    void Awake()
    {
        ItemsHeld = new List<string>();
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
        CanPickUp = false;
        RaycastHit hit;
        if (Physics.Raycast(new Ray(this.transform.position, this.transform.forward), out hit, 1.5f))
        {
            
            if (hit.collider.CompareTag("Pickup"))
            {
                CanPickUp = true;
                PickUpAction = "LMB to Collect";
            }
            else if (hit.collider.CompareTag("LockedDoor"))
            {
                CanPickUp = true;
                if (hit.collider.GetComponent<DoorController>())
                {
                    DoorController behaviour = hit.collider.GetComponent<DoorController>();
                    PickUpAction = (behaviour.TryUnlock(ItemsHeld)) ? "LMB to Unlock" : "Door Locked";
                }
            }

            if (CanPickUp && _input.activating2)
            {
                if (hit.collider.GetComponent<PickupBehaviour>())
                {
                    PickupBehaviour behaviour = hit.collider.GetComponent<PickupBehaviour>();
                    behaviour.TriggerPickup();
                }
                else if(hit.collider.GetComponent<DoorController>())
                {
                    DoorController behaviour = hit.collider.GetComponent<DoorController>();
                    MainUILogic mUI = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUILogic>();
                    mUI.RemoveItem( behaviour.Unlock(ItemsHeld) );
                }
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
