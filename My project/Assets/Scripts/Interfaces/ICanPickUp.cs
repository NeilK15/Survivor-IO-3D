using UnityEngine;

public interface ICanPickUp
{
    public float PickUp();

    public void Attract(Vector3 target, float force);
}
