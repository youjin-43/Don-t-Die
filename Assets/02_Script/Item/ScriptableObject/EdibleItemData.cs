using UnityEngine;

[CreateAssetMenu(menuName = "Item/EdibleItemData")]
public class EdibleItemData : ItemData
{
    [Header("EdibleItemData")]
    public float healthValue;     // 체력
    public float hungerValue;     // 먹었을 때 회복되는 허기 양
    public float thirstValue;     // 갈증

    //public void Execute()
    //{
    //    StatusManager.Instance.AddHealth(healthValue);
    //    StatusManager.Instance.AddHungry(hungerValue);
    //    StatusManager.Instance.AddThirsty(thirstValue);
    //}

    public void ExecuteRandom()
    {

    }
}
