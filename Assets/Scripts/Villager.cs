using System;
using System.Globalization;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(ScrollingText))]
public class Villager : MonoBehaviour
{
    private const int ItemTransferAmount = 1;           // Number of items to transfer when gathering

    public Job Job = Job.None;                          // Villager's Job
    public float WorkTime = 20.0f;                      // Time (in seconds) it takes for a Work task to be completed
    public GameObject HeldItem;
    public float TargetActiveRange = 1.0f;

    private Transform _compass;
    private bool _selected = false;
    private bool _walking = false;

    private Projector _selector;
    private ScrollingText _scrollingText;

    private CharacterController _characterController;
    private Animator _animator;
    private MovementController _movementController;
    private Inventory _inventory;
    private float _workStart = 0.0f;
    private WorkTask _jobTask;
    private WorkTask _currentTask;

    
    

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
        _scrollingText = GetComponent<ScrollingText>();
        _animator = GetComponent<Animator>();
        _movementController = GetComponent<MovementController>();

        _compass = transform.Find("Compass");
        _characterController = GetComponent<CharacterController>();

        switch (Job)
        {
            case Job.None: return; // Do nothing
            case Job.WoodCutter: _jobTask = WorkTask.ChopWood; break;
            case Job.Miner: _jobTask = WorkTask.Mine; break;
        }
        _currentTask = _jobTask;
    }

    private void Update()
    {
        GameObject target = null;

        // Set task
        //_currentTask = _inventory.IsFull ? WorkTask.Unload : _jobTask;

        switch (_currentTask)
        {
            case WorkTask.ChopWood: target = FindResource(); break;
            case WorkTask.Unload: target = FindStorage(); break;
        }

        if (target == null)
        {
            if (_inventory.HasItems)
                _currentTask = WorkTask.Unload;
            return;
        }
        else
        {
            _movementController.Target = target;
        }

        // If close enough to target
        if (Vector3.Distance(transform.position, _movementController.Target.collider.ClosestPointOnBounds(transform.position)) < TargetActiveRange)
            DoWork();
        else
            _workStart = 0;
    }
    
    private void DoWork()
    {
        switch (_currentTask)
        {
            case WorkTask.ChopWood: Gather(); break;
            case WorkTask.Unload: Unload(); break;
        }
    }

    private void Unload()
    {
        _workStart += Time.deltaTime;

        // Get the inventory
        var targetInventory = _movementController.Target.GetComponent<Inventory>();

        // Drop off any items

        while (_inventory.HasItems)
        {
            ItemType itemType;
            var amount = _inventory.TakeAll(out itemType);
            targetInventory.Give(itemType, amount);
        }
        Destroy(this.HeldItem); 
        if (_inventory.IsEmpty)
        {
            _currentTask = _jobTask;
            _animator.SetBool(Animator.StringToHash("IsCarrying"), false);
        }
            
    }
    private void Gather()
    {
        // Get the inventory
        var targetInventory = _movementController.Target.GetComponent<Inventory>();
        if (targetInventory.TotalItems == 0)
        {
            _animator.SetBool(Animator.StringToHash("IsGathering"), false);
            return;
        }

        _animator.SetBool(Animator.StringToHash("IsGathering"), true);

        _workStart += Time.deltaTime;

        while (_workStart > WorkTime)
        {
            var item = ItemType.Nothing;
            switch (Job)
            {
                case Job.WoodCutter: item = ItemType.Wood; break;
                case Job.Miner: item = ItemType.Iron; break;
            }

            var amount = targetInventory.Take(ItemTransferAmount, item);
            _inventory.Give(item, amount);

            var pos = new Vector3(0.1f, 0.75f, 0);
            
            _workStart -= WorkTime;
        }
        if (_inventory.IsFull)
        {
            _animator.SetBool(Animator.StringToHash("IsGathering"), false);
            _animator.SetBool(Animator.StringToHash("IsCarrying"), true);
            _currentTask = WorkTask.Unload;
        }
    }

    private GameObject FindResource()
    {
        GameObject closest = null;
        foreach (var resource in GameObject.FindGameObjectsWithTag("Resource"))
        {
            //            Debug.Log(resource.name + ": Empty = " + resource.GetComponent<Inventory>().IsEmpty);

            // Skip if it has no resources
            if (resource.GetComponent<Inventory>().IsEmpty)
                continue;

            if (closest == null)
            {
                closest = resource;
                continue;
            }
            var distance = Vector3.Distance(transform.position, resource.transform.position);

            if (distance <= Vector3.Distance(transform.position, closest.transform.position))
                closest = resource;
        }

        return closest;
    }

    private GameObject FindStorage()
    {
        GameObject closest = null;
        foreach (var storage in GameObject.FindGameObjectsWithTag("Storage"))
        {
            // Skip if it is full
            if (storage.GetComponent<Inventory>().IsFull)
                continue;

            if (closest == null)
            {
                closest = storage;
                continue;
            }

            var distance = Vector3.Distance(transform.position, storage.transform.position);

            if (distance <= Vector3.Distance(transform.position, closest.transform.position))
                closest = storage;
        }

        return closest;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 200, 200), "Villager Stats");

        float slotStart = 0;
        const float vspace = 22.0f;
        foreach (var slot in _inventory.Slots)
        {
            GUI.Label(new Rect(20, 40 + slotStart, 80, vspace), string.Format("{0} x {1}", slot.Quantity, slot.Item));
            slotStart += vspace;
        }

        GUI.Label(new Rect(20, 160, 180, vspace), string.Format("Target: {0}", _movementController.Target != null ? _movementController.Target.name : "No target"));
        GUI.Label(new Rect(20, 160 + vspace, 180, vspace), string.Format("Task: {0}", _currentTask));
    }
}
