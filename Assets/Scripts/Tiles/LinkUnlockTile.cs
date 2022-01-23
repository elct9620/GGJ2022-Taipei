using UnityEngine;

namespace Tiles
{
    public class LinkUnlockTile : Tile
    {
        public bool isLocked = true;
        public Tile[] UnlockTargets;
        public LinkUnlockTile[] Children;

        public AudioSource SFX;
        public AudioClip UnlockSFX;

        public override void OnTileEnter()
        {
            isLocked = false;

            bool childrenLocked = true;
            foreach (var child in Children)
            {
                childrenLocked &= child.isLocked;
            }

            if (!childrenLocked)
            {
                if (UnlockSFX)
                {
                    SFX.PlayOneShot(UnlockSFX);
                }
                foreach (var tile in UnlockTargets)
                {
                    tile.gameObject.SetActive(false);
                }

                gameObject.SetActive(false);
                foreach (var child in Children)
                {
                   child.gameObject.SetActive(false); 
                }
            }
        }

        public override void OnTileLeave()
        {
            isLocked = true;
        }
    }
}