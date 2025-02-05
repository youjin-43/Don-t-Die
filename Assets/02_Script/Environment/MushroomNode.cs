using UnityEngine;

public class MushroomNode : ResourceNode
{
    [SerializeField] private GameObject mushroomMonsterPrefab; // 변신할 버섯 몬스터 프리팹
    [SerializeField] private float transformChance = 0.8f; // 변신 확률 

    public override void Harvest()
    {
        if (Random.value < transformChance) // 확률 체크
        {
            TransformIntoMonster();
        }
        else
        {
            base.Harvest(); // 기본 아이템 드랍 로직 실행
            SoundManager.Instance.Play(AudioType.Effect, AudioClipName.Mushroom_Die);
        }
    }

    private void TransformIntoMonster()
    {
        // 기존 버섯 제거
        PoolManager.Instance.Push(gameObject);

        // 같은 위치에 버섯 몬스터 생성
        GameObject go = Instantiate(mushroomMonsterPrefab, transform.position, Quaternion.identity); // TODO : 풀 매니저를 이용하도록 수정
        MushroomMonster mushroomMonster = go.GetComponent<MushroomMonster>();

        if (mushroomMonster != null)
        {
            mushroomMonster.OnMonsterStateEnd += TurnBackToMushroom; // 몬스터가 도망 후 다시 버섯으로 돌아가도록 이벤트 등록
        }
    }

    private void TurnBackToMushroom(Vector3 position_AfterFleeing)
    {
        GameObject newMushroom = PoolManager.Instance.Pop(gameObject); // 기존 버섯 리스폰 // TODO : 이렇게 하는거 맞는지 희원님꼐 확인 
        newMushroom.transform.position = position_AfterFleeing;
    }
}
