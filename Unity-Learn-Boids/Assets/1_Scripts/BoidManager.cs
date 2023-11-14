using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public Vector2 GridSize;
    public int BoidsAmount;
    public GameObject Boid;
    public float BoidDetectRadius;
    public GameObject BoidsParent;

    public static BoidManager Instance {  get { return instance; } }
    private static BoidManager instance;

    private List<Boid> boids = new List<Boid>();

    public void Start()
    {
        instance = this;
        InitializeBoids();
    }

    public List<Boid> GetNeighbours(Boid mainBoid)
    {
        List<Boid> neighbours = new List<Boid>();

        foreach (Boid currentBoid in boids)
        {
            if (Vector2.Distance(mainBoid.DetectPoint.position, currentBoid.DetectPoint.position) < BoidDetectRadius)
            {
                neighbours.Add(currentBoid);
            }
        }

        return neighbours;
    }

    private void InitializeBoids()
    {
        for (int i = 0; i < BoidsAmount; i++)
        {
            GameObject newBoidGameObject = Instantiate(Boid);
            newBoidGameObject.transform.SetParent(BoidsParent.transform);
            newBoidGameObject.transform.position = new Vector2(Random.Range(0, GridSize.x), Random.Range(0, GridSize.y));
            newBoidGameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180, 180));
            Boid newBoid = newBoidGameObject.GetComponent<Boid>();
            boids.Add(newBoid);
        }
    }
}
