using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MangGuoTv.Views
{
    class MGLongListSelector : LongListSelector
    {
        public MGLongListSelector()
        {
            DefaultStyleKey = typeof(MGLongListSelector);
            ItemRealized += OnItemRealized;
        }
        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register("IsLoading",typeof(bool),typeof(MGLongListSelector),new PropertyMetadata(default(bool)));
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }
        private const int OffSet = 2;
        public event EventHandler DataRequest;
        protected virtual void OnDataRequest()
        {
            EventHandler handler = DataRequest;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        private void OnItemRealized(object sender, ItemRealizationEventArgs e)
        {
            if (!IsLoading && ItemsSource != null && ItemsSource.Count >= OffSet)
            {
                if (e.ItemKind == LongListSelectorItemKind.Item)
                {
                    var offSetItem = ItemsSource[ItemsSource.Count - OffSet];
                    if (e.Container.Content == offSetItem)
                    {
                        OnDataRequest();
                    }
                }
            }
        }
    }
}
