namespace DeepAction
{
    //thought "DeepDamage" sounded stupid
    public struct Damage
    {
        public int damage;
        public D_Resource target;

        public Damage(int damage, D_Resource target = D_Resource.Health)
        {
            this.damage = damage;
            this.target = target;
        }
    }
}
