using System;
using System.Collections.Generic;
using System.Linq;

public interface ITileSelector
{
	void SetTiles(IEnumerable<object> tiles);
	PlacedTile Select(int x, int y);
}

public class TileSelector : ITileSelector
{
	private readonly IConnectionsFinder _connectionsFinder;
	private readonly IRandom _random;
	private readonly NonStupidLookup<string, TileTemplate> _connectionToTilesMapping;
	private readonly ITwoDimensionalCollection<PlacedTile> _placedTiles;

	private const int FlipRotation = 2;

	public TileSelector(IConnectionsFinder connectionsFinder, IRandom random, ITwoDimensionalCollection<PlacedTile> placedTiles)
	{
		_connectionsFinder = connectionsFinder;
		_random = random;
		_connectionToTilesMapping = new NonStupidLookup<string, TileTemplate>();
		_placedTiles = placedTiles;
	}

	public void SetTiles(IEnumerable<object> tiles)
	{
		foreach (var tile in tiles)
		{
			var connectionSets = _connectionsFinder.FindConnectionSets(tile);

			foreach (var connectionSet in connectionSets)
			{
				_connectionToTilesMapping.Append(connectionSet.Connections, new TileTemplate(tile, connectionSet.Rotation));
			}
		}
	}

	public PlacedTile Select(int x, int y)
	{
		var requiredConnections = "";
		int rotationToRequiredConnections = 0;

		var leftTile = _placedTiles[x - 1, y];
		if (leftTile != null)
		{
			rotationToRequiredConnections = 1 + FlipRotation;
			requiredConnections += GetOppositeSide(GetSideConnections(leftTile.AllConnections, 1 - leftTile.Rotation));
		}
		var topTile = _placedTiles[x, y - 1];
		if (topTile != null)
		{
			rotationToRequiredConnections = requiredConnections == null ? 2 + FlipRotation : rotationToRequiredConnections;
			requiredConnections += GetOppositeSide(GetSideConnections(topTile.AllConnections, 2 - topTile.Rotation));
		}

		List<TileTemplate> possibleTemplates;
		if (requiredConnections != "")
		{
			if (!_connectionToTilesMapping.HasKey(requiredConnections))
			{
				throw new NoTileWithConnections(requiredConnections);
			}
			possibleTemplates = _connectionToTilesMapping[requiredConnections].ToList();
		}
		else
		{
			var keyGroupIndex = _random.Range(0, _connectionToTilesMapping.GetKeyGroupCount());
			possibleTemplates = _connectionToTilesMapping.GetKeyGroupByIndex(keyGroupIndex).ToList();
		}
		var selectedTemplate = possibleTemplates[_random.Range(0, possibleTemplates.Count())];

		return PlaceTile(x, y, selectedTemplate.Tile, selectedTemplate.Rotation + rotationToRequiredConnections);
	}

	private string GetSideConnections(string allConnections, int rotation)
	{
		rotation = GetNormalizedRotation(rotation);
		return allConnections.Substring(rotation*2, 2);
	}

	private static int GetNormalizedRotation(int rotation)
	{
		rotation = rotation%4;
		rotation = rotation < 0 ? 4 + rotation : rotation;
		return rotation;
	}

	private string GetOppositeSide(string sideConnections)
	{
		return sideConnections.Substring(1, 1) + sideConnections.Substring(0, 1);
	}

	public PlacedTile PlaceTile(int x, int y, object tile, int rotation)
	{
		var completeConnections = _connectionsFinder.GetCompleteConnectionsOriented(tile, rotation);
		return new PlacedTile(tile, completeConnections, GetNormalizedRotation(rotation));
	}
}

public class NoTileWithConnections : Exception
{
	public NoTileWithConnections(string connections) : base("No tile with following connections: " + connections)  { }
}

public class PlacedTile
{
	public object Tile { get; private set; }
	public string AllConnections { get; private set; }
	public int Rotation { get; private set; }

	public PlacedTile(object tile, string allConnections, int rotation)
	{
		Tile = tile;
		AllConnections = allConnections;
		Rotation = rotation;
	}
}

public class TileTemplate
{
	public TileTemplate(object tile, int rotation)
	{
		Tile = tile;
		Rotation = rotation;
	}

	public object Tile { get; private set; }
	public int Rotation { get; private set; }
}