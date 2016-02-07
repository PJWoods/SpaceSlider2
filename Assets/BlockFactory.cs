using UnityEngine;
using System;
using System.Collections.Generic;

public class BlockFactory
{
	private static BlockFactory m_instance;
	public static BlockFactory Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new BlockFactory();
			}
			return m_instance;
		}
		set { m_instance = value; }
	}

	public BlockBase CreateBlock(BlockBase.BlockProperty type)
	{
		string prefabName = null;

		switch(type)
		{
		case BlockBase.BlockProperty.Empty:
			break;
		case BlockBase.BlockProperty.Movable:
			prefabName = "Movable";
			break;
		case BlockBase.BlockProperty.NonMovable:
			prefabName = "NonMovable";
			break;
		case BlockBase.BlockProperty.PowerUp:
			prefabName = "PowerUp";
			break;
		case BlockBase.BlockProperty.LaneChangerLeft:
			prefabName = "LaneChangerLeft";
			break;
		case BlockBase.BlockProperty.LaneChangerRight:
			prefabName = "LaneChangerRight";
			break;
		default:
			break;
		}

		if(prefabName != null)
		{
			GameObject block = GameObjectPool.Instance.GetFromPool(prefabName, true);
			if(block == null)
				return null;
			return block.GetComponent<BlockBase>();
		}
		return null;
	}
}

