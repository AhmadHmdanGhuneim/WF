﻿using System;
using Xamarin.Forms;

namespace WF.Behaviors
{
    public class BehaviorBase<T> : Behavior<T>
        where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;

            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
    }
}