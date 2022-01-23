using UnityEngine;

namespace Tiles
{
    public class ModeToggleTile : Tile
    {
        public Tile[] TargetTiles;
        public AudioSource SFX;
        public AudioClip TriggerSFX;

        public Tile ActivateTile;
        public Tile DeactiveTile;
        public Tile BlackActiveTile;
        public Tile BlackDeactiveTile;

        public override void OnTileEnter()
        {
            var activeRenderer = ActivateTile.GetComponent<SpriteRenderer>();
            var deactiveRenderer = DeactiveTile.GetComponent<SpriteRenderer>();
            var blackActiveRenderer = BlackActiveTile.GetComponent<SpriteRenderer>();
            var blackDeactiveRenderer = BlackDeactiveTile.GetComponent<SpriteRenderer>();

            SFX.PlayOneShot(TriggerSFX);
            
            foreach (var tile in TargetTiles)
            {
                var renderer = tile.GetComponent<SpriteRenderer>();
                if (tile.IsBlack)
                {
                    if (IsActive(tile, BlackActiveTile))
                    {
                        UpdateTile(tile, BlackDeactiveTile);
                    }
                    else
                    {
                        UpdateTile(tile, BlackActiveTile);
                    }
                }
                else
                {
                    if (IsActive(tile, ActivateTile))
                    {
                        UpdateTile(tile, DeactiveTile);
                    }
                    else
                    {
                        UpdateTile(tile, ActivateTile);
                    }
                }
            }
        }

        bool IsActive(Tile tile, Tile activeTile)
        {
            return tile.gameObject.layer == activeTile.gameObject.layer;
        }

        void UpdateTile(Tile tile, Tile reference)
        {
            var renderer = tile.GetComponent<SpriteRenderer>();
            var referenceRenderer = reference.GetComponent<SpriteRenderer>();

            tile.gameObject.layer = reference.gameObject.layer;
            renderer.sprite = referenceRenderer.sprite;
            renderer.sortingLayerName = referenceRenderer.sortingLayerName;
        }
    }
}