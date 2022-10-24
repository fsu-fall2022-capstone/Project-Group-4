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
    Sprinting
}