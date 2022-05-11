using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerLogicController : MonoBehaviour
{
    private GameObject _player;
    private MainUILogic _uiLogic;
    private GameObject _flashlight;
    private StarterAssetsInputs _input;
    private bool _flashlightOn;
    private float _flashActivateCooldown = 0.25f;
    private Animator _animator;
    public bool CanPickUp;
    public string PickUpAction;

    private List<string> ItemsHeld;
    private float _checkForPickupTimer = 0;
    private float _checkForPickupTimerMax = 0.35f;
    private float _checkForPickupTimerMin = 0.1f;
    public List<string> GetItemsHeld() => ItemsHeld;

    public void AddItem(string NameID)
    {
        ItemsHeld.Add(NameID);
    }

    public void RemoveItem(string NameID)
    {
        ItemsHeld.Remove(NameID);
    }
    public bool HasItem(string NameID) => ItemsHeld.Contains(NameID);

    public bool HoldingItem(string NameID) => NameID == _uiLogic.GetHoldingID();

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

        _uiLogic = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUILogic>();
    }

    public bool TriggerMenu()
    {
        return _input.menu;
    }

    public bool FlashlightOn() => _flashlightOn;

    private void CheckForPickup()
    {
        CanPickUp = false;
        RaycastHit hit;
        if (Physics.Raycast(new Ray(this.transform.position, this.transform.forward), out hit, 1.5f))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable i = hit.collider.GetComponent<Interactable>();
                CanPickUp = true;
                i.TriggerLook();
                PickUpAction = i.GetActionText();
                if (Mouse.current.leftButton.isPressed)
                {
                    i.Trigger();
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _flashActivateCooldown -= Time.deltaTime;
        if(Mouse.current.rightButton.isPressed && _flashActivateCooldown < 0)
        {
            _flashActivateCooldown = 0.25f;
            _flashlight.GetComponentInChildren<Light>().enabled = !_flashlight.GetComponentInChildren<Light>().enabled;
            _animator.SetBool("Activated", _flashlight.GetComponentInChildren<Light>().enabled);
            _animator.SetTrigger("Switch");
        }

        _checkForPickupTimer -= Time.deltaTime;
        if (_checkForPickupTimer < 0 && CanPickUp)
        {
            CheckForPickup();
            _checkForPickupTimer = _checkForPickupTimerMin;
        }
        else if (_checkForPickupTimer < 0 && !CanPickUp)
        {
            CheckForPickup();
            _checkForPickupTimer = _checkForPickupTimerMax;
        }
    }
}
