using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatsHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float _value)
    {
        Health health = character.GetComponent<Health>();
        if (health != null)
            health.AddHealth((int)_value);
    }
}
