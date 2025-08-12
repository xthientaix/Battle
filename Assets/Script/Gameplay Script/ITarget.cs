using UnityEngine;

public interface ITarget
{
    public Transform Target { get; set; }
    public void HitTarget(Transform target);
}
