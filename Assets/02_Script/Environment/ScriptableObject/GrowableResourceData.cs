using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Environment/Object/GrowableResourceData")]
public class GrowableResourceData : NatureResourceData
{
    [SerializeField] int timeToGrow;
    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<int> growthStageTime;
    // 취득 가능한 아이템도 추가

    public int TimeToGrow { get { return timeToGrow; } }
    public List<Sprite> Sprites { get {  return sprites; } }
    public List<int> GrowthStageTime { get { return growthStageTime; } }
}
