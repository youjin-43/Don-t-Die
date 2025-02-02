using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 플레이어 자식 오브젝트 중 ToolCollider에 붙어있음 
/// </summary>
public class PlayerUseTool : MonoBehaviour
{
    public ToolItemData currentTool;
    [SerializeField] Transform target;
    [SerializeField] Torch torchLight;

    [HideInInspector] public PlayerAnimator playerAnimator; //PlayerMoveManager 에서 할당 받도록 함 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(target == null)
        {
            target = collision.transform;

            // 몬스터를 때린 경우 
            MonsterBase monsterBase = collision.GetComponent<MonsterBase>();
            if (monsterBase != null)
            {
                Debug.Log($"{transform.parent}가 {collision.name}을 공격!");
                monsterBase.OnHit(transform.parent, (int)currentTool.Atk); // OnHit 이벤트 발생 -> attacker로 플레이어의 transfrom 전달
            }
            else
            {
                IDamageable damageable = collision.GetComponent<IDamageable>();
                if(damageable != null)
                {
                    damageable.TakeDamage((int)currentTool.Atk);
                }
            }

            //TODO : 광석이나 나무 추가


        }
    }

    public void clearTarget()
    {
        target = null;
    }

    #region 장비 변경 이벤트

    private void Start()
    {
        // 이벤트 구독
        EquipmentManager.Instance.OnEquipChanged += HandleEquipChanged;
    }

    private void OnDestroy()
    {
        // 이벤트 해제 (메모리 누수 방지)
        EquipmentManager.Instance.OnEquipChanged -= HandleEquipChanged;
    }

    /// <summary>
    /// 장비가 변경될 때 호출되는 함수
    /// </summary>
    private void HandleEquipChanged(ItemData itemData, EquipmentSlot slot)
    {
        if (slot == EquipmentSlot.Hand) currentTool = EquipmentManager.Instance.GetCurrentTool();
        if (currentTool != null && currentTool.Type == ToolType.Torch)
        {
            torchLight.TurnOn(); 
        }
        else
        {
            torchLight.TurnOff();
        }
    }

    #endregion

    public void UseTool()
    {
        if (Input.GetMouseButtonDown(0)) StartUsingEquippedTool();
        if (Input.GetMouseButtonUp(0)) StopUsingEquippedTool();
    }

    public void StartUsingEquippedTool()
    {
        if (currentTool == null)
        {
            Debug.Log("착용중인 Tool 이 없습니다");
            return;
        }
        else
        {
            Debug.Log($"{currentTool.Type.ToString()} 사용 중");

            playerAnimator.SetUseToolAnimation_True();

            switch (currentTool.Type)
            {
                case ToolType.Sword:
                    playerAnimator.SetSwordAnimation_True();
                    break;
                case ToolType.Axe:
                    playerAnimator.SetAxeAnimation_True();
                    break;
                case ToolType.Pickaxe:
                    playerAnimator.SetPickAxeAnimation_True();
                    break;
                default:
                    break;
            }
        }
    }

    public void StopUsingEquippedTool()
    {
        playerAnimator.SetUseToolAnimation_False();
        playerAnimator.SetSwordAnimation_False();
        playerAnimator.SetAxeAnimation_False();
        playerAnimator.SetPickAxeAnimation_False();
    }
}
