using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ESources
{
    PLAYER,
    ENEMY1,
    ENEMY2,
    ENVIRONMENT,
    OTHER,
    MENU,
}
public enum ESoundTypes
{
    WALK,
    JUMP,
    ATTACK1,
    ATTACK2,
    CLICK,
}

public enum EAnimalStates
{
    None,
    Move,
    Eat,
    Drink,
    ReproduceReady,
    ReproRequest,
    Engaged
}
public enum ETargetTypes
{
    None,
    Grass,
    Animal,
    Water
}
