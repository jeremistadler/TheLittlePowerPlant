using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityGeneration
{
	public float NrOfTiles { get; set; }
	public float TileDimension { get; set; }
	private readonly IRandom _random;
	private readonly IBlockFactory _blockFactory;
	private readonly ITileSelector _tileSelector;

	public CityGeneration(IRandom random, IBlockFactory blockFactory, ITileSelector tileSelector)
	{
		_random = random;
		_blockFactory = blockFactory;
		_tileSelector = tileSelector;
	}

	public void Generate()
	{
		for (int x = 0; x < NrOfTiles; x++)
		{
			for (int z = 0; z < NrOfTiles; z++)
			{
				_blockFactory.Create(_tileSelector.Select(x, z), new Vector3(x * TileDimension, 0, z * TileDimension), new Vector3(0, 90 * _random.Range(0, 4)));
			}
		}
	}

	public void SetTiles(IEnumerable<object> tiles)
	{
		_tileSelector.SetTiles(tiles);
	}
}

public interface IBlockFactory
{
	object Create(object block, Vector3 position, Vector3 rotation);
}