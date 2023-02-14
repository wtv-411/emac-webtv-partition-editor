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
using Xceed.Wpf.Toolkit;

namespace webtv_partition_editor
{
    /// <summary>
    /// Interaction logic for AddPartition.xaml
    /// </summary>
    public partial class AddPartition : Window
    {
        private void on_partition_type_change(object sender, SelectionChangedEventArgs e)
        {
            var dc = this.DataContext as AddPartitionViewModel;

            if(dc != null)
            {
                dc.set_partition_size_values();
            }
        }

        public AddPartition(WebTVDisk disk, WebTVPartition part)
        {
            InitializeComponent();

            this.DataContext = new AddPartitionViewModel(this, disk, part);
        }
    }
}
