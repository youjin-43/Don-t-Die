using UnityEngine;

/// <summary>
/// 플레이어 자식 오브젝트 중 ToolCollider에 붙어있음 
/// </summary>
public class PlayerUseTool : MonoBehaviour
{
    public PlayerAnimator playerAnimator; //PlayerMoveManager 에서 할당 받도록 함 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterBase monsterBase = collision.GetComponent<MonsterBase>();
        if (monsterBase != null)
        {
            Debug.Log($"{collision.name}을 공격!");
            //TODO : 데미지는 현재 착용중인 도구의 데이터를 가져오도록
            monsterBase.OnHit(transform.parent,10); // OnHit 이벤트 발생 -> attacker로 플레이어의 transfrom 전달
        }
    }

    public void UseTool()
    {
        if (Input.GetMouseButtonDown(0)) StartUsingEquippedTool();
        if (Input.GetMouseButtonUp(0)) StopUsingEquippedTool();
    }

    void StartUsingEquippedTool()
    {
        ToolItemData currentTool = EquipmentManager.Instance.GetCurrentTool();

        if(currentTool == null)
        {
            Debug.Log("착용중인 Tool 이 없습니다");
        }
        else
        {
            playerAnimator.SetUseToolAnimation_True();
            Debug.Log($"{currentTool.Type.ToString()} 사용 중");

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
                case ToolType.Rod:
                    break;
                default:
                    break;
            }
        }
    }

    void StopUsingEquippedTool()
    {
        //Debug.Log("StopUsingEquippedTool 실행됨");

        playerAnimator.SetUseToolAnimation_False();
        playerAnimator.SetSwordAnimation_False();
        playerAnimator.SetAxeAnimation_False();
        playerAnimator.SetPickAxeAnimation_False();
    }
}
