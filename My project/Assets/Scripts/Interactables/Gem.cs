using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gem : MonoBehaviour, ICanPickUp
{

    [Header("Gem Data")]
    public GemData gemData;

    private Rigidbody _rb;
    
    private void Start()
    {
        int x = Random.Range(0, 360);
        int y = Random.Range(0, 360);
        int z = Random.Range(0, 360);

        transform.rotation = Quaternion.Euler(x, y, z);

        _rb = GetComponent<Rigidbody>();
    }

    public float PickUp()
    {
        Destroy(gameObject);
        return gemData.value;
    }

    public void Attract(Vector3 target, float force)
    {
        Vector3 direction = target - transform.position;

        transform.position = Vector3.Lerp(transform.position, target, force);
    }
    
}
