using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public BezierSpline Spline;

	public float duration;
	public float progress;
    public Vector3 offset = new Vector3(0, 0, 0);

	public SplineWalkerMode mode;

	private bool goingForward = true;

	private void Update () {
		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				if (mode == SplineWalkerMode.Once) {
					progress = 1f;
				}
				else if (mode == SplineWalkerMode.Loop) {
					progress -= 1f;
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
        transform.position = Vector3.Lerp(transform.position, position, 0.05f);
			//transform.LookAt(position + Spline.GetDirection(progress));
	}
}