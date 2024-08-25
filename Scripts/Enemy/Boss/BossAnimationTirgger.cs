using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationTirgger : Enemy_AnimationTriggers
{
    private Boss boss => GetComponentInParent<Boss>();

    private void Relocate() => boss.FindPosition();


}
