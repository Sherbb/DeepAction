namespace DeepAction
{
    /// <summary>
    /// A flag is a boolean that changes how an entity behaves. flags are disabled by default
    /// 
    /// What should / should not be a flag vs an attribute is up for debate, but roughly follow that
    /// discrete states like "silences" are FLAGS, where variable state like "slowed" are attributes 
    /// </summary>
    public enum D_Flag
    {
        Silenced,
    }
}