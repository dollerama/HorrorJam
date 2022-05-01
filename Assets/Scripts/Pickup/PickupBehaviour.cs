using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    Key
}

public class PickupBehaviour : MonoBehaviour
{
    public PickupI _pickup;
    public PickupType _type;
    // Start is called before the first frame update
    void Awake()
    {
        if(_type == PickupType.Key)
        {
            _pickup = new KeyPickup(this.gameObject);
        }
    }

    public void TriggerPickup()
    {
        _pickup.Grab();
    }
}
