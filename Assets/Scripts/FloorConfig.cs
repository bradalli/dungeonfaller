using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorConfig", menuName = "Level/Floor Configuration", order = 1)]
public class FloorConfig : ScriptableObject
{
    public enum ObjTypes { Pole, Board, Coin };

    [System.Serializable]
    public struct ObjInfo
    {
        public ObjTypes type;
        public Vector3 position;
        public Vector3 eulers;
        
    }

    public ObjInfo[] info;
}
