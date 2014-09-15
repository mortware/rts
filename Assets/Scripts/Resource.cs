using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Resource : MonoBehaviour
{
    public bool IsUsed = false;
    public float StockTime = 20.0f;
    public int ResourceAmount = 16;
    public ItemType ResourceType = ItemType.Wood; 

    private Inventory _inventory;

	private float _stockTimer = 20.0f;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
        _inventory.Add(ResourceType, ResourceAmount);
    }

    private void Update()
    {
        if (_stockTimer <= 0)
            Restock();

        if (_inventory.IsEmpty)
            IsUsed = true;

        if (IsUsed)
            _stockTimer -= Time.deltaTime;
    }

    void Restock()
    {
        _stockTimer = StockTime;
        _inventory.Add(ResourceType, ResourceAmount);
        IsUsed = false;
    }
}
