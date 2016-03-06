using UnityEngine;
using System.Collections;

public class SpiralEffectScript : ShaderEffectScript 
{
	[SerializeField]
	private Color 	m_lineColor = new Color(0f, 0f, 0f, 1f);
	[SerializeField]
	private float 	m_colorFill;
	[SerializeField]
	private int 	m_lineCount;
	[SerializeField]
	private float 	m_lineWidth;
	[SerializeField]
	private bool 	m_reversed;

	private float 	m_location;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		if(m_material)
		{
			m_material.SetVector("_Color", m_lineColor);
			m_material.SetFloat("_LineWidth", m_lineWidth);
			m_material.SetInt("_LineCount", m_lineCount);
			m_material.SetFloat("_ColorFill", m_colorFill);
		}
		m_location = 0f;
		if(m_reversed)
			m_location = 1f;
	}
	
	// Update is called once per frame
	public override void Update() 
	{
		base.Update();
		if(m_material)
		{
			if(!m_reversed)
			{
				m_location += Time.deltaTime * m_effectSpeed;
				if(m_location >= 1f)
					m_location = 0f;			
			}
			else
			{
				m_location -= Time.deltaTime * m_effectSpeed;
				if(m_location <= 0f)
					m_location = 1f;
			}
			m_material.SetFloat("_Location", m_location);	
		}
	}
}
