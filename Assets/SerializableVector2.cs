using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public struct SerializableVector2
{
	public float x;
	public float y;

	public SerializableVector2(float aX, float aY)
	{
		x = aX;
		y = aY;
	}

	public override string ToString()
	{
		return String.Format("[{0}, {1}]", x, y);
	}

	public static implicit operator Vector2(SerializableVector2 aValue)
	{
		return new Vector2(aValue.x, aValue.y);
	}

	public static implicit operator SerializableVector2(Vector2 aValue)
	{
		return new SerializableVector2(aValue.x, aValue.y);
	}
}