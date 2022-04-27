using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasterObj
{
    public Ray _ray;
    public RaycastHit _hit;

    public RaycasterObj(Ray _r, RaycastHit _h)
    {
        _ray = _r;
        _hit = _h;
    }

    public T GetActive<T>() => (!_hit.collider) ? (T)(object)_ray : (T)(object)_hit;
}

public class RaycasterTool : MonoBehaviour
{
    private float TAU = Mathf.PI * 2;

    public float Resolution = 47.8f;
    public float RefreshRate;
    public float Dist = 1000;
    public LayerMask rayMask;
    private float _timer = 0f;
    private List<Ray> _raycasters;
    private List<RaycastHit> _rayhits;
    private List<RaycasterObj> _castObjects;

    // Start is called before the first frame update
    void Start()
    {
        TAU = Mathf.PI * 2;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            _timer = RefreshRate;
            _castObjects = new List<RaycasterObj>();
            _rayhits = new List<RaycastHit>();
            _raycasters = new List<Ray>();
            Ray lastRay = new Ray(this.transform.position, Vector3.forward);
            for (float i = 0; i < Resolution; i++)
            {
                Vector3 newDir = (Quaternion.LookRotation(lastRay.direction) * new Vector3(Mathf.Cos(TAU / Resolution), 0, Mathf.Sin(TAU / Resolution)));
                Ray r = new Ray(this.transform.position, new Vector3(newDir.x, 0, newDir.z));
                lastRay = r;
                _raycasters.Add(r);
            }

            foreach (Ray r in _raycasters)
            {
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, Dist, rayMask))
                {
                    _castObjects.Add(new RaycasterObj(new Ray() , hit));
                    _rayhits.Add(hit);
                }
                else
                {
                    _castObjects.Add(new RaycasterObj(r, new RaycastHit()));
                }
            }
        }
    }

    public List<RaycasterObj> GetRaycast()
    {
        return _castObjects;
    }

    public List<Vector3> AllHitAdjPositions()
    {
        List<Vector3> retList = new List<Vector3>();


        for(int i= 1; i < _castObjects.Count - 2; i++)
        {
            if (!_castObjects[i]._hit.collider)
            {
                if(_castObjects[i - 1]._hit.collider)
                {
                    Vector3 pos = _castObjects[i - 1]._hit.point - _castObjects[i]._ray.GetPoint(_castObjects[i - 1]._hit.distance*1.5f);
                    Logger.Log(pos);
                    retList.Add(pos);
                }
            }

            if (!_castObjects[i-1]._hit.collider)
            {
                if (_castObjects[i]._hit.collider)
                {
                    Vector3 pos = _castObjects[i]._hit.point - _castObjects[i - 1]._ray.GetPoint(_castObjects[i]._hit.distance*1.5f);
                    Logger.Log(pos);
                    retList.Add(pos);
                }
            }
        }
        return retList;
    }

    private void OnDrawGizmos()
    {
        if (Logger.Active)
        {
            foreach (RaycasterObj r in _castObjects)
            {
                Gizmos.color = new Color(1, 0, 0, 1);
                Gizmos.DrawRay(transform.position, (r._hit.point - transform.position).normalized * (r._hit.distance));
                Gizmos.color = new Color(1, 1, 1, 0.5f);
                Gizmos.DrawRay(r._ray.origin, r._ray.direction * Dist);

                Gizmos.color = new Color(1, 1, 0, 1f);
                Gizmos.DrawWireSphere(r._hit.point, 0.1f);
            }

            List<Vector3> corners = AllHitAdjPositions();

            foreach(Vector3 v in corners)
            {
                Gizmos.color = new Color(0, 0, 1, 1f);
                Gizmos.DrawWireSphere(v, 0.5f);
            }
        }
    }
}
