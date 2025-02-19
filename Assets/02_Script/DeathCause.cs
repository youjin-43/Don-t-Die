

public enum DeathCause
{
    None, 
    BearAttack,   // 곰에게 공격받음
    BatAttack,    // 박쥐에게 공격받음
    SkelAttack,   // 스켈레톤에게 공격받음
    Starvation,   // 배고픔 게이지 0
    Dehydration,  // 목마름(탈수)으로 사망
    ParasiticInfection, // 기생충 감염
    Plague, // 쥐 흑사병
    PoisonousMushroom,  // 독버섯
    DarkDamage          // 밤에 불 안 킴
    //Freezing,     // 너무 추워서 동사
    //Poison,       // 독 중독으로 사망
    //Drowning      // 물에 빠져서 사망
}