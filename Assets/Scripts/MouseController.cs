using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TileMap))]
public class MouseController : MonoBehaviour
{
    public GameObject Tree;

    private RaycastHit _hit;
    private TileMap _map;
    private GameObject _tileCursor;

 //   private Unit[] _selectedUnits;

    private Vector3 _mouseDownPoint;

    void Awake()
    {
        _mouseDownPoint = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {
        _tileCursor = GameObject.Find("Tile Cursor");

        _map = GetComponent<TileMap>();
        if (_map == null)
            throw new Exception("TileMap not found");
    }

    // Update is called once per frame 
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(0))
                _mouseDownPoint = _hit.point;

            if (_hit.collider.gameObject.tag == "Unit")
            {
                var unit = _hit.collider.transform.Find("Selector").gameObject;
                unit.SetActive(true);
            }

            if (_hit.collider.gameObject.tag == "Terrain")
            {
                var tileCenter = _map.GetTileCenter(_hit.point);
                _tileCursor.transform.position = tileCenter;

                if (Input.GetMouseButtonUp(1))
                {
                    Instantiate(Tree, tileCenter, Quaternion.identity);
                }
            }
        }
    }


    #region Helpers

    private bool DidUserClickLeftMouse(Vector3 hitPosition)
    {
        const float clickZone = 0.8f;
        if ((_mouseDownPoint.x < hitPosition.x + clickZone && _mouseDownPoint.x > hitPosition.x - clickZone)
         && (_mouseDownPoint.y < hitPosition.y + clickZone && _mouseDownPoint.y > hitPosition.y - clickZone)
         && (_mouseDownPoint.z < hitPosition.z + clickZone && _mouseDownPoint.z > hitPosition.z - clickZone))
        {
            return true;
        }
        return false;
    }

    #endregion
}
