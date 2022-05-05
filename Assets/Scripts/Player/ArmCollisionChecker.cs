using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCollisionChecker : MonoBehaviour
{
    public LayerMask mask;
    public float distance;
    public float dampening;

    private Vector3 _offset;
    private Vector3 _ogPosition;
    private PlayerLogicController _pLC;

    // Start is called before the first frame update
    void Awake()
    {
        _ogPosition = transform.localPosition;
        _pLC = this.transform.parent.GetComponent<PlayerLogicController>();
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.parent.position - (transform.parent.forward/10), transform.parent.forward);
        Gizmos.color = new Color(1, 0, 0, 1);
        Gizmos.DrawRay(ray);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.parent.position - (transform.parent.forward / 10), transform.parent.forward);
        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            _offset = (transform.parent.forward*-(distance-hit.distance)).normalized;
            _offset.y = 0; _offset.x = 0; _offset.z = -Mathf.Abs(_offset.z);
        }  
        else
        {
            _offset = Vector3.zero;
        }

        transform.localPosition = Vector3.Slerp(transform.localPosition, _ogPosition + (_offset), Time.deltaTime*dampening);
    }
}
