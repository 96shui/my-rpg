using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{

    private PlayerStats stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private float buffDuration;
    [SerializeField] private int buffAmount;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        stats.IncreaseStatBy(buffAmount, buffDuration, stats.GetStat(buffType));
    }

    


}
