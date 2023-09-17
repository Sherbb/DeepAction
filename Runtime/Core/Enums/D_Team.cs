using System;
using System.Diagnostics;

namespace DeepAction
{
    /// <summary>
    /// Determines the "alignment" of the deepEntity. Use this when you want a behavior to only affect the player/enemies etc.
    /// </summary>

    //* NEW TEAM CHECKLIST
    //1 Add to D_Team
    //2 Add to D_TeamSelector, add to filters if relevant
    //3 Add conversion in D_TeamSelectorExtensions switch

    public enum D_Team
    {
        Player = 1 << 0,
        Enemy = 1 << 1,
        Neutral = 1 << 2,
        Artifact = 1 << 3,
    }

    /// <summary>
    /// A Flagged version of D_Team you can use to define groups of teams nicely. Check against with the HasTeam() method.
    /// </summary>
     
    [Flags]
    public enum D_TeamSelector
    {
        None = 0,
        All = ~0,

        PlayerOrEnemy = Player | Enemy,

        Player = 1 << 0,
        Enemy = 1 << 1,
        Neutral = 1 << 2,
        Artifact = 1 << 3,
    }

    public static class D_TeamSelectorExtensions
    {
        public static D_TeamSelector ConvertToSelector(this D_Team team)
        {
            switch (team)
            {
                case D_Team.Player:
                    return D_TeamSelector.Player;
                case D_Team.Enemy:
                    return D_TeamSelector.Enemy;
                case D_Team.Neutral:
                    return D_TeamSelector.Neutral;
                case D_Team.Artifact:
                    return D_TeamSelector.Artifact;
            }
            return D_TeamSelector.None;
        }

        public static bool HasTeam(this D_TeamSelector selector, D_Team team)
        {
            return selector.HasFlag(team.ConvertToSelector());
        }
    }
}
