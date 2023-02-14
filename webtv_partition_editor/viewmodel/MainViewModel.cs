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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Aga.Controls.Tree;
using System.IO;
using LTR.IO.ImDisk;
using System.Diagnostics;

namespace webtv_partition_editor
{
    class MainViewModel : INotifyPropertyChanged
    {
        private delegate void void_callback();

        public DiskView disk_view { get; set; }
        public WebTVDiskCollection webtv_disks { get; set; }
        public Window main_window { get; set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public string check
        {
            get
            {
                return "true";
            }
        }

        // Raise PropertyChanged event
        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Ribbon button ICommand properties
        private RelayCommand _add_disk_image;
        public ICommand add_disk_image
        {
            get
            {
                if (_add_disk_image == null)
                {
                    _add_disk_image = new RelayCommand(x => on_add_disk_image(), x => true);
                }

                return _add_disk_image;
            }
        }

        private RelayCommand _add_physical_disk;
        public ICommand add_physical_disk
        {
            get
            {
                if (_add_physical_disk == null)
                {
                    _add_physical_disk = new RelayCommand(x => on_add_physical_disk(), x => true);
                }

                return _add_physical_disk;
            }
        }

        private RelayCommand _remove_disk;
        public ICommand remove_disk
        {
            get
            {
                if (_remove_disk == null)
                {
                    _remove_disk = new RelayCommand(x => on_remove_disk(), x => true);
                }

                return _remove_disk;
            }
        }

        private RelayCommand _initialize_disk;
        public ICommand initialize_disk
        {
            get
            {
                if (_initialize_disk == null)
                {
                    _initialize_disk = new RelayCommand(x => on_initialize_disk(), x => true);
                }

                return _initialize_disk;
            }
        }

        private RelayCommand _add_partition;
        public ICommand add_partition
        {
            get
            {
                if (_add_partition == null)
                {
                    _add_partition = new RelayCommand(x => on_add_partition(), x => true);
                }

                return _add_partition;
            }
        }

        private RelayCommand _consolidate_free;
        public ICommand consolidate_free
        {
            get
            {
                if (_consolidate_free == null)
                {
                    _consolidate_free = new RelayCommand(x => on_consolidate_free(), x => true);
                }

                return _consolidate_free;
            }
        }

        private RelayCommand _delete_partition;
        public ICommand delete_partition
        {
            get
            {
                if (_delete_partition == null)
                {
                    _delete_partition = new RelayCommand(x => on_delete_partition(), x => true);
                }

                return _delete_partition;
            }
        }

        private RelayCommand _replace_webtv_build;
        public ICommand replace_webtv_build
        {
            get
            {
                if (_replace_webtv_build == null)
                {
                    _replace_webtv_build = new RelayCommand(x => on_replace_webtv_build(), x => true);
                }

                return _replace_webtv_build;
            }
        }

        private RelayCommand _build_info;
        public ICommand build_info
        {
            get
            {
                if (_build_info == null)
                {
                    _build_info = new RelayCommand(x => on_build_info(), x => true);
                }

                return _build_info;
            }
        }

        private RelayCommand _edit_nvram;
        public ICommand edit_nvram
        {
            get
            {
                if (_edit_nvram == null)
                {
                    _edit_nvram = new RelayCommand(x => on_edit_nvram(), x => true);
                }

                return _edit_nvram;
            }
        }
        

        private RelayCommand _mount_partition;
        public ICommand mount_partition
        {
            get
            {
                if (_mount_partition == null)
                {
                    _mount_partition = new RelayCommand(x => on_mount_partition(), x => true);
                }

                return _mount_partition;
            }
        }
        
        
        private RelayCommand _unmount_partition;
        public ICommand unmount_partition
        {
            get
            {
                if (_unmount_partition == null)
                {
                    _unmount_partition = new RelayCommand(x => on_unmount_partition(), x => true);
                }

                return _unmount_partition;
            }
        }

        private RelayCommand _explore_partition;
        public ICommand explore_partition
        {
            get
            {
                if (_explore_partition == null)
                {
                    _explore_partition = new RelayCommand(x => on_explore_partition(), x => true);
                }

                return _explore_partition;
            }
        }

        private RelayCommand _copy_partition_data;
        public ICommand copy_partition_data
        {
            get
            {
                if (_copy_partition_data == null)
                {
                    _copy_partition_data = new RelayCommand(x => on_copy_partition_data((string)x), x => true);
                }

                return _copy_partition_data;
            }
        }

        private RelayCommand _xxx;
        public ICommand xxx
        {
            get
            {
                if (_xxx == null)
                {
                    _xxx = new RelayCommand(x => on_xxx(), x => true);
                }

                return _xxx;
            }
        }
        #endregion

        private RelayCommand _select_partition_command;
        public ICommand select_partition_command
        {
            get
            {
                if (_select_partition_command == null)
                {
                    _select_partition_command = new RelayCommand(x => select_partition(x), x => true);
                }

                return _select_partition_command;
            }
        }

        private RelayCommand _select_disk_command;
        public ICommand select_disk_command
        {
            get
            {
                if (_select_disk_command == null)
                {
                    _select_disk_command = new RelayCommand(x => select_disk(x), x => true);
                }

                return _select_disk_command;
            }
        }

        void clear_selection()
        {
            this.disk_view.SelectedDisk = null;
            this.disk_view.SelectedPartition = null;

            var tree = this.disk_view.disk_collection_tree;

            if (tree != null)
            {
                tree.SelectedItem = null;
            }
        }

        void select_partition(object selected_object)
        {
            var part = selected_object as WebTVPartition;

            if (part != null)
            {
                this.disk_view.SelectedDisk = null;
                this.disk_view.SelectedPartition = part;

                this.set_tree_selection_by_id(part.id);
            }
        }

        void select_disk(object selected_object)
        {
            var disk = selected_object as WebTVDisk;

            if (disk != null)
            {
                this.disk_view.SelectedDisk = disk;
                this.disk_view.SelectedPartition = null;

                this.set_tree_selection_by_id(disk.id);
            }
        }

        #region Ribbon button callbacks
        public void on_add_disk_image()
        {
            OpenFileDialog file_dialog = new OpenFileDialog();
            file_dialog.Filter = "Raw Disk Image Files (*.img, *.bin, *.ima)|*.img;*.bin;*.ima|All Files (*.*)|*.*";
            file_dialog.Title = "Select WebTV Disk Image";


            if (file_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var existing_disk = this.webtv_disks.get_disk_by_path(file_dialog.FileName);

                    if (existing_disk != null)
                    {
                        this.select_disk(existing_disk);
                    }
                    else
                    {
                        this.webtv_disks.add_disk_image(file_dialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error adding disk! " + ex.Message);
                }
            }
        }

        public void on_add_physical_disk()
        {
            var add_disk_dialog = new AddDisk(this.webtv_disks);

            add_disk_dialog.Owner = this.main_window;

            add_disk_dialog.ShowDialog();
        }

        public void on_remove_disk()
        {
            var disk = get_selected_disk();

            if (disk != null)
            {
                this.webtv_disks.remove_disk(disk);

                this.clear_selection();
            }
        }

        public void on_initialize_disk()
        {
            var disk = get_selected_disk();

            if (disk != null)
            {
                if (disk.size_bytes >= WebTVPartitionManager.MINIMUM_DISK_SIZE)
                {
                    var initilize_disk_dialog = new DiskInitialize(disk);

                    initilize_disk_dialog.Owner = this.main_window;

                    initilize_disk_dialog.ShowDialog();

                    reset_disk_partitions(disk, true);
                }
                else
                {
                    System.Windows.MessageBox.Show("The disk is too small to be initilized.  It must be at least " + BytesToString.bytes_to_iec(WebTVPartitionManager.MINIMUM_DISK_SIZE) + ".");
                }
            }
        }

        public void reset_disk_partitions(WebTVDisk disk, bool update_disk_state = false)
        {
            if (update_disk_state)
            {
                disk.update_disk_state();

                this.disk_view.ItemsSource = null;
                this.disk_view.ItemsSource = this.webtv_disks;
            }
            else
            {
                disk.enumerate_partitions();
            }

            this.clear_selection();
            this.refresh_tree();
        }

        public void on_add_partition()
        {
            var disk = get_selected_disk();

            if (disk != null)
            {
                var part = this.disk_view.SelectedPartition;

                var add_partition_dialog = new AddPartition(disk, part);

                add_partition_dialog.Owner = this.main_window;

                add_partition_dialog.ShowDialog();

                reset_disk_partitions(disk);
            }
        }

        public void on_consolidate_free()
        {
            var disk = get_selected_disk();

            if (disk != null)
            {
                var partition_manager = new WebTVPartitionManager(disk);

                partition_manager.consolidate_free_partitions();

                reset_disk_partitions(disk);
            }
        }

        public void on_delete_partition()
        {
            if (this.disk_view.SelectedPartition != null)
            {
                var part = this.disk_view.SelectedPartition;

                if(part != null)
                {
                    if(part.sector_start == 0)
                    {
                        System.Windows.MessageBox.Show("You cannot delete the first partition. The first partition is used for permanently addressed data.");
                    }
                    else
                    {
                        var dialog_result = System.Windows.MessageBox.Show("Are you sure you want to delete the '" + part.name + "' partition?  There is no undo button!",
                                                                           "Don't run with scissors",
                                                                           System.Windows.MessageBoxButton.YesNo);

                        if (dialog_result == MessageBoxResult.Yes)
                        {
                            this.do_unmount_partition(part);

                            var partition_manager = new WebTVPartitionManager(part.disk);

                            partition_manager.delete_partition(part);

                            reset_disk_partitions(part.disk);
                        }
                    }
                }
            }
        }

        public void on_xxx()
        {

        }

        public void on_replace_webtv_build()
        {
            if (this.disk_view.SelectedPartition != null
            && (this.disk_view.SelectedPartition.type == PartitionType.BOOT
            || (this.disk_view.SelectedPartition.disk.layout == DiskLayout.WEBSTAR && this.disk_view.SelectedPartition.type == PartitionType.BOOT2)))
            {
                var build_dialog = new BuildView(this.disk_view.SelectedPartition, false);

                build_dialog.Owner = this.main_window;

                build_dialog.ShowDialog();
            }
        }

        public void on_edit_nvram()
        {

            var nvram_location = new ObjectLocation()
            {
                offset = 0x200,
                size_bytes = 04000
            };


            var nvtest = new NVRAM(this.disk_view.SelectedDisk.io, nvram_location);

            //var webtv_hd_nvram = nvtest.unserialize_nvram();

            Console.Write("Done!");
        }

        public void on_build_info()
        {
            var build_dialog = new BuildView(this.disk_view.SelectedPartition, true);

            build_dialog.Owner = this.main_window;

            build_dialog.ShowDialog();
        }

        public bool can_mount_partitions()
        {
            var imdisk_info = new ImDiskInfo();

            return imdisk_info.IsInstalled();
        }
        
        public void on_mount_partition()
        {
            if (this.disk_view.SelectedPartition != null
             && (this.disk_view.SelectedPartition.type == PartitionType.FAT16 || this.disk_view.SelectedPartition.type == PartitionType.FAT16_DVR)
             && !this.disk_view.SelectedPartition.has_device_attached())
            {
                if (this.can_mount_partitions())
                {
                    var mount_dialog = new MountPartition(this.disk_view.SelectedPartition);

                    mount_dialog.Owner = this.main_window;

                    mount_dialog.ShowDialog();
                }
                else
                {
                    var dialog_result = System.Windows.MessageBox.Show("ImDisk Virtual Disk Driver must be installed in order to mount a WebTV partition.  Do you want to go to the ImDisk website to download the ImDisk install package?",
                                                                       "Mount Error",
                                                                       System.Windows.MessageBoxButton.YesNo);

                    if (dialog_result == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process.Start(ImDiskInfo.HUMAN_DOWNLOAD_URL);
                    }
                }
            }
        }

        public void do_unmount_partition(WebTVPartition part)
        {
            if (part != null 
             && part.has_device_attached())
            {
                var wait_window = new WaitMessage("Unmounting Device...");
                wait_window.Owner = this.main_window;
                wait_window.ShowDialog(new Action(() =>
                {
                    try
                    {
                        part.unmount();
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Error unmounting disk! " + ex.Message);
                    }
                }));
            }
        }

        public void on_unmount_partition()
        {
            if (this.disk_view.SelectedPartition != null
             && (this.disk_view.SelectedPartition.type == PartitionType.FAT16 || this.disk_view.SelectedPartition.type == PartitionType.FAT16_DVR))
            {
                this.do_unmount_partition(this.disk_view.SelectedPartition);
            }
        }

        public void on_explore_partition()
        {
            if (this.disk_view.SelectedPartition != null
             && (this.disk_view.SelectedPartition.type == PartitionType.FAT16 || this.disk_view.SelectedPartition.type == PartitionType.FAT16_DVR)
             && this.disk_view.SelectedPartition.has_device_attached())
            {
                try
                {
                    var drive_letter = this.disk_view.SelectedPartition.server.get_drive_letter();

                    if (drive_letter != "")
                    {
                        var explorer_start = new ProcessStartInfo()
                        {
                            UseShellExecute = true,
                            Verb = "open",
                            FileName = drive_letter + @":\",
                            WorkingDirectory = drive_letter + @":\",
                            WindowStyle = ProcessWindowStyle.Normal
                        };

                        Process.Start(explorer_start);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Error trying to explore disk! " + e.Message);
                }
            }
        }

        public void on_copy_partition_data(string copy_what)
        {
            var copy_string = "";

            var disk = this.get_selected_disk();
            if(disk != null)
            {
                ulong partition_start_offset = 0;
                ulong partition_end_offset = 0;

                if (this.disk_view.SelectedPartition != null)
                {
                    partition_start_offset = this.disk_view.SelectedPartition.sector_start * this.disk_view.SelectedPartition.disk.sector_bytes_length;
                    partition_end_offset = (this.disk_view.SelectedPartition.sector_start + this.disk_view.SelectedPartition.sector_length) * this.disk_view.SelectedPartition.disk.sector_bytes_length;
                }

                switch (copy_what)
                {
                    case "start-address":
                        copy_string = partition_start_offset.ToString("X");
                        break;

                    case "data-start-address":
                        partition_start_offset += WebTVPartitionManager.PARTITON_DATA_OFFSET;

                        copy_string = partition_start_offset.ToString("X");
                        break;

                    case "end-address":
                        copy_string = partition_end_offset.ToString("X");
                        break;

                    case "table-address":
                        copy_string = WebTVPartitionManager.get_partition_table_offset(disk).ToString("X");
                        break;
                }
            }

            if (copy_string != "")
            {
                try
                {
                    System.Windows.Clipboard.SetData(System.Windows.DataFormats.Text, (Object)copy_string);
                }
                catch { }
            }
        }
        #endregion

        public WebTVDisk get_selected_disk()
        {
            if (this.disk_view.SelectedPartition != null || this.disk_view.SelectedDisk != null)
            {
                var disk = this.disk_view.SelectedDisk;
                var part = this.disk_view.SelectedPartition;

                if (disk == null)
                {
                    disk = part.disk;
                }

                return disk;
            }

            return null;
        }

        private void tree_selection_change(object sender, SelectionChangedEventArgs e)
        {
            var item = get_selected_tree_item();

            if (item != null)
            { 
                if(item.object_type == TreeListViewDatum.ItemObjectType.DISK)
                {
                    var disk = this.webtv_disks.get_disk_by_id(item.id);

                    this.select_disk(disk);

                    this.scroll_to_visual_item_by_id(disk.id);
                }
                else if(item.object_type == TreeListViewDatum.ItemObjectType.PARTITION)
                {
                    var part = this.webtv_disks.get_partition_by_id(item.id);

                    if (part != null)
                    {
                        this.select_partition(part);

                        this.scroll_to_visual_item_by_id(part.id);
                    }
                }
            }
        }

        public TreeListViewDatum get_selected_tree_item()
        {
            var tree = this.disk_view.disk_collection_tree;

            if (tree != null)
            {
                var list_items = tree.Items;

                for (int i = 0; i < list_items.Count; i++)
                {
                    var node = list_items[i] as Aga.Controls.Tree.TreeNode;

                    if (node != null)
                    {
                        var item = node.Tag as TreeListViewDatum;
                        var _item = tree.ItemContainerGenerator.ContainerFromItem(node) as TreeListItem;

                        if (_item != null && _item.IsSelected)
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        public void set_tree_selection_by_id(Guid id)
        {
            var tree = this.disk_view.disk_collection_tree;

            if (tree != null)
            {
                var list_items = tree.Items;

                for (int i = 0; i < list_items.Count; i++)
                {
                    var node = list_items[i] as Aga.Controls.Tree.TreeNode;

                    if (node != null)
                    {
                        var item = node.Tag as TreeListViewDatum;

                        if (item.id == id)
                        {
                            tree.Focus();

                            var _item = tree.ItemContainerGenerator.ContainerFromItem(node) as TreeListItem;
                            if (_item != null)
                            {
                                _item.IsSelected = true;
                                _item.BringIntoView();
                                _item.Focus();
                            }

                            break;
                        }

                    }
                }
            }
        }

        public void refresh_tree()
        {
            if (this.disk_view.disk_collection_tree.Dispatcher.CheckAccess() == false)
            {
                var cb = new void_callback(refresh_tree);

                this.disk_view.Dispatcher.Invoke(cb, null);
            }
            else
            {
                this.disk_view.disk_collection_tree.RefreshList();
            }
        }

        public void scroll_to_visual_item_by_id(Guid id)
        {
            var scroller = this.find_element_by_id("disk_collection_visual_scroller", this.disk_view.disk_collection_visual) as ScrollViewer;
            if (scroller != null)
            {
                var scroll_index = this.find_scroll_index_by_id(scroller, id.ToString());

                if (scroll_index > -1)
                {
                    scroller.ScrollToVerticalOffset(scroll_index);
                }
            }
        }

        public void mounted_disks_changed(Object sender, EventArgs e)
        {
            this.disks_changed(sender, null);
        }

        public void disks_changed(Object sender, NotifyCollectionChangedEventArgs e)
        {
            this.refresh_tree();
        }

        public double find_scroll_index_by_id(ScrollViewer scroller, string id, bool ignore_if_visible = true, DependencyObject parent = null, double current_index = 0)
        {
            bool is_disk_stack_parent = false;

            if (parent == null)
            {
                parent = scroller;
            }

            if (parent != null)
            {
                var _parent = parent as UIElement;
                if (_parent.Uid == "disk_collection_visual_stack")
                {
                    is_disk_stack_parent = true;
                }

                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    var el = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                    if (el != null)
                    {
                        if (el.Uid != "")
                        {
                            if (el.Uid == id)
                            {
                                if (ignore_if_visible)
                                {
                                    var childTransform = el.TransformToAncestor(scroller);
                                    var rectangle = childTransform.TransformBounds(new Rect(new Point(0, 0), el.RenderSize));
                                    var result = Rect.Intersect(new Rect(new Point(0, 0), scroller.RenderSize), rectangle);
                                    
                                    if (result != Rect.Empty && result.Size.Height > 25)
                                    {
                                        return -1;
                                    }
                                }

                                return current_index;
                            }
                        }

                        var _current_index = this.find_scroll_index_by_id(scroller, id, ignore_if_visible, el, current_index);
                        if (_current_index > -1)
                        {
                            return _current_index;
                        }

                        if (is_disk_stack_parent)
                        {
                            current_index += el.ActualHeight;
                        }
                    }
                }
            }

            return -1;
        }

        public FrameworkElement find_element_by_id(string id, DependencyObject parent = null)
        {
            if (parent == null)
            {
                parent = this.disk_view;
            }

            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var el = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (el != null)
                {
                    if (el.Uid != "")
                    {
                        if (el.Uid == id)
                        {
                            return el;
                        }
                    }

                    el = this.find_element_by_id(id, el);

                    if (el != null)
                    {
                        return el;
                    }
                }
            }

            return null;
        }

        private void clenup_application(object sender, EventArgs e)
        {
            if (this.can_mount_partitions())
            {
                ImDiskAPI.DriveListChanged -= this.mounted_disks_changed;
            }

            if (this.webtv_disks != null)
            {
                var wait_window = new WaitMessage("Cleaning Up...");
                wait_window.Owner = this.main_window;
                wait_window.ShowDialog(new Action(() =>
                {
                    this.webtv_disks.clear_disks();

                    this.webtv_disks = null;
                }));
            }
        }

        public MainViewModel(Window main_window, DiskView window_disk_view)
        {
            /*
            * Check:
            *  this.disk_view != null
            *  this.disk_view.disk_collection_tree != null
            *  this.disk_view.disk_collection_visual != null
            */

            this.main_window = main_window;
            this.disk_view = window_disk_view;

            this.webtv_disks = new WebTVDiskCollection();

            this.disk_view.ItemsSource = this.webtv_disks;
            this.disk_view.SelectPartitionCommand = this.select_partition_command;
            this.disk_view.SelectDiskCommand = this.select_disk_command;
            this.webtv_disks.CollectionChanged += new NotifyCollectionChangedEventHandler(this.disks_changed);
            if (this.can_mount_partitions())
            {
                ImDiskAPI.DriveListChanged += this.mounted_disks_changed;
            }
            this.disk_view.disk_collection_tree.SelectionChanged += this.tree_selection_change;
            this.disk_view.disk_collection_tree.Model = new DiskCollectionTreeModel(this.webtv_disks);
            main_window.Closing += clenup_application;
            AppDomain.CurrentDomain.UnhandledException += clenup_application;
        }
    }
}
