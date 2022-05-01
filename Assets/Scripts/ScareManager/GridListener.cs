using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridListener : MonoBehaviour
{
    private System.Action<GameObject> _triggerAction;
    public List<GameObject> Contents;
    private bool _init;

    public void Start()
    {
        Contents = new List<GameObject>();
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        _init = false;
    }

    public void LateUpdate()
    {
        if(!_init && this.GetComponent<BoxCollider>())
        {
            _init = true;
            Collider[] colliders = Physics.OverlapBox(this.GetComponent<BoxCollider>().bounds.center, this.GetComponent<BoxCollider>().bounds.extents);

            foreach (Collider c in colliders)
            {
                if(c.gameObject && !c.gameObject.CompareTag("Player"))
                    Contents.Add(c.gameObject);
            }
        }
    }

    public void AddAction(System.Action<GameObject> a)
    {
        _triggerAction = a;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) _triggerAction.Invoke(this.gameObject);
    }
}
