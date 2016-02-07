 using UnityEngine;
 using System;
 using System.Collections;
 
 [System.Serializable]
 public struct SerializableVector3
 {
     public float x;
     public float y;
     public float z;
     
     public SerializableVector3(float aX, float aY, float aZ)
     {
         x = aX;
         y = aY;
         z = aZ;
     }
     
     public override string ToString()
     {
         return String.Format("[{0}, {1}, {2}]", x, y, z);
     }

     public static implicit operator Vector3(SerializableVector3 aValue)
     {
		return new Vector3(aValue.x, aValue.y, aValue.z);
     }

	public static implicit operator SerializableVector3(Vector3 aValue)
     {
		return new SerializableVector3(aValue.x, aValue.y, aValue.z);
     }
 }