using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Transform DetectPoint;
    public float Speed;
    public float SeperationDistance;
    [Header("Max Rotation Per Second")]
    public float SeperationRotationSpeed;
    public float CohesionRotationSpeed;
    public float AllignmentRotationSpeed;
    public float GridBoundsRotationSpeed;

    private Rigidbody2D rb;
    private Vector2 gridMiddle;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        gridMiddle = new Vector2(BoidManager.Instance.GridSize.x / 2, BoidManager.Instance.GridSize.y / 2);
        SeperationRotationSpeed /= 50;
        CohesionRotationSpeed /= 50;
        AllignmentRotationSpeed /= 50;
        GridBoundsRotationSpeed /= 50;
}

    public void FixedUpdate()
    {
        List<Boid> neighbours = BoidManager.Instance.GetNeighbours(this);
        Vector3 neighbourCenter = Vector2.zero;
        float directionAverage = 0;

        foreach (Boid currentBoid in neighbours)
        {
            //Speration
            if (Vector3.Distance(transform.position, currentBoid.transform.position) < SeperationDistance)
            {
                RotateAwayFromPos(currentBoid.transform.position, SeperationRotationSpeed);
            }

            //Cohesion
            neighbourCenter += transform.position;

            //Allignment
            directionAverage += transform.rotation.z;
        }

        //Cohesion
        neighbourCenter /= neighbours.Count;
        RotateTowardsPos(neighbourCenter, CohesionRotationSpeed);

        //Allignment
        directionAverage /= neighbours.Count;

        if (!IsInGridBounds())
        {
            RotateTowardsPos(gridMiddle, GridBoundsRotationSpeed);
        }
        else
        {
            RotateTowardsAngle(directionAverage, AllignmentRotationSpeed);
        }


        MoveForward();
    }

    private void MoveForward()
    {
        float zRotation = transform.rotation.eulerAngles.z;
        Quaternion rotation = Quaternion.Euler(0f, 0f, zRotation);
        Vector2 forwardDirection = rotation * Vector2.up;
        rb.AddForce(forwardDirection * Speed, ForceMode2D.Impulse);
    }

    private void RotateTowardsPos(Vector2 _pos, float _speed)
    {
        float targetAngle = GetAngle(_pos) - 90;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), _speed);
    }

    private void RotateAwayFromPos(Vector2 _pos, float _speed)
    {
        float targetAngle = GetAngle(_pos) + 90;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetAngle), _speed);
    }

    private float GetAngle(Vector2 _targetPos)
    {
        Vector2 difference = _targetPos - new Vector2(transform.position.x, transform.position.y);
        difference.Normalize();
        return Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    }

    private void RotateTowardsAngle(float _angle, float _speed)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, _angle), _speed);
    }

    private bool IsInGridBounds()
    {
        if (transform.position.x < 0 || transform.position.x > BoidManager.Instance.GridSize.x)
        {
            return false;
        }

        if (transform.position.y < 0 || transform.position.y > BoidManager.Instance.GridSize.y)
        {
            return false;
        }

        return true;
    }
}
