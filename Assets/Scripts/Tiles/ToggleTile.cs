using UnityEngine;

namespace Tiles
{
    public class ToggleTile : Tile
    {
        public Tile[] TargetTiles;

        public override void OnTileEnter()
        {
            foreach (var tile in TargetTiles)
            {
                tile.gameObject.SetActive(!tile.gameObject.activeSelf);
            }
        }
    }
}