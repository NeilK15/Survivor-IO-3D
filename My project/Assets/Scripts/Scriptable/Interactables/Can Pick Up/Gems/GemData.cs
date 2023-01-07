using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Gem", menuName = "Gems/Gem", order = 0)]
public class GemData : ScriptableObject
{

    [Header("General Gem Info")]
    public new string name;

    public float value = 5f;

    public GameObject prefab;

}
