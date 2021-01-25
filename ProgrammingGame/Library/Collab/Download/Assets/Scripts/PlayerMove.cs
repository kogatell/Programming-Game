using System.Collections;
using System.Collections.Generic;
using interactor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    //public List<Transform> points = new List<Transform>();
    
    public Transform point;
    private bool done;
    private Transform[] points;
    private bool walking = false;
    private Terminal terminal;
    

    public enum State
    {
        Walk,
        Waiting
    }

    private void Start()
    {
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        points = manager.GetComponent<VariableScript>().points.ToArray();
        //WalkLoop(points);
        terminal = FindObjectOfType<Terminal>();
    }


    void Update()
    {
        Action action = Interactor.GetAction();
        if (action != null && action.Type == ActionType.GetProblem)
        {
            ProblemHolder[] problemHolders = FindObjectsOfType<ProblemHolder>();
            ProblemHolder selected = null;
            for (int i = 0; i < problemHolders.Length; i++)
            {
                if (Vector3.Distance(problemHolders[i].transform.position, transform.position) <= 1.5f)
                {
                    selected = problemHolders[i];
                }
            }

            if (selected)
            {
                ProblemManager.Instance.CurrentProblemHolder = selected;
                ProblemManager.Instance.CurrentProblem = selected.Problem;
                terminal.OpenProblem();
                action.Answer(Boolean.True);
            }
            else
            {
                action.Answer(Boolean.False);
            }
        }
        if (!walking) return;
        Walk(point);
    }

    public bool WalkToPoint(int pointNumber)
    {
        if (pointNumber < 0 || pointNumber >= points.Length) return false;
        walking = true;
        point = points[pointNumber].transform;
        return true;
    }
    
    public void Walk(Transform point)
    {
        if (!point) return;
        float _speed = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, point.position, _speed);
        transform.LookAt(point.transform);
        if (CheckIfDone(transform, point.transform))
        {
            walking = false;
        }
    }

    public void WalkLoop(Transform [] points)
    {
        int i = 0;
        
        if (CheckIfDone(this.transform, points[points.Length - 1].transform))
        {
            done = true;
        }
        if (!CheckIfDone(this.transform, points[i]) && !done)
        {
            Walk(points[i]);
        }
        else i++;
    }

    private bool CheckIfDone(Transform point1, Transform point2)
    {
        if (Vector3.Distance(point1.transform.position, point.position) < 0.001f)
        {
            return true;
        }
        return false;
    }
}
