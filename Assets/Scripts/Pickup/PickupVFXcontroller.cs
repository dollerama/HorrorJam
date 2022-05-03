using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupVFXcontroller : MonoBehaviour
{
    private float _timer = 0;
    private bool _alive = true;

    // Start is called before the first frame update
    public void Kill()
    {
        _alive = false;
        this.GetComponentInChildren<ParticleSystem>().Stop();
        Destroy(this.gameObject, 6);
    }

    public void Update()
    {
        if(!_alive)
        {
            MeshRenderer[] matRenderers = this.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer mr in matRenderers)
            {
                mr.material.SetFloat("_Fade", 1-_timer);
                _timer += Time.deltaTime / 10;
            }
        }
    }
}
