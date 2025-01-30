using System;
using UnityEngine;

enum PlayerState
{
    Idle,
    Walk,
    Attack,
    Die
}

enum PlayerObjChilds
{
    MainCamera,
    AttackCollider,
    FishingTip
}

public class PlayerMoveManager : MonoBehaviour,IDamageable
{
    // 플레이어의 현재 상태 관련 
    [SerializeField] PlayerState playerState;
    [SerializeField] bool isDead;

    // 플레이어 관련 수치들 
    PlayerStatus playerStatus;

    // 플레이어 애니메이터 관리 
    PlayerAnimator playerAnimator;

    // 플레이어가 할 수 있는 행동들 
    PlayerMove playerMove;
    PlayerAutoInteract playerAutoInteract;
    PlayerAttack playerAttack;
    //TODO : fishing 추가

    void Start()
    {
        // 플레이어의 현재 상태 
        playerState = PlayerState.Idle;
        isDead = false;

        // 플레이어 관련 수치들 
        playerStatus = GetComponent<PlayerStatus>();

        // 플레이어가 할 수 있는 행동들 
        playerMove = GetComponent<PlayerMove>();
        playerAutoInteract = GetComponent<PlayerAutoInteract>();
        playerAttack = transform.GetChild((int)PlayerObjChilds.AttackCollider).GetComponent<PlayerAttack>();

        // 각 행동에 애니메이터 설정
        playerAnimator = GetComponent<PlayerAnimator>();
        SetAnimatorAtEachMoves(); 
    }

    void SetAnimatorAtEachMoves()
    {
        playerMove.playerAnimator = playerAnimator;
        playerAutoInteract.playerAnimator = playerAnimator;
        playerAttack.playerAnimator = playerAnimator;
    }

    void Update()
    {
        // 살아있을때만 조작이 가능하도록 
        if (!isDead)
        {
            if (playerState != PlayerState.Attack) //공격중에는 이동 및 공격 못하도록 
            {
                playerMove.HandleMovement(); // 상하좌우 입력 관리 
                playerAutoInteract.AutoInteract(); // 자동 상호작용(스페이스바)
                if (Input.GetKeyDown(KeyCode.F)) playerAttack.Attack(); //공격 //TODO : 현재 장착중인 장비 확인 하고 상호작용 하도록
                                                                        //TODO : F공격이 좀 불편한것 같아서 현재 가지고 있는 도구에 따라 클릭하면 자동으로 맞는거 사용하도록 하면 좋을것 같음 
            }

            // 인벤토리에서 선택된 아이템 사용
            if (Input.GetKeyDown(KeyCode.U))
            {
                InventoryManager.Instance.UseSelectedItem();
            }
        }

        // 조합창 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CraftManager.Instance.ToggleCraftingUI();
            InventoryManager.Instance.DisableScrollToggle();
        }

        
    }

    #region ChangeStateFunc
    public void ChangeToIdleState(){ playerState = PlayerState.Idle; } //공격 애니메이션 끝날 때 호출 
    public void ChangeToWalkState(){ playerState = PlayerState.Walk; }
    public void ChangeToAttackState(){ playerState = PlayerState.Attack; } //공격 애니메이션 시작할 떄 호출
    public void ChangeToDieState() { playerState = PlayerState.Die; }
    #endregion

    #region    IDamageable
    public void TakeDamage(int damage)
    {
        // TODO : 현재 방어구 기반으로 수치 조정 후 status 로 전달 
        playerStatus.LoseHP(damage);
    }

    public bool IsDead()
    {
        return isDead;
    }

    #endregion
    public void Die()
    {
        isDead = true;
        playerAnimator.TriggerDieAnimation();
        ChangeToDieState();
    }
}
