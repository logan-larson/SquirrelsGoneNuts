using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCone
{

    private Vector3 origin;
    private Vector3 direction;
    private float radius;
    private int numCasts;

    private float drawDistance;

    private Vector3[] points;
    private Vector3[] normals;
    private float[] distances;

    public RaycastCone(Vector3 origin, Vector3 direction, float radius, int numCasts, float drawDistance)
    {
        this.origin = origin;
        this.direction = direction;
        this.radius = radius;
        this.numCasts = numCasts;

        this.drawDistance = drawDistance;

        this.points = new Vector3[numCasts];
        this.normals = new Vector3[numCasts];
        this.distances = new float[numCasts];

        this.Cast();
    }

    public void Cast()
    {
        float angle = 360 / this.numCasts;

        for (int i = 0; i < this.numCasts; i++)
        {
            float iAngle = i * angle;

            // Creates a direction to 
            Quaternion q = Quaternion.AngleAxis(iAngle, -this.direction);

            Vector3 qV = q.eulerAngles;

            Vector3 rot = Quaternion.Euler(qV.x, qV.y, qV.z) * -this.direction;

            Debug.DrawLine(this.origin, this.origin + (rot * this.drawDistance));

            // Creates Vector3 direction of angle 
            Vector3 v = (q.eulerAngles.normalized * this.radius) + (this.origin + this.direction);

            RaycastHit hit;
            Ray r = new Ray(this.origin, this.origin + v.normalized);
            if (Physics.Raycast(r, out hit))
            {
                this.points[i] = hit.point;
                this.normals[i] = hit.normal;
                this.distances[i] = hit.distance;

                Debug.DrawRay(r.origin, r.direction * this.drawDistance, Color.green);
            }
            else
            {
                //Debug.DrawRay(r.origin, r.direction * this.drawDistance, Color.red);
            }
        }
    }

    public Vector3 GetClosestPoint()
    {
        int i = 0;
        float closestDistance = 100000000f;
        Vector3 closestPoint = new Vector3();

        while (this.distances[i] != 0f)
        {
            if (this.distances[i] < closestDistance)
            {
                closestDistance = this.distances[i];
                closestPoint = this.points[i];
            }
            i++;
        }

        return closestPoint;
    }

    public float GetClosestDistance()
    {
        int i = 0;
        float closestDistance = 100000000f;

        while (this.distances[i] != 0f)
        {
            if (this.distances[i] < closestDistance)
            {
                closestDistance = this.distances[i];
            }
            i++;
        }

        return closestDistance;
    }

}
