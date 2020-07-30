using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FloorConfigurator : MonoBehaviour
{
    [SerializeField] FloorConfig target;
    [SerializeField] bool update;

    void OnValidate()
    {
        if (target && transform.childCount > 0 && update)
        {
            target.info = new FloorConfig.ObjInfo[transform.childCount];

            for(int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                target.info[i].position = child.position;
                target.info[i].eulers = child.eulerAngles;

                switch (child.tag)
                {
                    case "Obstacle":
                        target.info[i].type = FloorConfig.ObjTypes.Pole;
                        break;

                    case "Destructable":
                        target.info[i].type = FloorConfig.ObjTypes.Board;
                        break;

                    case "Coin":
                        target.info[i].type = FloorConfig.ObjTypes.Coin;
                        break;
                }
            }
        }

        update = false;
    }
}
