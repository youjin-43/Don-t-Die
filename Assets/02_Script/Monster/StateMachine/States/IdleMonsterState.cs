using UnityEngine;

/// <summary>
/// 몬스터가 랜덤하게 움직이거나 대기하는 상태
/// </summary>
public class IdleMonsterState : IMonsterState
{
    MonsterBase monster;
    Animator monsterAnimator;
    public BiomeType biomeType;
    float moveSpeed;
    float moveInterval;

    // 랜덤으로 움직이는데 필요한 변수들 
    bool isMoving; // 현재 이동중인가? 
    float moveTimer;  // 대기 시간 타이머
    Vector3 targetPosition;

    public IdleMonsterState(MonsterBase monster)
    {
        this.monster = monster;
        monsterAnimator = monster.MonsterAnimator;
        biomeType = monster.BiomeType;
        moveSpeed = monster.MoveSpeed;
        moveInterval = monster.MoveInterval;
    }

    public void EnterState()
    {
        Debug.Log($"{monster.gameObject.name} 이 Idle 상태로 진입!");
        isMoving = false;
        moveTimer = Random.Range(5, moveInterval);
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
        //움직이고 있지 않는동안 타이머 작동 
        if (!isMoving && moveTimer>0)
        {
            moveTimer -= Time.deltaTime;
            //Debug.Log($"대기중. 남은시간 {moveTimer}");

            // 대기 시간 타이머가 0 이하가 되면 
            if (moveTimer <= 0)
            {
                //Debug.Log("moveTimer가 0이하가 됐습니다 ");

                //이동할 타일 탐색 
                targetPosition = monster.GetRandomPosition();
                //Debug.Log($"{targetPosition} 로 이동할거임!");

                BiomeType nextPosBiome = monster.GetBiomeInfo(targetPosition);
                if (nextPosBiome == biomeType) // 그 타일이 내 바이옴과 일치하면 
                {
                    isMoving = true;

                    // 애니메이션 설정
                    Vector3 dir = targetPosition - monster.transform.position;
                    monsterAnimator.SetBool("IsMoving", true);
                    monsterAnimator.SetFloat("dirX", dir.x);
                    monsterAnimator.SetFloat("dirY", dir.y);
                }
                else
                {
                    //Debug.Log($"타일 바이옴 {nextPosBiome}가 몬스터 바이옴 {biomeType}과 다릅니다.");
                    isMoving = false;
                    moveTimer = Random.Range(0, moveInterval); // 타이머 초기화 
                }
            }
        }
        else
        {
            // 타겟 위치와 가까워질 떄 까지 이동 
            if(Vector3.Distance(monster.transform.position, targetPosition) > 0.1f)
            {
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                //Debug.Log($"이동중  {monster.transform.position}");
            }
            else
            {
                //도착하면 움직임 멈추기
                //Debug.Log("목표 위치에 도착!");
                isMoving = false;
                monsterAnimator.SetBool("IsMoving", false);
                moveTimer = Random.Range(0,moveInterval); // 타이머 초기화 
            }
        }
    }
}
