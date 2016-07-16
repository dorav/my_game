using System;
using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline Spline;

	public float duration;
	public float progress = 0;
    public Vector3 offset = new Vector3(0, 0, 0);

	public SplineWalkerMode mode;

	private bool goingForward = true;

	public virtual void FixedUpdate () {
		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				if (mode == SplineWalkerMode.Once) {
					progress = 1f;
				}
				else if (mode == SplineWalkerMode.Loop) {
                    LoopWalker();
				}
				else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
        }
		else {
			progress -= Time.deltaTime / duration;
            if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		Vector3 position = Spline.GetPoint(progress) + offset;
        SetPosition(position);
			//transform.LookAt(position + Spline.GetDirection(progress));
	}

    public virtual void LoopWalker()
    {
        progress -= 1f;
    }

    public virtual void SetPosition(Vector3 newPos)
    {
        transform.position = Vector3.Lerp(transform.position, newPos, 0.05f);
    }
}