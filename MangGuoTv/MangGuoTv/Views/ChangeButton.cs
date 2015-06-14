using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace MangGuoTv
{
    public class ChangeButton : Button
    {
        public ChangeButton()
        {
            DefaultStyleKey = typeof(ChangeButton);
        }
        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register("DefaultImage", typeof(ImageSource), typeof(ChangeButton), null);
        public ImageSource DefaultImage
        {
            get
            {
                return (ImageSource)base.GetValue(DefaultImageProperty);
            }
            set
            {
                base.SetValue(DefaultImageProperty,value);
            }
        }
        public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register("SeletedImage", typeof(ImageSource), typeof(ChangeButton), null);
        public ImageSource SeletedImage
        {
            get
            {
                return (ImageSource)base.GetValue(SelectedImageProperty);
            }
            set
            {
                base.SetValue(SelectedImageProperty, value);
            }
        }
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(ChangeButton), new PropertyMetadata(false, IsCheckedChanged));
        public bool IsChecked
        {
            get
            {
                return (bool)base.GetValue(IsCheckedProperty);
            }
            set
            {
                base.SetValue(IsCheckedProperty, value);
            }
        }
        private static void IsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetState((ChangeButton)d);
        }
        private static void SetState(ChangeButton headerButton)
        {
            VisualStateManager.GoToState(headerButton,headerButton.IsChecked?"Checked":"Unchecked",true);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>((s, e) =>
            {
                IsChecked = !IsChecked;
                e.Handled = true;
            });
            SetState(this);
        }
    }
}
