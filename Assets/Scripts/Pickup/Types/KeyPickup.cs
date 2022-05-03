using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : PickupI
{
    public GameObject Obj;
    public GameObject ObjMesh;
    public GameObject VFX;

    public void Grab()
    {
        Obj.GetComponent<ParticleSystem>().Play();
        Obj.GetComponent<SphereCollider>().enabled = false;
        GameObject.Destroy(ObjMesh);
        VFX.GetComponent<PickupVFXcontroller>().Kill();
        GameObject.Destroy(Obj, 7);
    }

    public KeyPickup(GameObject PI, GameObject OM, GameObject vfx)
    {
        Obj = PI;
        ObjMesh = OM;
        VFX = vfx;
    }
}
