using UnityEngine;

public class Skel : MonsterBase
{
    protected override void HandleMonsterHit(Transform attacker)
    {
        if (monsterStateMachine.CurrentState != monsterStateMachine.dieMonsterState) OnFlee();
    }

    private void OnEnable()
    {
        DamageableResourceNode.OnResourceAttacked += HandleResourceAttacked;
    }

    private void OnDisable()
    {
        DamageableResourceNode.OnResourceAttacked -= HandleResourceAttacked;
    }

    [SerializeField] private float detectionRadius = 5f; // 반응 거리
    // 플레이어가 주변 광석을 캐면 플레이어를 쫒아가서 공격 
    private void HandleResourceAttacked(Transform resourceTransform)
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.GetPlayerPos());

        if (distance <= detectionRadius)
        {
            Target = GameManager.Instance.PlayerTransform;  // 플레이어를 타겟으로 설정
            monsterStateMachine.TransitionTo(monsterStateMachine.chaseMonsterState);
        }
    }

    public override void AfterFleeState()
    {
        OnChase();
    }
}
