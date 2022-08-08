namespace DeepAction
{
    /// <summary>
    /// A state is a boolean that changes how an entity behaves. states are disabled by default
    ///
    /// ex: SILENCED would be a state that prevents abilities from triggering,
    /// but SLOW should be applied through an attribute mod
    /// </summary>
    public enum D_State
    {
        stunned,
        silenced,
    }
}