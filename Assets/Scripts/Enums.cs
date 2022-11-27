[System.Flags]
public enum ElementType
{
    None,
    Ice, 
    Fire, 
    Water, 
    Lightning
}

[System.Flags]
public enum AbilityType {
    None,
    Spawn, 
    DeadSpawn,
    Shield, 
    Heal, 
    Sprint, 
    Overcharge
}

[System.Flags]
public enum StatusType {
    None,
    Frozen,
    Electrocuted,
    Burning,
    Stunned,
    Overcharged,
    Sprinting,
    Shielded
}

[System.Flags]
public enum BoonType
{
    Power,
    Swiftness,
    Farsight,
    Fortune
}
