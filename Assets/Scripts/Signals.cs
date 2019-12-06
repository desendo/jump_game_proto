public struct SignalLoadScene { public string name; }
public struct SignalLoaderProgress { public float progress; public bool isComplete; }
public struct SignalLevelComplete { }
public struct SignalControlEnabled { public bool value; }
public struct SignalHitPad { public CharController charController; public Pad pad; public float dist;}
public struct SignalPlayerSpawned { public CharController player; }
public struct SignalGameover {  }
