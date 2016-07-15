using UnityEngine;
using System;
using System.Collections.Generic;
using AutomatedMovment;

public class BezierSpline
{
    class WalkerWrapper
    {
        public float startTime;
        public float endTime;
        public IPath walker;

        public WalkerWrapper(IPath walker)
        {
            this.walker = walker;
        }
    }

    List<WalkerWrapper> walkers = new List<WalkerWrapper>();

    public Vector3 LastPoint
    {
        get
        {
            return walkers[walkers.Count - 1].walker.LastPoint;
        }
    }

    public Vector3 FirstPoint
    {
        get
        {
            return walkers[0].walker.FirstPoint;
        }
    }

    public BezierSpline(IPath firstSegment)
    {
        AddPath(firstSegment);
    }

	//private void EnforceMode (int index)
 //   {
	//	int modeIndex = (index + 1) / 3;
	//	BezierControlPointMode mode = modes[modeIndex];
	//	if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1)) {
	//		return;
	//	}

	//	int middleIndex = modeIndex * 3;
	//	int fixedIndex, enforcedIndex;
	//	if (index <= middleIndex) {
	//		fixedIndex = middleIndex - 1;
	//		if (fixedIndex < 0) {
	//			fixedIndex = points.Length - 2;
	//		}
	//		enforcedIndex = middleIndex + 1;
	//		if (enforcedIndex >= points.Length) {
	//			enforcedIndex = 1;
	//		}
	//	}
	//	else {
	//		fixedIndex = middleIndex + 1;
	//		if (fixedIndex >= points.Length) {
	//			fixedIndex = 1;
	//		}
	//		enforcedIndex = middleIndex - 1;
	//		if (enforcedIndex < 0) {
	//			enforcedIndex = points.Length - 2;
	//		}
	//	}

	//	Vector3 middle = points[middleIndex];
	//	Vector3 enforcedTangent = middle - points[fixedIndex];
	//	if (mode == BezierControlPointMode.Aligned) {
	//		enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
	//	}
	//	points[enforcedIndex] = middle + enforcedTangent;
	//}


    private Vector3 GetPoint(WalkerWrapper walker, float relativeTime)
    {
        return walker.walker.GetPoint(relativeTime);
    }

    /* 
     * Expects a value between 0-1
     */
	public Vector3 GetPoint (float progressPercentage)
    {
        WalkerWrapper current = WalkerForTime(progressPercentage);

        progressPercentage = (progressPercentage - current.startTime) / (current.endTime - current.startTime);

        return current.walker.GetPoint(progressPercentage);
	}

    private WalkerWrapper WalkerForTime(float progressPercentage)
    {
        for (int i = 0; i < walkers.Count; ++i)
        {
            if (walkers[i].startTime <= progressPercentage &&
                walkers[i].endTime > progressPercentage)
                return walkers[i];
        }

        return walkers[walkers.Count - 1];
    }

 //   public Vector3 GetVelocity (float t) {
	//	int i;
	//	if (t >= 1f) {
	//		t = 1f;
	//		i = points.Length - 4;
	//	}
	//	else {
	//		t = Mathf.Clamp01(t) * CurveCount;
	//		i = (int)t;
	//		t -= i;
	//		i *= 3;
	//	}
	//	return Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t);
	//}
	
	//public Vector3 GetDirection (float t) {
	//	return GetVelocity(t).normalized;
	//}

 //   public void AddCurve(Vector3 firstControlPoint, Vector3 secondControlPoint, Vector3 dst)
 //   {
 //       Array.Resize(ref points, points.Length + 3);
 //       points[points.Length - 3] = firstControlPoint;
 //       points[points.Length - 2] = firstControlPoint;
 //       points[points.Length - 1] = dst;

 //       Array.Resize(ref modes, modes.Length + 1);
 //       modes[modes.Length - 1] = modes[modes.Length - 2];
 //       EnforceMode(points.Length - 4);

 //       if (loop)
 //       {
 //           points[points.Length - 1] = points[0];
 //           modes[modes.Length - 1] = modes[0];
 //           EnforceMode(0);
 //       }
 //   }

    public void AddPath (IPath walker)
    {
        var wrapper = new WalkerWrapper(walker);
        walkers.Add(wrapper);
        calculateTimeSegments();
    }

    private void calculateTimeSegments()
    {
        WalkerWrapper prev = walkers[0];
        prev.endTime = 0f;

        for (int i = 0; i < walkers.Count; ++i)
        {
            var walker = walkers[i];

            walker.startTime = prev.endTime;
            walker.endTime = walker.startTime + walker.walker.Distance / walker.walker.Speed;
            prev = walker;
        }

        NormalizeSegmentTimes();
    }

    /*
     * This normalizes the times to be between 0 and 1, thus allowing
     * an outside viewer to control the entire spline time without
     * knowing internal timings, but only the relative speeds
     */
    private void NormalizeSegmentTimes()
    {
        for (int i = 0; i < walkers.Count; ++i)
        {
            walkers[i].startTime /= walkers[walkers.Count - 1].endTime;
            walkers[i].endTime /= walkers[walkers.Count - 1].endTime;
        }
    }
}