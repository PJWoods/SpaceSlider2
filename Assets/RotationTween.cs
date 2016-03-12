using UnityEngine;
using System.Collections;

public class RotationTween : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

	void Update ()
    {
        transform.Rotate(Vector3.forward * (Time.deltaTime * m_speed));
    }
}
