using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : PickupBehaviour, PickupI
{
    private GameObject Obj;
    private GameObject ObjMesh;
    private GameObject VFX;

    public void Awake()
    {
        Init();
    }

    public override void Grab()
    {
        base.Grab();

        Obj.GetComponent<ParticleSystem>().Play();
        Obj.GetComponent<SphereCollider>().enabled = false;

        GameObject.Destroy(ObjMesh);
        VFX.GetComponent<PickupVFXcontroller>().Kill();
        GameObject.Destroy(Obj, 7);
    }

    public override void Init()
    {
        base.Init();

        Obj = gameObject;
        ObjMesh = GetComponentInChildren<MeshRenderer>().gameObject;
        VFX = GetComponentInChildren<PickupVFXcontroller>().gameObject;
    }
}
