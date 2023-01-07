using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    [Header("Player Reference")]
    public Transform player;

    [Header("Preferences")] 
    public float offsetX = 0f;
    public float offsetY = 0f;
    public float offsetZ = 0f;
    public float positionSmoothing = 4f;
    public float rotationSmoothing = 1f;
    

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, player.position + new Vector3(offsetX, offsetY, offsetZ), positionSmoothing * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, rotationSmoothing * Time.deltaTime);
    }
}
