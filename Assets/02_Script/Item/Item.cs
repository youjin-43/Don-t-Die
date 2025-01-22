using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData ItemData;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetItemData(ItemData itemData)
    {
        ItemData = itemData;
    }
}