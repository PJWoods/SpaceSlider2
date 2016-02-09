//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>

[AddComponentMenu("NGUI/Tween/Position")]
public class TweenPosition : UITweener
{
	public Vector2 from;
	public Vector2 to;

    RectTransform mTrans;

	public RectTransform cachedTransform
    {
        get
        {
            if (mTrans == null)
                mTrans = GetComponent<RectTransform>();
            return mTrans;
        }
    }
	public Vector3 position
    {
        get
        {
            return cachedTransform.anchoredPosition;
        }
        set
        {
            cachedTransform.anchoredPosition = value;
        }
    }

	protected override void OnUpdate (float factor, bool isFinished)
    {
        cachedTransform.anchoredPosition = from * (1f - factor) + to * factor;
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenPosition Begin (GameObject go, float duration, Vector3 pos)
	{
		TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
		comp.from = comp.position;
		comp.to = pos;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}