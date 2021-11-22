using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private float ropeSegmentLength = 0.5f;
    private int segmentNumber = 20;
    private float lineWidth = 0.1f;

    private void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = this.transform.position;

        for(int i = 0; i < segmentNumber; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegmentLength;
        }
    }

    private void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();
    }

    private void Simulate()
    {
        Vector2 gravityForce = new Vector2(0f, -1f);

        for(int i = 0; i < this.segmentNumber; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += gravityForce * Time.deltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        for(int i = 0; i < 20; i++)
        {
            this.ApplyConstraints();
        }
    }

    private void ApplyConstraints()
    {
        RopeSegment firstSegment = this.ropeSegments[0];
        RopeSegment lastSegment = this.ropeSegments[segmentNumber - 1];
        firstSegment.posNow = this.transform.position;
        lastSegment.posNow = this.transform.parent.position;
        this.ropeSegments[0] = firstSegment;
        this.ropeSegments[segmentNumber - 1] = lastSegment;

        for(int i = 0; i < this.segmentNumber - 2; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = dist - this.ropeSegmentLength;
            Vector2 changeDirHelp = (firstSeg.posNow - secondSeg.posNow).normalized;
            Vector2 changeDir = changeDirHelp * error;

            Vector2 changeAmount = changeDir * error;
            if(i != 0 || i!= segmentNumber - 2)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }else if(i == 0)
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                firstSeg.posNow -= changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
                
            }
        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentNumber];
        for(int i = 0; i < this.segmentNumber; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }
    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

}
