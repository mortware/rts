using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Tree : MonoBehaviour
{
    private Transform _leaves;
    private Inventory _inventory;
    private Resource _resource;
    
    void Start()
    {
        _leaves = transform.Find("Leaves");
        _inventory = GetComponent<Inventory>();
        _resource = GetComponent<Resource>();
        
        _leaves.gameObject.SetActive(true);
    }

    private void Update()
    {
        // if resource is used up, hide the leaves
        if (_resource.IsUsed)
            _leaves.gameObject.SetActive(false);
        else if (!_resource.IsUsed && _leaves.gameObject.activeSelf)
            _leaves.gameObject.SetActive(true);
    }
}
