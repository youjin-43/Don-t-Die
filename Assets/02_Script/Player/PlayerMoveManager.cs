using UnityEngine;

enum PlayerObjChilds
{
    MainCamera,
    ToolCollider,
    FishingTip
}

public class PlayerMoveManager : MonoBehaviour
{
    // 이동 가능한 상태인가? 
    [SerializeField] bool canMove;
    [SerializeField] bool isAutoInteracting;

    // 플레이어 관련 수치들 
    PlayerStatus playerStatus;

    // 플레이어 애니메이터 관리 
    PlayerAnimator playerAnimator;

    // 플레이어가 할 수 있는 행동들 
    PlayerMove playerMove; // 이동 
    PlayerAutoInteract playerAutoInteract; // 자동 상호작용 
    PlayerUseTool playerUseTool;
    PlayerFishingAction playerFishingAction;
    PlayerGetWaterAction playerGetWaterAction;

    void Start()
    {
        canMove = true;

        // 플레이어 관련 수치들 
        playerStatus = GetComponent<PlayerStatus>();

        // 플레이어가 할 수 있는 행동들 
        playerMove = GetComponent<PlayerMove>();
        playerAutoInteract = GetComponent<PlayerAutoInteract>();
        playerUseTool = transform.GetChild((int)PlayerObjChilds.ToolCollider).GetComponent<PlayerUseTool>();
        playerFishingAction = GetComponent<PlayerFishingAction>();
        playerGetWaterAction = GetComponent<PlayerGetWaterAction>();
        // 각 행동에 애니메이터 설정 -> awake start 순서 꼬일까봐 매니저에서 한번에 셋팅 
        playerAnimator = GetComponent<PlayerAnimator>();
        SetAnimatorAtEachMoves(); 
    }

    void SetAnimatorAtEachMoves()
    {
        playerStatus.playerAnimator = playerAnimator;
        playerMove.playerAnimator = playerAnimator;
        playerAutoInteract.playerAnimator = playerAnimator;
        playerUseTool.playerAnimator = playerAnimator;
    }

    void Update()
    {
        // 플레이어가 죽었으면 조작 금지
        if (playerStatus.IsDead()) return;

        if (CraftManager.Instance.isCrafing) return;

        if (canMove && !playerFishingAction.isFishing)
        {
            playerAutoInteract.AutoInteract(); // 자동 상호작용 중이면 이동 X
            isAutoInteracting = playerAutoInteract.isAutoInteracting;

            if (!isAutoInteracting) playerMove.HandleMovement(); // 상하좌우 이동
            if (UIManager.Instance.IsUIClick() == false && Input.GetMouseButtonDown(0)) playerUseTool.StartUsingEquippedTool(); // 도구 사용 

        }
        if (Input.GetMouseButtonUp(0)) playerUseTool.StopUsingEquippedTool(); 

        playerFishingAction.Fishing(); // 낚시 
        playerGetWaterAction.GetWater(); // 물 퍼오기ㄷ

        // 인벤토리에서 선택된 아이템 사용
        if (Input.GetKeyDown(KeyCode.U))InventoryManager.Instance.UseSelectedItem();

        // 조합창 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, 1f, LayerMask.GetMask("Interactable"));

            if(collider != null)
            {
                string[] split = collider.name.Split('(');

                if(split[0] == "CraftingTable")
                {
                    CraftManager.Instance.InRange(true);
                }
                else
                {
                    CraftManager.Instance.InRange(false);
                }
            }
            else
            {
                CraftManager.Instance.InRange(false);
            }

            CraftManager.Instance.ToggleCraftingUI();
            InventoryManager.Instance.DisableScrollToggle();
        }

        // 인벤토리 단축키
        // Update안에서 for문 돌린 이유
        // 안 그러면 if(1) else if(2) else if(3) .... 늘어저서 그랬어요...
        for(int i = 0; i < 9; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                InventoryManager.Instance.UseItem((int)(KeyCode.Alpha1 + i));
            }
        }

        // DEBUG : 상자 토글
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(GameManager.Instance.IsBagCreated == true)
            {
                BoxManager.Instance.ToggleBoxUI();
            }
        }
    }

    #region Set canMove - Tool 애니메이션에서 호출됨 

    public void SetcanMove_True()
    {
        canMove = true;
    }

    public void SetcanMove_False()
    {
        canMove = false;
    }

    #endregion

    // tool 애니메이션 끝날때 호출 
    public void ClearTarget()
    {
        playerUseTool.clearTarget();
    }
}
