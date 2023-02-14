using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace webtv_partition_editor
{
    /// <summary>
    /// Interaction logic for BuildView.xaml
    /// </summary>
    public partial class BuildView : Window
    {
        public bool? OnlyInfo
        {
            get { return GetValue(OnlyInfoProperty) as bool?; }
            set { SetValue(OnlyInfoProperty, value); }
        }
        public static readonly DependencyProperty OnlyInfoProperty =
            DependencyProperty.Register("OnlyInfo",
                                        typeof(bool?),
                                        typeof(BuildView));

        public ObjectLocation SelectedObjectLocation
        {
            get { return GetValue(SelectedObjectLocationProperty) as ObjectLocation; }
            set { SetValue(SelectedObjectLocationProperty, value); }
        }
        public static readonly DependencyProperty SelectedObjectLocationProperty =
            DependencyProperty.Register("SelectedObjectLocation",
                                        typeof(ObjectLocation),
                                        typeof(BuildView));

        private void on_build_location_change(object sender, SelectionChangedEventArgs e)
        {
            var dc = this.DataContext as BuildViewModel;

            if(dc != null)
            {
                dc.set_build_info();
            }
        }

        public BuildView(WebTVPartition part, bool? only_info)
        {
            InitializeComponent();

            this.OnlyInfo = only_info;

            this.DataContext = new BuildViewModel(this, part, only_info);
        }
    }
}
