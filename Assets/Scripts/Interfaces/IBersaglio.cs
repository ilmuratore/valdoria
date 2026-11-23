using UnityEngine;

public interface IBersaglio
{
    Vector2 Posizione { get; }
    
    bool EAttivo { get; }
}
