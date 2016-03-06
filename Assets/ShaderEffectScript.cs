using UnityEngine;
using System.Collections;

public class ShaderEffectScript : MonoBehaviour 
{
	[SerializeField]
	protected float m_effectSpeed = 0.5f;
	protected Material m_material;

	// Use this for initialization
	protected virtual void Start () 
	{
		m_material = GetComponent<SpriteRenderer>().material;
	}
	
	// Update is called once per frame
	public virtual void Update() 
	{	
	}
}
