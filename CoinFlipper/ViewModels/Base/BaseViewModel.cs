using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CoinFlipper
{
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event with lambda
        /// </summary>
        /// <param name="name"></param>
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            string propertyName = ((MemberExpression)property.Body).Member.Name;
            OnPropertyChanged(propertyName);
        }
    }
}
