using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike effect", menuName = "Data/Item effect/Thunder strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thurderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThurderStrike = Instantiate(thurderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThurderStrike, 1);
    }


}
