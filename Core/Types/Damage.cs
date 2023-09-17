using UnityEngine;

namespace DeepAction
{
    public struct Damage
    {
        public int damage;
        public Color color;
        public D_Resource target;

        public Damage(int damage, D_Resource target = D_Resource.Health)
        {
            this.damage = damage;
            this.target = target;
            this.color = Color.red;
        }

        public Damage(int damage, Color color, D_Resource target = D_Resource.Health)
        {
            this.damage = damage;
            this.target = target;
            this.color = color;
        }
    }
}
