using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] public float G;
    [SerializeField] public GameObject particle;

    // Constants
    public static float HEIGHT = 8.88f;
    public static float WIDTH = 5.0f;

    [SerializeField] public int n; // num Particles
    public static int m = 8; // num Colors
    [SerializeField] public float forceFactor = 20;
    [SerializeField] public double rMax = 4;
    [SerializeField] public float beta = .3f;
    

    public static double frictionHalfLife = 0.040;    
    public static double frictionFactor;
    public static double[,] attractionMatrix;
    
    // Particle Arrays
    // color, posx, posy, vx, vy
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Screen.height);
        attractionMatrix = new double[m,m];
        Debug.Log(UnityEngine.Random.Range(-1.0f, 1.0f));
        for(int row = 0; row < m; ++row) {
            for(int col = 0; col < m; ++col) {
                attractionMatrix[row, col] = UnityEngine.Random.Range(-1.0f, 1.0f);
            }
        }

        // Create Particles
        for(int i = 0; i < n; ++i) {
            Instantiate(particle, new Vector3(UnityEngine.Random.Range(-HEIGHT, HEIGHT), UnityEngine.Random.Range(-WIDTH, WIDTH), 0), Quaternion.identity);
        }
        Particle[] particles = FindObjectsOfType<Particle>();
        int colorId;
        foreach(Particle p in particles) {
            colorId = UnityEngine.Random.Range(0,m);
            p.setColor(colorId);
        }
    }

    // Updates particles
    void Update()
    {
        if(Time.time < 1) {
            return;
        }
        // calculate force for each particle and update force
        Particle[] particles = FindObjectsOfType<Particle>();
        for(int i = 0; i < particles.Length; ++i) {
            float totalForceX = 0;
            float totalForceY = 0;

            for(int j = 0; j < particles.Length; ++j) {
                if(j == i) continue;
                Vector2 distance = particles[i].getDistance(particles[j]);
                // MAKE THIS FASTER
                float r = (float)Math.Sqrt(distance.x*distance.x + distance.y*distance.y);
                float attraction = (float)particles[i].getAttraction(particles[j]);
                if(r > 0 && r < rMax) {
                    float f = Force((float)(r / rMax), attraction);
                    totalForceX += distance.x / r * f; // attraction cos0
                    totalForceY += distance.y / r * f; // attraction sin0
                }
            }

            totalForceX *= (float)rMax * forceFactor;
            totalForceY *= (float)rMax * forceFactor;
            // apply friction (air resistance)
            frictionFactor = Math.Pow(0.5f, Time.deltaTime/frictionHalfLife);
            particles[i].Friction();
            // apply gravity towards center
            particles[i].Gravity(G);
            // apply force to particles
            particles[i].setForce(new Vector3(totalForceX, totalForceY, 0));
        }
    }
    // r - radius, a - attraction
    float Force(float r, float a) {
        float result;
        // repulsive force if r is less than beta (min distance)
        if(r < beta) {
            result = r / beta - 1;
        } else if (beta < r && r < 1) {
            result = a * (1 - Math.Abs(2 * r - 1 - beta) / (1 - beta));
        } else {
            result = 0;
        }

        return result;
    }
}

