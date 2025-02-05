using System;
using System.Collections;
using UnityEngine;

public class DarkSprit : MonoBehaviour, IDamageable
{
    Animator monsterAnimator;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float extinguishAmount = 3f; // 불을 끌 때 줄어드는 내구도 양

    public Campfire targetCampfire;
    private Coroutine moveCoroutine; // 현재 이동 코루틴 저장

    void Start()
    {
        monsterAnimator = GetComponent<Animator>();
        if (targetCampfire != null) moveCoroutine = StartCoroutine(MoveToPosition(targetCampfire.transform.position));

    }

    private IEnumerator MoveToPosition(Vector3 targetPos)
    {
        SetIsMovingAnimation(true); // 애니메이션 설정 
        SetDirnimaiton(targetPos - transform.position);

        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        targetCampfire.AddDurability(-extinguishAmount);
        TakeDamage(0);
    }

    #region 피격당했을 때 

    // TODO : 에.. 뭐 아이템 드롭 할거 있으면 추가 
    public void TakeDamage(int damage)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // 이동 코루틴 중지
        }
        else
        {
            //Debug.Log("moveCoroutine 이 없습니다");
        }

        SetIsMovingAnimation(false); // 이동 중지
        SetDieAnimation();
        Destroy(gameObject,1f); // TODO : 풀매니저로 나중에 변경.. 
    }

    public bool IsDead()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region 애니메이션

    void SetIsMovingAnimation(bool value)
    {
        monsterAnimator.SetBool("isMoving", value);
    }

    void SetDirnimaiton(Vector3 vector)
    {
        monsterAnimator.SetFloat("dirX", vector.x);
        monsterAnimator.SetFloat("dirY", vector.y);
    }

    void SetDieAnimation()
    {
        monsterAnimator.SetTrigger("Die");
    }
    #endregion


    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"{other.name}");
        if(other.transform.CompareTag("Player")){
            TakeDamage(0);
        }
    }
}
