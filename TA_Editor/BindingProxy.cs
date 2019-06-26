using System.Windows;

namespace TA_Editor
{
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get => (object)this.GetValue(DataProperty);
            set => this.SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object),
                                         typeof(BindingProxy));
    }
}
