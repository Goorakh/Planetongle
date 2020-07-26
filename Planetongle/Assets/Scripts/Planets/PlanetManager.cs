using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlanetManager : Singleton<PlanetManager>
{
    public static readonly float G = 50f;

    List<Planet> _planets = new List<Planet>();

    public void OnPlanetCreated(Planet planet)
    {
        if (planet.IsInitialized)
            return;

        _planets.Add(planet);
        planet.Initialize();
    }

    public void OnPlanetDestroyed(Planet planet)
    {
        _planets.Remove(planet);
    }

    public List<ForceData> GetForcesFromAllPlanets(GravityRigidbody rigidbody)
    {
        List<ForceData> forces = new List<ForceData>();

        foreach (Planet planet in _planets)
        {
            forces.Add(new ForceData(rigidbody, planet));
        }

        return forces;
    }
}

public class ForceData : IComparable<ForceData>
{
    public ForceData(GravityRigidbody origin, Planet planet)
    {
        float distance = Vector2.Distance(origin.transform.position, planet.transform.position);
        float forceToPlanet = PlanetManager.G * (planet.Mass * origin.Mass / Mathf.Pow(distance, 2f));

        Vector2 direction = Utils.Get2DDirection(origin.transform.position, planet.transform.position);

        Force = direction * forceToPlanet;
        Origin = planet;
    }

    public Vector2 Force;
    public Planet Origin;

    public int CompareTo(ForceData other)
    {
        return Force.magnitude.CompareTo(other.Force.magnitude);
    }
}
