using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    public int Life;
    public List<Action> ActionList;
    public int Shield;
    public int attackMod;

    public AudioClip AttackAudio;
    public AudioClip DamagedSound;
    public AudioClip DieSound;
    public AudioClip DefenseSound;
    public AudioClip BuffSound;
}
