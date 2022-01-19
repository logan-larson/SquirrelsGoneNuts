using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCone
{

    private Vector3 origin;
    private Vector3 direction;
    private Vector3 orthogonalDirection;
    private float radius;
    private int numCasts;
    private float range;

    private float drawDistance;

    private Vector3[] points;
    private Vector3[] normals;
    private float[] distances;

    private Vector3 averagePoint;
    private Vector3 averageNormal;

    public RaycastCone(Vector3 origin, Vector3 direction, Vector3 orthogonalDirection, float radius, int numCasts, float range, float drawDistance)
    {
        this.origin = origin;
        this.direction = direction;
        this.orthogonalDirection = orthogonalDirection;
        this.radius = radius;
        this.numCasts = numCasts;
        this.range = range;

        this.drawDistance = drawDistance;

        this.points = new Vector3[numCasts];
        this.normals = new Vector3[numCasts];
        this.distances = new float[numCasts];

        this.Cast();
    }

    public void Cast()
    {
        float angle = 360 / this.numCasts;

        int numHit = 0;
        Vector3 totalPoints = new Vector3();
        Vector3 totalNormals = new Vector3();

        for (int i = 0; i < this.numCasts; i++)
        {
            float iAngle = i * angle;

            // Creates a direction to 
            Quaternion offsetAngle = Quaternion.AngleAxis(iAngle, this.direction);

            Vector3 offsetVector = (offsetAngle * this.orthogonalDirection) * this.radius;

            Vector3 rayDirection = (this.origin + (this.direction + offsetVector)) - this.origin;

            // Creates Vector3 direction of angle 

            RaycastHit hit;
            Ray r = new Ray(this.origin, rayDirection);
            if (Physics.Raycast(r, out hit, this.range))
            {
                this.points[i] = hit.point;
                this.normals[i] = hit.normal;
                this.distances[i] = hit.distance;

                numHit++;
                totalPoints += hit.point;
                totalNormals += hit.normal;

                Debug.DrawLine(r.origin, hit.point, Color.green);
            }
            else
            {
                Debug.DrawRay(r.origin, r.direction, Color.red);
            }
        }

        this.averagePoint = totalPoints / numHit;
        this.averageNormal = totalNormals / numHit;
    }

    public Vector3 GetPoint(int index) {
        return this.points[index];
    }

    public Vector3 GetNormal(int index) {
        return this.normals[index];
    }

    public int GetClosestIndex() {
        int i = 0;
        float closestDistance = 100000000f;
        int ret = 0;

        while (i < this.distances.Length)
        {
            if (this.distances[i] < closestDistance)
            {
                closestDistance = this.distances[i];
                ret = i;
            }
            i++;
        }

        return ret;
    }

    public Vector3 GetClosestPoint()
    {
        int i = 0;
        float closestDistance = 100000000f;
        Vector3 closestPoint = new Vector3();

        while (i < this.distances.Length)
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

        while (i < this.distances.Length)
        {
            if (this.distances[i] < closestDistance)
            {
                closestDistance = this.distances[i];
            }
            i++;
        }

        return closestDistance;
    }

    public float GetClosestDistanceToPoint(Vector3 point)
    {
        int i = 0;
        float closestDistance = 100000000f;

        while (i < this.points.Length)
        {
            if ((this.points[i] - point).magnitude < closestDistance)
            {
                closestDistance = this.distances[i];
            }
            i++;
        }

        return closestDistance;
    }

    public float GetAverageToCenter() {
        return 1f;
    }

    public Vector3 GetAveragePoint() {
        return this.averagePoint;
    }

    public Vector3 GetAverageNormal() {
        return this.averageNormal;
    }

}
