using System;
using System.Collections;
using UnityEngine;

public class Dropper : MonoBehaviour
{

    private GemData[] _gems;
    private float[] _dropProbability;
    private int _drops;
    
    public void DropperConstructor(GemData[] gems, float[] probability)
    {
        if (gems.Length != probability.Length || gems.Length <= 0)
        {
            throw new DataMisalignedException("List of Gems and Probability are not equal or not greater than 0");
        }
        _gems = gems;
        _dropProbability = probability;
        _drops = _gems.Length;
    }

    public void Drop(int index, Vector3 position, Quaternion rotation)
    {
        Instantiate(_gems[index].prefab, position, rotation);
    }
    
    public void DropAll(Vector3 position, Quaternion rotation, float spread)
    {
        for (int i = 0; i < _drops; i++)
        {
            if (ShouldDrop(i))
            {
                float randomLength = UnityEngine.Random.Range(0f, spread);
                float randomAngle = UnityEngine.Random.Range(0f, 360f);

                Vector3 spot = Vector3.Scale(position,
                    new Vector3(randomLength * Mathf.Cos(randomAngle), 0, randomLength * Mathf.Sin(randomAngle)));

                Drop(i, position, rotation);
            }
        }
    }

    private bool ShouldDrop(int index)
    {
        float random = UnityEngine.Random.Range(0f, 1f);

        return (random <= _dropProbability[index]);
    }

}
