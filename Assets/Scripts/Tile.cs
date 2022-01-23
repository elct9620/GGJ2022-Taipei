using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsBlack = false;
    public virtual void OnTileEnter()
    {
    }

    public virtual void OnTileLeave()
    {
    }
}