using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blades : AreaOfAttackWeapon
{

    [Header("Blades Settings")] 
    public GameObject bladePrefab;

    public float bladesDistance;

    [Range(2,6)]
    public int numBlades = 2;
    private int _numBlades = 2;

    private GameObject[] _blades;
    
    [Tooltip("Degrees Per Second")]
    public float rotationSpeed;

    public float rotationSmoothing;

    private void Start()
    {
        _blades = new GameObject[6];
        SetBlades();
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        if (SetBladesOnChange())
        {
            SetBlades();
        }
        RotateBlades();
    }

    private bool SetBladesOnChange()
    {
        numBlades = Mathf.Clamp(numBlades, 2, 6);
        if (numBlades != _numBlades)
        {
            _numBlades = numBlades;
            return true;
        }

        return false;
    }
    
    private void SetBlades()
    {
        // Destroying all the blades
        for (int i = 0; i < _blades.Length; i++)
        {
            if (_blades[i])
            {
                Destroy(_blades[i]);
                _blades[i] = null;
            }
        }
        
        // Setting the new blades

        float angleStep = Mathf.Deg2Rad * (360 / numBlades);
        
        for (int i = 0; i < numBlades; i++)
        {
            float angle = angleStep * i;
            Vector3 bladeOffset = new Vector3(Mathf.Cos(angle) * bladesDistance, 0, Mathf.Sin(angle) * bladesDistance);
            Vector3 bladePos = transform.position + bladeOffset;
            _blades[i] = Instantiate(bladePrefab, bladePos, Quaternion.identity, transform);
        }
    }

    private void RotateBlades()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * Quaternion.Euler(0,rotationSpeed,0), rotationSmoothing * Time.deltaTime);
    }


    protected override void AttackOn()
    {
        base.AttackOn();
    }

    protected override void AttackOff()
    {
        base.AttackOff();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, bladesDistance);
    }
}
