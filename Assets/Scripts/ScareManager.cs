using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareManager : MonoBehaviour
{
    public GameObject Player;
    public Vector3 AreaDimensions;
    public Vector3 Origin;
    public Vector2 AreaSize;
    private GameObject[,] Areas;

    public GameObject ActiveGridCell;
    private GridListener ActiveGridCellListener;


    // Start is called before the first frame update
    void Start()
    {
        Logger.Active = true;

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
}
