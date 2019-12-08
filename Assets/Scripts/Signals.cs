public struct SignalLoadScene { public string name; }
public struct SignalLoaderProgress { public float progress; public bool isComplete; }
public struct SignalLevelComplete { }
public struct SignalControlEnabled { public bool value; }
public struct SignalHitPad { public CharController charController; public Pad pad; public float dist;}
public struct SignalCharSpawned { public CharController charController; }
public struct SignalDie { public CharController charController; }
public struct SignalFirstRank { public CharController charController; }
public struct SignalMurder
{
    public CharController killer;
    public CharController victim;
}