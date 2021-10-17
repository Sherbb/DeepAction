using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace DeepAction
{
    [HideReferenceObjectPicker]
    public class DeepState
    {
        [GUIColor("@GetColor()")]
        [DisplayAsString, ShowInInspector, ReadOnly, LabelText("Current Status:")]
        public bool state { get; private set; }

        [HideInInspector]
        public Action OnStateActivated;
        [HideInInspector]
        public Action OnStateDeactivated;
        [HideInInspector]
        public Action<bool> OnStateChanged;

        private List<DeepStateModifier> modifiers;
        private bool oldState;
        public DeepStateModifier AddModifier(DeepStateModifier modifier)
        {
            if (modifiers == null)
            {
                modifiers = new List<DeepStateModifier>();
            }
            modifiers.Add(modifier);
            UpdateValue();
            return modifier;
        }

        public bool RemoveModifier(DeepStateModifier modifier)
        {
            if (modifiers == null)
            {
                modifiers = new List<DeepStateModifier>();
                return false;
            }

            if (modifiers.Remove(modifier))
            {
                UpdateValue();
                return true;
            }
            return false;
        }

        public void UpdateValue()
        {
            oldState = state;
            if (modifiers.Count == 0)
            {
                state = false;
                if (state != oldState)
                {
                    OnStateDeactivated?.Invoke();
                    OnStateChanged?.Invoke(state);
                }
                return;
            }

            foreach (DeepStateModifier m in modifiers)
            {
                if (m.modType == DeepStateModifier.ModifyType.PreventActive)
                {
                    state = false;
                    if (state != oldState)
                    {
                        OnStateDeactivated?.Invoke();
                        OnStateChanged?.Invoke(state);
                    }
                    return;
                }
            }

            state = true;
            if (state != oldState)
            {
                OnStateActivated?.Invoke();
                OnStateChanged?.Invoke(state);
            }
            return;

        }

        private Color GetColor()//for inspector use only
        {
            if (state)
            {
                return new Color(.7f, 1f, .7f);
            }
            else
            {
                return new Color(1f, .7f, .7f);
            }
        }
    }

    [HideReferenceObjectPicker]
    public class DeepStateModifier
    {
        public enum ModifyType
        {
            Active,//turns state on
            PreventActive,//prevents state from turning on.
        }
        public ModifyType modType;
        public DeepStateModifier(DeepStateModifier other)
        {
            modType = other.modType;
        }
        public DeepStateModifier(ModifyType modType)
        {
            this.modType = modType;
        }
    }
}