using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : PickupI
{
    public GameObject Obj;
    public void Grab()
    {
        GameObject.Destroy(Obj);
    }

    public KeyPickup(GameObject PI)
    {
        Obj = PI;
    }
}
