using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmCollisionChecker : MonoBehaviour
{
    public LayerMask mask;
    public float distance;
    public float dampening;
    private Ray _caster;
    private Vector3 _offset;
    private Vector3 _ogPosition;
    private PlayerLogicController _pLC;

    // Start is called before the first frame update
    void Awake()
    {
        _ogPosition = transform.localPosition;
        _pLC = this.transform.parent.GetComponent<PlayerLogicController>();
        _caster = new Ray(transform.parent.position - (transform.parent.forward / 10), transform.parent.forward);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        _caster.origin = (transform.parent.position - (transform.parent.forward / 10));
        _caster.direction = transform.parent.forward;

        if (Physics.Raycast(_caster, out hit, distance, mask))
        {
            _offset = (transform.parent.forward*-(distance-hit.distance)).normalized;
            _offset.y = 0; _offset.x = 0; _offset.z = -Mathf.Abs(_offset.z);
        }  
        else
        {
            _offset = Vector3.zero;
        }

        float dampAmt = Time.smoothDeltaTime * dampening;
        transform.localPosition = Vector3.Slerp(transform.localPosition, _ogPosition + (_offset), dampAmt);
    }
}
