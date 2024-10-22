using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] public float mass; // you can set when instantiating object with a method
    public Vector3 velocity;
    public Vector3 force;
    public int color;

    // Start is called before the first frame update
    void Start()
    {
        mass = 1.0f;
        velocity = new Vector3(0,0,0);
        force = new Vector3(0,0,0);
        //if (Math.Abs(Time.time-5) < 0.1)
        //    transform.position = new Vector3(UnityEngine.Random.Range(-Main.HEIGHT, Main.HEIGHT), UnityEngine.Random.Range(-Main.WIDTH, Main.WIDTH), 0);
    }
    // Update is called once per frame
    void Update()
    {
        
        // update velocity
        velocity += new Vector3 ((force.x/mass)*Time.deltaTime,(force.y/mass)*Time.deltaTime,0);

        // update position
        Move(velocity.x*Time.deltaTime, velocity.y*Time.deltaTime);
    }
    public void setForce(Vector3 f) {
        force = f;
    }
    public void Friction() {
        velocity = new Vector3((float)velocity.x*(float)Main.frictionFactor, (float)velocity.y*(float)Main.frictionFactor, 0);
    }
    public void Gravity(float G) {
        transform.position = new Vector3((float)transform.position.x*G, (float)transform.position.y*G, 0);
    }
    public void Move(float x, float y) {
        // x -8.89 -> 8.88
        // float xcurrent = transform.position.x;
        // float ycurrent = transform.position.y;
        // if(xcurrent > Main.HEIGHT) {
        //     x -= 2*Main.HEIGHT;
        // } else if(xcurrent < -Main.HEIGHT) {
        //     x += 2*Main.HEIGHT;
        // }
        // if(ycurrent > Main.WIDTH) {
        //     y -= 2*Main.WIDTH;
        // } else if(xcurrent < -Main.WIDTH) {
        //     y += 2*Main.WIDTH;
        // }

        // 
        transform.position += new Vector3(x, y, 0);
    }
    public void Move(Vector3 vector) {
        transform.position += vector;
    }

    public void setColor(int colorid, Color c) {
        this.gameObject.GetComponentInChildren<Renderer>().material.color = c;
        // This is getting the first circle in the whole game instead of itself
        color = colorid;
    }
    public void setColor(int colorid) {
        this.gameObject.GetComponentInChildren<Renderer>().material.color = Color.HSVToRGB((float)colorid / (float)Main.m, 1.0f, .8f);
        color = colorid;
    }

    public Vector2 getDistance(Particle other) {
        float rx = other.transform.position.x - transform.position.x;
        float ry = other.transform.position.y - transform.position.y;
        return new Vector2(rx, ry);
    }

    public double getAttraction(Particle other) {
        return Main.attractionMatrix[color, other.color];
    }
}
