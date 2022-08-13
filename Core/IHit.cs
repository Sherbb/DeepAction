namespace DeepAction
{
    //todo possibly remove this. don't think any non-deepEntities should be getting Hit()
    public interface IHit
    {
        void Hit(params Damage[] damage);
    }
}