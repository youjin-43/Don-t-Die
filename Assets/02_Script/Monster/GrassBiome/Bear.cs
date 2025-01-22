using System.Collections;
using UnityEngine;

enum BearState
{
    Idle,
    Chase
}

public class Bear : MonsterBase
{
    private void OnValidate()
    {
        DebugController.Log($"{moveInterval} is updated");
    }

    [Header("Bear Attributes")]
    [SerializeField] BearState bearState = BearState.Idle;
    [SerializeField] float moveInterval = 5f;
    [SerializeField] float moveRange = 3f; 

    void Start()
    {
        SetData(); //몬스터 기본 데이터 셋팅
        StartCoroutine(MoveRoutine());
    }
    

    public override void Move()
    {
        //주변으로 이동하는데 이동할 타일이 grass라면 이동     
    }


    private IEnumerator MoveRoutine()
    {
        while (bearState == BearState.Idle)
        {
            Vector3 targetPosition = GetRandomPosition();
            BiomeType nextPosbiome = GetBiomeInfo(targetPosition);

            if (nextPosbiome == MybiomeType) // 이동할 위치가 내 바이옴과 위치한다면 그쪽으로 이동 
            {
                while(Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);
                    yield return null;
                }
                Debug.Log($"Moved to {targetPosition}");
            }
            else
            {
                Debug.Log("Invalid tile. Skipping move.");
            }
            yield return new WaitForSeconds(Random.Range(0, moveInterval)); // 타겟 위치로 이동 후 랜덤 시간만큼 대기 
        }

    }



    private Vector3 GetRandomPosition()
    {
        // 현재 위치 기준으로 무작위 위치 계산
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);

        Vector3 currentPos = transform.position;
        return new Vector3(currentPos.x + randomX, currentPos.y + randomY, currentPos.z);
    }

    BiomeType GetBiomeInfo(Vector3 pos)
    {
        return EnvironmentManager.Instance.biomeMap.GetTileBiome(new Vector2Int((int)pos.x, (int)pos.y));
    }


}
