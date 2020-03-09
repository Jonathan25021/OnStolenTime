using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGengeratorScript : MonoBehaviour
{
    public int DungeonWidth;
    public int DungeonHeight;
    public int MaxRoomSize;
    public int MinRoomSize;
    public float MaxWidthHeightRatio;

    public class SubDungeon
    {
        private SubDungeon _l;
        private SubDungeon _r;
        private Rect _rect;
        


        public SubDungeon(float roomRatio, Rect r)
        {
            _rect = r;
            _l = null;
            _r = null;
        }

        public void Split()
        {

        }

        public bool IsLeaf()
        {
            return _l == null && _r == null;
        }

        public SubDungeon GetL()
        {
            return _l;
        }
        public SubDungeon GetR()
        {
            return _r;
        }

    }


    #region unityFuncs
    // Start is called before the first frame update
    void Start()
    {
        SubDungeon root = new SubDungeon(MaxWidthHeightRatio, new Rect(0, 0, DungeonWidth, DungeonHeight));


    }
    #endregion
    
}
