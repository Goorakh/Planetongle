using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityRigidbody : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    public float Mass => _rigidbody.mass;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
    }

    void FixedUpdate()
    {
        List<ForceData> forces = PlanetManager.Instance.GetForcesFromAllPlanets(this);
        
        Vector2 totalForce = forces.Total();

        if (totalForce.magnitude <= 0.1f)
            return;

        _rigidbody.AddForceAtPosition(totalForce, _rigidbody.worldCenterOfMass, ForceMode2D.Force);
    }
}
