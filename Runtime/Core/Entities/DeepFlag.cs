using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using System;

namespace DeepAction
{
    // TODO: this is a bit of a stub.
    // flags are intended for things like "stunned" where they have a strict on/off state
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class DeepFlag
    {
#if ODIN_INSPECTOR
        [DisplayAsString, ShowInInspector, ReadOnly, LabelText("Current Status:")]
#endif
        public bool flag { get; private set; }

        [HideInInspector]
        public Action OnFlagActivated;
        [HideInInspector]
        public Action OnFlagDeactivated;
        [HideInInspector]
        public Action<bool> OnFlagChanged;

        private List<DeepFlagModifier> modifiers;
        private bool oldFlag;

        public DeepFlagModifier AddModifier(DeepFlagModifier modifier)
        {
            if (modifiers == null)
            {
                modifiers = new List<DeepFlagModifier>();
            }
            modifiers.Add(modifier);
            UpdateValue();
            return modifier;
        }

        public bool RemoveModifier(DeepFlagModifier modifier)
        {
            if (modifiers == null)
            {
                modifiers = new List<DeepFlagModifier>();
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
            oldFlag = flag;
            flag = modifiers.Count > 0;

            if (flag != oldFlag)
            {
                OnFlagChanged?.Invoke(flag);
                if (flag)
                {
                    OnFlagActivated?.Invoke();
                }
                else
                {
                    OnFlagDeactivated?.Invoke();
                }
            }
        }
    }

#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class DeepFlagModifier
    {
        public DeepBehavior source { get; private set; }

        public DeepFlagModifier(DeepBehavior source = null)
        {
            this.source = source;
        }
    }
}