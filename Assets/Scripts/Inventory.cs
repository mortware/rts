using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(ScrollingText))]
public class Inventory : MonoBehaviour
{
    // Villagers will have 1 slot, with a max of 32 items
    // Stores will have 4 slots with a max of 1024 in each

    public int SlotCount = 1;
    public int SlotItemLimit = 32;

    private List<InventorySlot> _slots = new List<InventorySlot>();

    public IList<InventorySlot> Slots
    {
        get { return _slots.ToList(); }
    }

    /// <summary>
    /// Returns the total number of items in all slots
    /// </summary>
    public int TotalItems
    {
        get { return Convert.ToInt32(_slots.Sum(s => s.Quantity)); }
    }

    /// <summary>
    /// Returns if all slots are filled to their limits
    /// </summary>
    public bool IsFull
    {
        get { return _slots.All(s => s.Quantity >= SlotItemLimit); }
    }

    /// <summary>
    /// Returns if any slots have any space available
    /// </summary>
    public bool IsEmpty
    {
        get { return _slots.All(s => s.Quantity <= 0); }
    }

    public bool HasItems
    {
        get { return _slots.Any(s => s.Quantity > 0); }
    }

    private ScrollingText _scrollingText;

    private void Awake()
    {
        _slots = new List<InventorySlot>(SlotCount);
        for (int i = 0; i < SlotCount; i++)
        {
            _slots.Add(new InventorySlot());
        }
    }

    private void Start()
    {
        _scrollingText = GetComponent<ScrollingText>();
    }

    public int Give(ItemType item, int amount)
    {
        var qty = Add(item, amount);
        if (qty > 0)
        {
            _scrollingText.ShowText(string.Format("+{0} {1}", qty, item));
        }
        return qty;
    }

    public int Add(ItemType item, int amount)
    {
        // Check for space
        if (!HasSpaceForItem(item))
            return 0;

        Debug.Log(string.Format("Adding {0} x {1}", amount, item));

        int amountToAdd = amount;
        int amountAdded = 0;
        while (amountToAdd > 0 && HasSpaceForItem(item))
        {
            var slot = _slots.First(s => (s.Item == item && s.Quantity < SlotItemLimit) || s.Item == ItemType.Nothing);
            slot.Item = item;

            int added = Math.Min(SlotItemLimit - slot.Quantity, amountToAdd);
            slot.Quantity += added;

            amountAdded += added;
            amountToAdd -= added;
        }


        return amountAdded;
    }

    /// <summary>
    /// Takes all of any item (one type at a time)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int TakeAll(out ItemType item)
    {
        var itemToRemove = _slots.First(s => s.Quantity > 0).Item;
        var amount = _slots.Where(s => s.Item == itemToRemove).Sum(s => s.Quantity);

        item = itemToRemove;

        var qty = Remove(amount, item);
        if (qty > 0)
        {
            _scrollingText.ShowText(string.Format("-{0} {1}", qty, item));
        }
        return qty;
    }

    public int Take(int amount, out ItemType item)
    {
        var qty = Remove(amount, out item);
        if (qty > 0)
        {
            _scrollingText.ShowText(string.Format("-{0} {1}", qty, item));
        }
        return qty;
    }

    public int Take(int amount, ItemType item)
    {
        var qty = Remove(amount, item);
        if (qty > 0)
        {
            _scrollingText.ShowText(string.Format("-{0} {1}", qty, item));
        }
        return qty;
    }

    /// <summary>
    /// Takes the specified amount of any item
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Remove(int amount, out ItemType item)
    {
        if (IsEmpty)
        {
            item = ItemType.Nothing;
            return 0;
        }

        int amountToRemove = amount;
        int amountRemoved = 0;

        // Get the item to remove
        var itemToRemove = _slots.First(s => s.Quantity > 0).Item;

        while (amountToRemove > 0 && HasItem(itemToRemove))
        {
            var slot = _slots.First(s => s.Item == itemToRemove && s.Quantity > 0);

            int removed = Math.Min(slot.Quantity, amountToRemove);
            slot.Quantity -= removed;

            amountRemoved += removed;
            amountToRemove -= removed;
        }
        item = itemToRemove;

        return amountRemoved;
    }

    /// <summary>
    /// Takes an amount of the specified item from the inventory
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public int Remove(int amount, ItemType item)
    {
        if (IsEmpty || !HasItem(item)) return 0;

        int amountToRemove = amount;
        int amountRemoved = 0;

        while (amountToRemove > 0 && HasItem(item))
        {
            var slot = _slots.First(s => s.Item == item && s.Quantity > 0);

            int removed = Math.Min(slot.Quantity, amountToRemove);
            slot.Quantity -= removed;

            amountRemoved += removed;
            amountToRemove -= removed;
        }
        _scrollingText.ShowText(string.Format("-{0} {1}", amountRemoved, item));
        return amountRemoved;
    }

    /// <summary>
    /// Checks to see if the inventory has space for the specified item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasSpaceForItem(ItemType item)
    {
        return _slots.Any(s => (s.Item == item && s.Quantity < SlotItemLimit) || s.Item == ItemType.Nothing);
    }

    private bool HasItem(ItemType item)
    {
        return _slots.Any(s => s.Item == item && s.Quantity > 0);
    }


    private void OnGUI()
    {
        /*
        var pos = Camera.main.camera.WorldToScreenPoint(transform.position);
        pos.y = Screen.height - pos.y;

        float slotStart = pos.y;
        foreach (var slot in _slots)
        {
            GUI.Label(new Rect(pos.x, slotStart, 80, 20), string.Format("{0} x {1}", slot.Quantity, slot.Item));

            slotStart += 12;
        }
        */
    }
}

public class InventorySlot
{
    public ItemType Item { get; set; }
    public int Quantity { get; set; }
}