using UnityEngine;

public class Bear : MonsterBase
{
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MonsterName = monsterData.MonsterName;
        CurrentHp = monsterData.MaxHP;
        MoveSpeed = monsterData.MoveSpeed;
        AttackDamage = monsterData.AttackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        //DebugController.Log(GetCurrnetPosTileInfo().ToString());
    }

    public override void Attack(GameObject target)
    {

    }

    public override void Move(Vector3 targetPosition)
    {

    }

    public BiomeType GetCurrnetPosTileInfo()
    {
        return EnvironmentManager.Instance.biomeMap.GetTileBiome(new Vector2Int((int)transform.position.y,(int)transform.position.x));
    }
}
