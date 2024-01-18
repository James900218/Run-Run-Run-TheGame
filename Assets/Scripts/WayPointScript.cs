using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointScript : MonoBehaviour
{
    //attributes
    public bool wayPointVisited;        //bool for is the enemy has seen this waypoint
    public float timeSinceLastVisit;    //time since enemy has seen this waypoint
    public float wpTimer;               //timer until waypoint is no longer visited

    public Transform wpPos;


    private void Awake()
    {
        wayPointVisited = false;
        timeSinceLastVisit = 0;
        wpTimer = 60f;
    }

    // Update is called once per frame
    void Update()
    {
        // if wp visited, start timer until unvisited and timeSince = 0
        if (wayPointVisited == true)
        {
            timeSinceLastVisit = 0f;
            wpTimer -= Time.deltaTime;
        }
        else if (wayPointVisited == false)
        {
            timeSinceLastVisit += Time.deltaTime;
            wpTimer = 60f;
        }

        if (wpTimer <= 0f)
        {
            wayPointVisited = false;
        }
    }

    public void WpVisited()
    {
        wayPointVisited = true;
    }

    public bool GetWPStatus()
    {
        return wayPointVisited;
    }

    public float GetWPTimer()
    {
        return timeSinceLastVisit;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
}
