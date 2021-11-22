using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeRope : MonoBehaviour
{
    public GameObject segment;
    public float segmentLength = 0.1f;

    List<GameObject> segments = new List<GameObject>();

    private void Awake()
    {
        


    }

    public void CreateRope()
    {
        Vector2 rope = transform.position - transform.parent.position;
        transform.up = rope;
        float length = rope.magnitude;
        int segmentNumber = (int)(length / segmentLength);

        for(int i = 0; i < segmentNumber; i++)
        {
            AddSegment();
        }
    }

    private void AddSegment()
    {
        if (segments.Count > 0)
        {
            
            GameObject temp = Instantiate(segment, SetTop(segments[segments.Count - 1], GetBottom(segments[segments.Count - 1])), transform.rotation, transform) as GameObject;
            HingeJoint2D hj = temp.GetComponent<HingeJoint2D>();
            hj.anchor = new Vector2(0, 0.5f);
            hj.connectedBody = segments[segments.Count - 1].GetComponent<Rigidbody2D>();
            segments.Add(temp);
        }
        else
        {
            GameObject temp = Instantiate(segment, SetTop(this.gameObject, GetBottom(this.gameObject)), transform.rotation, transform) as GameObject;
            temp.GetComponent<HingeJoint2D>().anchor = new Vector2(0, 0.5f);

            segments.Add(temp);
        }
    }

    

    Vector2 SetTop(GameObject obj, Vector3 pos)
    {
        float height = obj.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector2 newPos = pos - obj.transform.up * height / 2;
        return newPos;
    }
    Vector2 GetBottom(GameObject obj)
    {
        float height = obj.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector2 bottom = obj.transform.position - obj.transform.up * height / 2;
        return bottom;
    }

    public Rigidbody2D GetLastSegmentRB()
    {
        return segments[segments.Count - 1].GetComponent<Rigidbody2D>();
    }
}
