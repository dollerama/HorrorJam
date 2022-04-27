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
    public float DistFallOff = 100;
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

        for (int i = 0; i < _castObjects.Count - 1; i++)
        {
            if (_castObjects[i]._hit.collider)
            {
                for(int k=1; k < 5; k++)
                {
                    Vector3 dir = -_castObjects[i]._hit.normal * (k);
                    if(!Physics.CheckSphere(_castObjects[i]._hit.point+dir, 0.1f))
                    {
                        Vector3 pos = _castObjects[i]._hit.point + dir;
                        pos.y = this.transform.position.y;
                        retList.Add(pos);
                    }
                }
                
            }
        }
        return retList;
    }

    public List<Vector3> AllHitPositions()
    {
        List<Vector3> retList = new List<Vector3>();

        for (int i = 0; i < _castObjects.Count - 1; i++)
        {
            if (_castObjects[i]._hit.collider)
            {
                Vector3 pos = _castObjects[i]._hit.point;
                pos.y = this.transform.position.y;
                retList.Add(pos);
            }
        }
        return retList;
    }

    public List<Vector3> AllRandomOnRays()
    {
        List<Vector3> retList = new List<Vector3>();


        for (int i = 0; i < _castObjects.Count - 1; i++)
        {
            Vector3 pos = _castObjects[i]._ray.GetPoint(Random.Range(DistFallOff, DistFallOff*2));
            pos.y = this.transform.position.y;
            retList.Add(pos);
        }
        return retList;
    }

    public List<Vector3> CloseOnRays()
    {
        List<Vector3> retList = new List<Vector3>();

        for (int i = 0; i < _castObjects.Count - 1; i++)
        {
            Vector3 pos = _castObjects[i]._ray.GetPoint(Random.Range(1, 5));
            pos.y = this.transform.position.y;
            retList.Add(pos);
        }
        return retList;
    }

    public List<Vector3> HiddenSpawnPosition()
    {
        List<Vector3> cornerAdj = AllHitAdjPositions();
        List<Vector3> retList = new List<Vector3>();

        foreach (Vector3 v in cornerAdj)
        {
            RaycastHit hit;
            Ray r = new Ray(v, (this.transform.position-v).normalized);
            if ((v - this.transform.position).sqrMagnitude > DistFallOff)
            {
                retList.Add(v);
            }
        }

        return retList;
    }

    public List<Vector3> BehindPlayerSpawnPosition()
    {
        List<Vector3> cornerAdj = AllRandomOnRays();
        List<Vector3> retList = new List<Vector3>();

        foreach (Vector3 v in cornerAdj)
        {
            RaycastHit hit;
            Ray r = new Ray(v, (this.transform.position - v).normalized);
            if (!Utility.IsLookingAtObject(this.transform.forward, -r.direction) &&
                (v - this.transform.position).sqrMagnitude > DistFallOff)
            {
                retList.Add(v);
            }
        }

        return retList;
    }

    public List<Vector3> CloseBehindPlayerSpawnPosition()
    {
        List<Vector3> cornerAdj = CloseOnRays();
        List<Vector3> retList = new List<Vector3>();

        foreach (Vector3 v in cornerAdj)
        {
            RaycastHit hit;
            Ray r = new Ray(v, (this.transform.position - v).normalized);
            if (!Utility.IsLookingAtObject(this.transform.forward, -r.direction))
            {
                retList.Add(v);
            }
        }

        return retList;
    }

    public Vector3 RandomHiddenSpawnPos()
    {
        List<Vector3> hS = HiddenSpawnPosition();

        return (hS.Count != 0) ? hS[Random.Range(0, hS.Count)] : Vector3.zero;
    }

    public Vector3 RandomBehindPlayerSpawnPos()
    {
        List<Vector3> hS = BehindPlayerSpawnPosition();

        return (hS.Count != 0) ? hS[Random.Range(0, hS.Count)] : Vector3.zero;
    }

    public Vector3 RandomCloseBehindPlayerSpawnPos()
    {
        List<Vector3> hS = CloseBehindPlayerSpawnPosition();

        return (hS.Count != 0) ? hS[Random.Range(0, hS.Count)] : Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (Logger.Active)
        {
            if (_castObjects.Count == 0) return;
            
            foreach (RaycasterObj r in _castObjects)
            {
                Gizmos.color = new Color(1, 0, 0, 0.5f);
                Gizmos.DrawRay(transform.position, (r._hit.point - transform.position).normalized * (r._hit.distance));
                Gizmos.color = new Color(0, 1, 0, 0.1f);
                Gizmos.DrawRay(r._ray.origin, r._ray.direction * Dist);

                Gizmos.color = new Color(1, 1, 0, 1f);
                Gizmos.DrawWireSphere(r._hit.point, 0.1f);
            }

            List<Vector3> corners = HiddenSpawnPosition();

            foreach(Vector3 v in corners)
            {
                Gizmos.color = new Color(1, 0, 0, 1);
                Gizmos.DrawWireSphere(v, 0.2f);
            }

            List<Vector3> behind = BehindPlayerSpawnPosition();

            foreach (Vector3 v in behind)
            {
                Gizmos.color = new Color(0, 1, 0, 1);
                Gizmos.DrawWireSphere(v, 0.2f);
            }

            List<Vector3> close = CloseBehindPlayerSpawnPosition();

            foreach (Vector3 v in close)
            {
                Gizmos.color = new Color(0, 1, 0, 1);
                Gizmos.DrawWireSphere(v, 0.2f);
            }
        }
    }
}
