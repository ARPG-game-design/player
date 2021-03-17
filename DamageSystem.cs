using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public float dmg = 1.0f;
    public TargetType type = TargetType.Enemy;

    void OnTriggerEnter2D(Collider2D co)
    {
        switch ((int)type)
        {
            case 0:
                if (co.gameObject.layer == 10)
                {
                    co.GetComponent<Enemy>().Damage(dmg);
                }
                break;
            case 1:
                if (co.gameObject.tag == "Player")
                {
                    co.GetComponent<Battle>().Damage(dmg);
                }
                break;
            case 2:
                if (co.gameObject.layer == 10 || co.gameObject.layer == 9)
                {
                    co.GetComponent<Enemy>().Damage(dmg);
                }
                break;
        }
    }

    public enum TargetType {Enemy, Player, All};
}
