using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tiles
{
    public class ExitTile : Tile
    {
        public bool isLocked = true;
        public ExitTile[] Children;

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
                SceneManager.LoadScene("Clear");
            }
        }

        public override void OnTileLeave()
        {
            isLocked = true;
        }
    }
}