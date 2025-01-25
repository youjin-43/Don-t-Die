using System.Collections;
using UnityEngine;

public class Bear : MonsterBase
{
    void Start()
    {
        SetData(); // 몬스터 기본 데이터 셋팅
        SetCompoenets(); // 기본 컴포넌트들 셋팅
        StartCoroutine(IdleMoveRoutine()); // 움직이기 시작! 
    }
    

    public override void Move()
    {
        //주변으로 이동하는데 이동할 타일이 grass라면 이동     
    }

    #region IdleAction
    private IEnumerator IdleMoveRoutine()
    {
        while (currnetState == MonsterState.Idle) 
        {
            //Debug.Log($"{monsterData.name} 에서 코루틴 실행 중 ");

            Vector3 targetPosition = GetRandomPosition();  
            BiomeType nextPosbiome = GetBiomeInfo(targetPosition); 
            Vector3 dir = targetPosition - transform.position; // for 애니메이션

            if (nextPosbiome == monsterData.biomeType) // 이동할 타일 위치가 내 바이옴과 알치한다면 그쪽으로 이동 
            {
                // 애니메이션 셋팅 
                monsterAnimator.SetBool("IsMoving", true);
                monsterAnimator.SetFloat("dirX", dir.x);
                monsterAnimator.SetFloat("dirY", dir.y);

                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, monsterData.MoveSpeed * Time.deltaTime);
                    yield return null;
                }

                monsterAnimator.SetBool("IsMoving", false);
            }
            yield return new WaitForSeconds(Random.Range(0, monsterData.moveInterval)); // 타겟 위치로 이동 후 랜덤 시간만큼 대기 
        }

    }

    /// <summary>
    /// 현재 위치 기준으로 무작위 위치 계산
    /// </summary>
    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);

        Vector3 currentPos = transform.position;
        return new Vector3(currentPos.x + randomX, currentPos.y + randomY, currentPos.z);
    }

    BiomeType GetBiomeInfo(Vector3 pos)
    {
        return EnvironmentManager.Instance.biomeMap.GetTileBiome(new Vector2Int((int)pos.x, (int)pos.y));
    }
    #endregion
}
