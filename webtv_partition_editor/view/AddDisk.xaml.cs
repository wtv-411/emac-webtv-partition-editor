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
    /// Interaction logic for AddDisk.xaml
    /// </summary>
    public partial class AddDisk : Window
    {
        public void on_add_disk_click(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                var vm = this.DataContext as AddDiskViewModel;

                vm.on_add_disk_click();
            }
        }

        public AddDisk(WebTVDiskCollection webtv_disks)
        {
            InitializeComponent();

            this.DataContext = new AddDiskViewModel(this, webtv_disks);
        }
    }
}
