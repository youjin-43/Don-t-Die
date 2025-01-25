using UnityEngine;

enum PlayerState
{
    Idle,
    Walk,
    Attack
}

public class PlayerMoveManager : MonoBehaviour
{
    [SerializeField] PlayerState playerState;
    PlayerMove playerMove;
    PlayerAutoInteract playerAutoInteract;
    PlayerAttack playerAttack;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerState = PlayerState.Idle;
        playerMove = GetComponent<PlayerMove>();
        playerAutoInteract = GetComponent<PlayerAutoInteract>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState != PlayerState.Attack) //공격중에는 이동 및 공격 못하도록 
        {
            playerMove.HandleMovement(); // 상하좌우 입력 관리 
            playerAutoInteract.AutoInteract(); //자동 상호작용
            if (Input.GetKeyDown(KeyCode.F)) playerAttack.Attack(); //공격 //TODO : 현재 장착중인 장비 확인 하고 상호작용 하도록
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
    public void ChangeToIdleState(){ playerState = PlayerState.Idle; }
    public void ChangeToWalkState(){ playerState = PlayerState.Walk; }
    public void ChangeToAttackState(){ playerState = PlayerState.Attack; }
    #endregion
}
