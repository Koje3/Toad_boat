using UnityEngine;
using UnityEngine.Events;

public interface IShockable
{
    public void OnShocked(float shockDuration, float amplitude);
}
