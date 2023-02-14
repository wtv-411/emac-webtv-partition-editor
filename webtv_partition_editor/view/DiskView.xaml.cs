using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using System.Collections.Specialized;

namespace webtv_partition_editor
{
    /// <summary>
    /// Interaction logic for DiskView.xaml
    /// </summary>
    public partial class DiskView : UserControl
    {
        public WebTVDiskCollection ItemsSource
        {
            get { return GetValue(ItemsSourceProperty) as WebTVDiskCollection; }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource",
                                        typeof(WebTVDiskCollection),
                                        typeof(DiskView));

        public ICommand SelectDiskCommand
        {
            get { return GetValue(SelectDiskCommandProperty) as ICommand; }
            set { SetValue(SelectDiskCommandProperty, value); }
        }
        public static readonly DependencyProperty SelectDiskCommandProperty =
            DependencyProperty.Register("SelectDiskCommand",
                                        typeof(ICommand),
                                        typeof(DiskView));

        public WebTVDisk SelectedDisk
        {
            get { return GetValue(SelectedDiskProperty) as WebTVDisk; }
            set { SetValue(SelectedDiskProperty, value); }
        }
        public static readonly DependencyProperty SelectedDiskProperty =
            DependencyProperty.Register("SelectedDisk",
                                        typeof(WebTVDisk),
                                        typeof(DiskView));

        public ICommand SelectPartitionCommand
        {
            get { return GetValue(SelectPartitionCommandProperty) as ICommand; }
            set { SetValue(SelectPartitionCommandProperty, value); }
        }
        public static readonly DependencyProperty SelectPartitionCommandProperty =
            DependencyProperty.Register("SelectPartitionCommand",
                                        typeof(ICommand),
                                        typeof(DiskView));

        public WebTVPartition SelectedPartition
        {
            get { return GetValue(SelectedPartitionProperty) as WebTVPartition; }
            set { SetValue(SelectedPartitionProperty, value); }
        }
        public static readonly DependencyProperty SelectedPartitionProperty =
            DependencyProperty.Register("SelectedPartition",
                                        typeof(WebTVPartition),
                                        typeof(DiskView));

        public DiskView()
        {
            InitializeComponent();
        }
    }
}
