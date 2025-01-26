using UnityEngine;

enum PlayerState
{
    Idle,
    Walk,
    Attack
}

enum PlayerObjChilds
{
    MainCamera,
    AttackCollider,
    FishingTip
}

public class PlayerMoveManager : MonoBehaviour
{
    [SerializeField] PlayerState playerState;

    PlayerAnimator playerAnimator;

    // 플레이어가 할 수 있는 행동들 
    PlayerMove playerMove;
    PlayerAutoInteract playerAutoInteract;
    PlayerAttack playerAttack;
    //TODO : fishing 추가

    void Start()
    {
        playerState = PlayerState.Idle;

        playerAnimator = GetComponent<PlayerAnimator>();
        playerMove = GetComponent<PlayerMove>();
        playerAutoInteract = GetComponent<PlayerAutoInteract>();
        playerAttack = transform.GetChild((int)PlayerObjChilds.AttackCollider).GetComponent<PlayerAttack>();
        
        SetAnimatorAtEachMoves(); // 각 행동에 애니메이터 설정 
    }

    void SetAnimatorAtEachMoves()
    {
        playerMove.playerAnimator = playerAnimator;
        playerAutoInteract.playerAnimator = playerAnimator;
        playerAttack.playerAnimator = playerAnimator;
    }

    void Update()
    {
        if(playerState != PlayerState.Attack) //공격중에는 이동 및 공격 못하도록 
        {
            playerMove.HandleMovement(); // 상하좌우 입력 관리 
            playerAutoInteract.AutoInteract(); //자동 상호작용
            if (Input.GetKeyDown(KeyCode.F)) playerAttack.Attack(); //공격 //TODO : 현재 장착중인 장비 확인 하고 상호작용 하도록
            //TODO : F공격이 좀 불편한것 같아서 현재 가지고 있는 도구에 따라 클릭하면 자동으로 맞는거 사용하도록 하면 좋을것 같음 
        }

        // 조합창 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CraftManager.Instance.ToggleCraftingUI();
            InventoryManager.Instance.DisableScrollToggle();
        }

        // 인벤토리에서 선택된 아이템 사용
        if(Input.GetKeyDown(KeyCode.U))
        {
            InventoryManager.Instance.UseSelectedItem();
        }
    }

    #region ChangeStateFunc
    public void ChangeToIdleState(){ playerState = PlayerState.Idle; } //공격 애니메이션 끝날 때 호출 
    public void ChangeToWalkState(){ playerState = PlayerState.Walk; }
    public void ChangeToAttackState(){ playerState = PlayerState.Attack; } //공격 애니메이션 시작할 떄 호출 
    #endregion
}
