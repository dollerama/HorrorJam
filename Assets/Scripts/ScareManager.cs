using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utility
{
    static public bool IsLookingAtObject(Vector3 dir1, Vector3 dir2) => (Vector3.Dot(dir1, dir2) < 0) ? false : true;
}

[System.Serializable]
public enum ScarePositionType
{
    Hidden,
    BehindPlayer
}

[System.Serializable]
public class ScareObject
{
    public ScarePositionType ScarePosType;
    public GameObject Template;
    [Range(0, 1)]
    public float Chance;
    public float Life;
    public ScareObject(GameObject T, ScarePositionType Spt, float c, float l = 5)
    {
        Life = l;
        Template = T;
        ScarePosType = Spt;
        Chance = c;
    }

    private Vector3 GetPositionFromType(RaycasterTool PlayerCaster)
    {
        switch(ScarePosType)
        {
            case ScarePositionType.BehindPlayer: return PlayerCaster.RandomBehindPlayerSpawnPos();
            case ScarePositionType.Hidden: return PlayerCaster.RandomHiddenSpawnPos(); 
        }
        return Vector3.zero;
    }

    public void SpawnWithChance(RaycasterTool PlayerCaster)
    {
        if (Random.Range(0, 100) <= (Chance*10))
        {
            GameObject gh = GameObject.Instantiate(Template, GetPositionFromType(PlayerCaster), Quaternion.identity);
            GameObject.Destroy(gh, Life);
        }
    }
}

public class ScareManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject Player;
    public Vector3 AreaDimensions;
    public Vector3 Origin;
    public Vector2 AreaSize;
    [HideInInspector]
    public GameObject ActiveGridCell;
    [Range(0, 1)]
    public float Intensity = 0;
    public Vector2 RateMinMax;

    private GameObject[,] Areas;
    private GridListener ActiveGridCellListener;
    private RaycasterTool PlayerCaster;
    private float _RefreshRate = 0;
    private float _RefreshTimer = 0;

    public List<ScareObject> ScareObjects;

    // Start is called before the first frame update
    void Start()
    {
        Logger.Active = true;
        PlayerCaster = Camera.main.GetComponent<RaycasterTool>();
        Areas = new GameObject[(int)AreaSize.x,(int)AreaSize.y];

        Vector3 _origin = Origin;

        for(int i=0; i < AreaSize.x; i++)
        {
            for(int j=0; j < AreaSize.y; j++)
            {
                GameObject tmp = new GameObject($"Area{i}_{j}");
                tmp.transform.SetParent(this.transform);

                BoxCollider coll = tmp.AddComponent<BoxCollider>();
                coll.size = AreaDimensions;
                coll.isTrigger = true;

                //add a listener to each grid cell. this will tell the manager which cell the player is in
                GridListener gl = tmp.AddComponent<GridListener>();
                gl.AddAction((tmp) => {
                    ActiveGridCell = tmp;
                    ActiveGridCellListener = tmp.GetComponent<GridListener>();
                });

                tmp.transform.position = _origin;
                _origin.x += AreaDimensions.x;
            }
            _origin.x = Origin.x;
            _origin.z += AreaDimensions.z;
        }

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Tick()
    {
        for (int i = 0; i < Intensity * 10; i++)
        {
            foreach (ScareObject so in ScareObjects)
            {
                so.SpawnWithChance(PlayerCaster);
            }
        }
    }

    public void Update()
    {
        _RefreshTimer -= Time.deltaTime;

        if (_RefreshTimer < 0)
        {
            _RefreshRate = Logger.Remap(Intensity, 0, 1, RateMinMax.x, RateMinMax.y);
            _RefreshTimer = _RefreshRate;

            Tick();
        }
    }
}
