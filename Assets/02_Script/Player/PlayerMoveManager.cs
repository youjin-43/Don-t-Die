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
    ToolCollider,
    FishingTip
}

public class PlayerMoveManager : MonoBehaviour
{
    // 플레이어의 현재 상태 관련 
    [SerializeField] PlayerState playerState;

    // 플레이어 관련 수치들 
    PlayerStatus playerStatus;

    // 플레이어 애니메이터 관리 
    PlayerAnimator playerAnimator;

    // 플레이어가 할 수 있는 행동들 
    PlayerMove playerMove;
    PlayerAutoInteract playerAutoInteract;
    PlayerUseTool playerUseTool;
    //TODO : fishing 추가

    void Start()
    {
        // 플레이어의 현재 상태 
        playerState = PlayerState.Idle;

        // 플레이어 관련 수치들 
        playerStatus = GetComponent<PlayerStatus>();

        // 플레이어가 할 수 있는 행동들 
        playerMove = GetComponent<PlayerMove>();
        playerAutoInteract = GetComponent<PlayerAutoInteract>();
        playerUseTool = transform.GetChild((int)PlayerObjChilds.ToolCollider).GetComponent<PlayerUseTool>();

        // 각 행동에 애니메이터 설정 -> awake start 순서 꼬일까봐 매니저에서 한번에 셋팅 
        playerAnimator = GetComponent<PlayerAnimator>();
        SetAnimatorAtEachMoves(); 
    }

    void SetAnimatorAtEachMoves()
    {
        playerMove.playerAnimator = playerAnimator;
        playerAutoInteract.playerAnimator = playerAnimator;
        playerUseTool.playerAnimator = playerAnimator;
    }

    void Update()
    {
        // 플레이어가 죽었으면 조작 금지
        if (playerStatus.IsDead()) return;

        //if (playerState != PlayerState.Attack) //공격중에는 이동 및 공격 못하도록 
        //{
            playerMove.HandleMovement(); // 상하좌우 입력 관리 
            playerAutoInteract.AutoInteract(); // 자동 상호작용 (스페이스바)
        //}

        
        playerUseTool.UseTool(); // 현재 장착중인 도구 사용 (마우스 왼쪽 클릭)

        // 인벤토리에서 선택된 아이템 사용
        if (Input.GetKeyDown(KeyCode.U))
        {
            InventoryManager.Instance.UseSelectedItem();
        }

        // 조합창 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CraftManager.Instance.ToggleCraftingUI();
            InventoryManager.Instance.DisableScrollToggle();
        }
    }

    public void Die()
    {
        playerAnimator.TriggerDieAnimation();
        ChangeToDieState();
    }

    #region ChangeStateFunc

    public void ChangeToIdleState() { playerState = PlayerState.Idle; } //공격 애니메이션 끝날 때 호출 
    public void ChangeToWalkState() { playerState = PlayerState.Walk; }
    public void ChangeToAttackState() { playerState = PlayerState.Attack; } //공격 애니메이션 시작할 떄 호출
    public void ChangeToDieState() { playerState = PlayerState.Die; }

    #endregion
}
