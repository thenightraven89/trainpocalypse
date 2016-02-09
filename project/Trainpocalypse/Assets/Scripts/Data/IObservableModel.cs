using System;
using System.Reflection;

namespace Funk.Data
{
    public interface IObservableModel
    {
        event EventHandler<ModelChangedEventArgs> ModelChanged;

        void SubscribeToModelChanged(EventHandler<ModelChangedEventArgs> action);
    }

    public class ModelChangedEventArgs : EventArgs
    {
        public PropertyInfo ChangedProperty { get; private set; }

        public ModelChangedEventArgs(PropertyInfo changedProperty)
        {
            ChangedProperty = changedProperty;
        }
    }
}
