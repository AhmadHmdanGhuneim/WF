using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using WF.Functions;
using Xamarin.Forms;

namespace WF.Helpers
{
    public class ObservableObject : INotifyPropertyChanged
    {

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
        /// <param name="backingStore">Backing store.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="onChanged">On changed.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var changed = PropertyChanged;

            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //  public bool IsLtrLang => Settings.Culture.Name == "en-US";

        public bool IsLtrLang => GeneralFunctions.GetLanguage().Contains(GeneralFunctions.Language.en.ToString()); // .Culture.Name == "en-US";


        public LayoutOptions TextAligment => IsLtrLang ? LayoutOptions.Start : LayoutOptions.End;

        public int StartSpan => IsLtrLang ? 0 : 1;

        public int EndSpan => IsLtrLang ? 1 : 0;
                                                     /* is English */
        public TextAlignment HorizontalTextAligment => IsLtrLang ? TextAlignment.Start : TextAlignment.End;

        public GridLength StartGridLength => IsLtrLang ? GridLength.Auto : GridLength.Star;

        public GridLength EndGridLength => IsLtrLang ? GridLength.Star : GridLength.Auto;
    }
}
