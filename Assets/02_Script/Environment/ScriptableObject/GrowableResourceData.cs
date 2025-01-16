using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Environment/Object/GrowableResourceData")]
public class GrowableResourceData : NatureResourceData
{
    [SerializeField] int timeToAllGrown;        // 다 자라는 데까지 걸리는 시간
    [SerializeField] List<Sprite> sprites;
    [SerializeField] List<int> growthStageTime; // 다음 스프라이트로 변경되는 시간
    [SerializeField] int minTimeToHarvest;      // 수확 가능한 자원이 되기까지 걸리는 시간
    // 취득 가능한 아이템도 추가

    public int TimeToAllGrown { get { return timeToAllGrown; } }
    public List<Sprite> Sprites { get {  return sprites; } }
    public List<int> GrowthStageTime { get { return growthStageTime; } }
    public int MinTimeToHarvest { get {  return minTimeToHarvest; } }
}
