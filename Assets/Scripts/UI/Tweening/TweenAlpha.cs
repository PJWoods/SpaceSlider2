//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the object's alpha.
/// </summary>

[AddComponentMenu("NGUI/Tween/Alpha")]
public class TweenAlpha : UITweener
{
	public float from = 1f;
	public float to = 1f;

	Transform mTrans;
	Text mText;
	Image mImage;

	/// <summary>
	/// Current alpha.
	/// </summary>

	public float alpha
	{
		get
		{
			if (mText != null) return mText.color.a;
			if (mImage != null) return mImage.color.a;
			return 0f;
		}
		set
		{
			if (mText != null) mText.color = new Color(mText.color.r, mText.color.g, mText.color.b, value);
			else if (mImage != null) mImage.color = new Color(mText.color.r, mText.color.g, mText.color.b, value);
        }
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
        mImage = GetComponent<Image>();
		if (mImage == null) mText = GetComponent<Text>();
	}

	/// <summary>
	/// Interpolate and update the alpha.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { alpha = Mathf.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAlpha Begin (GameObject go, float duration, float alpha)
	{
		TweenAlpha comp = UITweener.Begin<TweenAlpha>(go, duration);
		comp.from = comp.alpha;
		comp.to = alpha;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
