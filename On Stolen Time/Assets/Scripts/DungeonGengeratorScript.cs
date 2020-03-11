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
        private float _maxWHRatio;
        private int _minRoomSize;
        private int _maxRoomSize;
        private Rect room;

        public SubDungeon(float roomRatio, Rect r, float MaxWHRatio, int minRoomSize, int maxRoomSize)
        {
            _rect = r;
            _maxWHRatio = MaxWHRatio;
            _minRoomSize = minRoomSize;
            _maxRoomSize = maxRoomSize;
            _l = null;
            _r = null;
        }

        public bool Split()
        {
            if (Mathf.Min(_rect.height, _rect.width) / 2 < _minRoomSize)
            {
                return false;
            }
            if (Mathf.Min(_rect.height, _rect.width) < _maxRoomSize)
            {
                int dice = (int)Random.Range(1, _maxRoomSize - Mathf.Min(_rect.height, _rect.width));
            }
            if (_rect.width / _rect.height >= _maxWHRatio)
            {
                SplitVert();
            }
            else
            {
                SplitHoriz();
            }
            return true;
        }

        private void SplitHoriz()
        {
            if (!IsLeaf())
             {
                int divider = Random.Range(_minRoomSize, (int) _rect.width - _minRoomSize);
                _l = new SubDungeon(_maxWHRatio, new Rect(_rect.x, _rect.y, _rect.width, divider), _maxWHRatio, _minRoomSize, _maxRoomSize);
                _r = new SubDungeon(_maxWHRatio, new Rect(_rect.x, _rect.y + divider, _rect.width, _rect.height - divider), _maxWHRatio, _minRoomSize, _maxRoomSize);
            }
        }

        private void SplitVert()
        {
            if (!IsLeaf())
            {
                int divider = Random.Range(_minRoomSize, (int) _rect.height - _minRoomSize);
                _l = new SubDungeon(_maxWHRatio, new Rect(_rect.x, _rect.y, divider, _rect.height), _maxWHRatio, _minRoomSize, _maxRoomSize);
                _r = new SubDungeon(_maxWHRatio, new Rect(_rect.x + divider, _rect.y, _rect.width - divider, _rect.height), _maxWHRatio, _minRoomSize, _maxRoomSize);
            }
        }

        public void CreateRoom()
        {
            if (IsLeaf())
            {
                int roomWidth = (int)Random.Range(_rect.width / 2, _rect.width - 2);
                int roomHeight = (int)Random.Range(_rect.height / 2, _rect.height - 2);
                int roomX = (int)Random.Range(1, _rect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, _rect.height - roomHeight - 1);
                room = new Rect(roomWidth, roomHeight, roomX, roomY);
            }
            else
            {
                if (_l != null)
                {
                    _l.CreateRoom();
                }
                if (_r != null)
                {
                    _r.CreateRoom();
                }
            }
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

    public void CreateBSP(SubDungeon SB)
    {
        if (SB.Split())
        {
            CreateBSP(SB.GetL());
            CreateBSP(SB.GetR());
        }
    }

    #region unityFuncs
    // Start is called before the first frame update
    void Start()
    {
        SubDungeon root = new SubDungeon(MaxWidthHeightRatio, new Rect(0, 0, DungeonWidth, DungeonHeight), MaxWidthHeightRatio, MinRoomSize, MaxRoomSize);
        CreateBSP(root);
        root.CreateRoom();
    }
    #endregion
    


}
