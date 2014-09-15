using System;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(ScrollingText))]
public class Store : MonoBehaviour
{

    private Inventory _inventory;

    void Start()
    {
        _inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        /*for (int i = 0; i < _inventory.Slots.Count(); i++)
        {
            var slot = _inventory.Slots[i];

            switch (slot.Item)
            {
                case ItemType.Wood:

                    var obj = Instantiate(LogStack, transform.position, Quaternion.identity);
                    
                    break;
                case ItemType.Iron:
                    break;
            }
        }*/
    }
}
