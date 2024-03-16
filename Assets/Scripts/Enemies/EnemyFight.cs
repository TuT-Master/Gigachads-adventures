using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFight : MonoBehaviour
{
    public enum Attitude
    {
        MeleeAgressive,
        MeleeEvasive,
        MeleeWandering,
        MeleeStealth,
        RangedStatic,
        RangedWandering,
        Placeable,
    }
    public Attitude attitude;
}