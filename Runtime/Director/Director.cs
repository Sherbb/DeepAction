using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeepAction.Directors
{
    public class Director
    {
        public float credits;
        public float creditsPerSecond;

        public DirectorRule[] activeRules;

        public bool TrySpendCredits(int count)
        {
            if (credits >= count)
            {
                credits -= count;
                return true;
            }
            return false;
        }

        public void AddCredits(int count)
        {
            credits += count;
        }
    }

    public abstract class DirectorRule
    {
        //* Requirements to activate
        public virtual int creditsToActivate { get; protected set; } = 0;
        // Each director has an arbitrary "difficulty" level. It can vary over time.
        public virtual float difficultyToActivate { get; protected set; } = 0f;

        //* State
        public bool active = false;
        /// <summary>
        /// The credit activation cost is deposited in the bank. Credits will always be consumed from the bank before requesting more from the director.
        /// </summary>
        public int creditBank;

        public Director director
        {
            get { return director; }
            private set { director = value; }
        }

        protected virtual void OnActivate() { }
        protected virtual void OnDeactivate() { }

        public virtual bool TryToActivate()
        {
            return false;
        }

        /// <summary>
        /// Called every frame while on an active director
        /// </summary>
        public virtual void Tick() { }

        public void InitializeOnDirector(Director director)
        {
            this.director = director;
        }

        public void DeactivateRule()
        {

        }

        /// <summary>
        /// If a request for credits fails the rule will immedatly deactivate.
        /// </summary>
        private bool RequestCredits(int count)
        {
            if (director.TrySpendCredits(count))
            {
                creditBank += count;
                return true;
            }
            DeactivateRule();
            return false;
        }
    }

    //************ Example

    public class RuleExample : DirectorRule
    {
        public override int creditsToActivate => 5;
        public override float difficultyToActivate => 1f;

        private void Test()
        {
        }
    }
}
