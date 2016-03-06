using UnityEngine;
using System.Collections;

public class ShinyEffectScript : ShaderEffectScript  
{
	[SerializeField]
	protected Color		m_color;
	[SerializeField]
	protected float		m_colorFill = 0.5f;
	[SerializeField]
	protected float		m_colorWidth = 0.5f;
	[SerializeField]
	protected float		m_effectWidth = 0.1f;
	private float 		m_location;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		if(m_material)
		{
			m_material.SetVector("_Color", m_color);
			m_material.SetFloat("_EffectWidth", m_effectWidth);
			m_material.SetFloat("_ShineWidth", m_colorWidth);
			m_material.SetFloat("_ColorFill", m_colorFill);	
		}
		m_location = -1f;
	}
	
	// Update is called once per frame
	public override void Update() 
	{
		base.Update();
		if(m_material)
		{
			m_location += Time.deltaTime * m_effectSpeed;
			if(m_location >= 2f)
				m_location = -1f;
			m_material.SetFloat("_ShineLocation", m_location);	
		}
	}
}
