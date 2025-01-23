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
        if (ItemData.Image == null)
        {
            DebugController.Log($"{itemData.name}의 이미지가 설정되지 않았습니다.");
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemData.Image;
    }
}