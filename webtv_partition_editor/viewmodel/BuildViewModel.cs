using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.IO;

using System.Threading;

namespace webtv_partition_editor
{
    class BuildViewModel
    {
        private delegate void void_call();

        public BuildView build_dialog { get; set; }
        public WebTVPartition part { get; set; }
        public ObjectLocationCollection build_locations;
        public string new_build_filename;
        public uint calculated_code_checksum;
        public uint calculated_romfs_checksum;

        private NoReturnProgressWindow progress_window;
        private Thread progress_thread;
        private long last_time_ms = 0;
        private long cumulative_time = 0;
        private long cululative_written_bytes = 0;
        private double bytes_per_min = 0.0;

        private RelayCommand _cancel_command;
        public ICommand cancel_command
        {
            get
            {
                if (_cancel_command == null)
                {
                    _cancel_command = new RelayCommand(x => on_cancel_click(), x => true);
                }

                return _cancel_command;
            }
        }

        private RelayCommand _load_build_image;
        public ICommand load_build_image
        {
            get
            {
                if (_load_build_image == null)
                {
                    _load_build_image = new RelayCommand(x => on_load_build_image(), x => true);
                }

                return _load_build_image;
            }
        }

        private RelayCommand _write_image_command;
        public ICommand write_image_command
        {
            get
            {
                if (_write_image_command == null)
                {
                    _write_image_command = new RelayCommand(x => on_write_image_command(), x => true);
                }

                return _write_image_command;
            }
        }

        public void close_window()
        {
            if (this.build_dialog.Dispatcher.CheckAccess() == false)
            {
                var cb = new void_call(this.close_window);

                this.build_dialog.Dispatcher.Invoke(cb);
            }
            else
            {
                if (this.progress_window != null)
                {
                    this.progress_window.Close();
                }

                this.build_dialog.Close();
            }
        }

        public void on_cancel_click()
        {
            close_window();
        }

        public void on_load_build_image()
        {
            System.Windows.Forms.OpenFileDialog file_dialog = new System.Windows.Forms.OpenFileDialog();
            file_dialog.Filter = "ROM Image Files (*.o, *.img, *.bin, *.ima)|*.o;*.img;*.bin;*.ima|All Files (*.*)|*.*";
            file_dialog.Title = "Select WebTV ROM Image";

            if (file_dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                new_build_filename = file_dialog.FileName;

                set_new_build_info();
            }
        }

        public void write_progress(int bytes_written, int bytes_written_since, int bytes_total)
        {
            if(this.progress_window != null)
            {
                float progress_percentage = ((float)bytes_written / (float)bytes_total) * 100;
                string progress_message = String.Format("{0:00.00}%", progress_percentage);

                long current_time_ms = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                if (this.last_time_ms > 0)
                {
                    long time_difference_ms = Math.Max(current_time_ms - this.last_time_ms, 1);

                    this.cumulative_time += time_difference_ms;
                }

                this.cululative_written_bytes += bytes_written_since;

                this.last_time_ms = current_time_ms;

                if (this.cumulative_time > 250)
                {
                    this.bytes_per_min = ((double)this.cululative_written_bytes / (double)this.cumulative_time) * (1000.0 * 60.0);

                    this.cumulative_time = 0;
                    this.cululative_written_bytes = 0;
                }

                if (this.bytes_per_min > 0)
                {
                    progress_message += String.Format(" [{0}/min]", BytesToString.bytes_to_iec((ulong)this.bytes_per_min));
                }

                this.progress_window.set_progress(progress_percentage, progress_message);
            }
        }

        public void writing_thread(WebTVBuildWriter.block_written progress_callback, bool new_use_calculated_code_checksum, bool new_use_calculated_romfs_checksum)
        {
            try
            {
                this.build_dialog.Dispatcher.Invoke((Action)(() =>
                {
                    this.last_time_ms = 0;

                    var build_writer = new WebTVBuildWriter(this.part, this.build_dialog.SelectedObjectLocation, new_build_filename);

                    if (new_use_calculated_code_checksum)
                    {
                        build_writer.set_code_checksum(this.calculated_code_checksum);
                    }

                    if (new_use_calculated_romfs_checksum)
                    {
                        build_writer.set_romfs_checksum(this.calculated_romfs_checksum);
                    }

                    build_writer.write_build(write_progress);
                }));
                    
            }
            catch (Exception e)
            {
                MessageBox.Show("Error! " + e.Message);
            }

            close_window();
        }

        public void progress_window_loaded(Object sender, RoutedEventArgs e)
        {
            var progress_window = sender as NoReturnProgressWindow;

            if (progress_window != null)
            {
                var new_use_calculated_code_checksum = (bool)this.build_dialog.new_build_use_calculated_code_checksum.IsChecked;
                var new_use_calculated_romfs_checksum = (bool)this.build_dialog.new_build_use_calculated_code_checksum.IsChecked;

                this.progress_thread = new Thread(() => this.writing_thread(this.write_progress, new_use_calculated_code_checksum, new_use_calculated_romfs_checksum));
                this.progress_thread.Start();
            }
        }

        public void progress_window_unloaded(Object sender, RoutedEventArgs e)
        {
            if (this.progress_thread != null)
            {
                this.progress_thread.Abort();

                this.set_button_visibility(Visibility.Visible);

                this.set_build_info();
            }
        }

        public void set_button_visibility(Visibility visibility)
        {
            this.build_dialog.cancel_button.Visibility = visibility;
            this.build_dialog.submit_button.Visibility = visibility;
        }

        public void on_write_image_command()
        {
            if (this.new_build_filename != null && this.new_build_filename != "")
            {
                if (this.build_dialog.SelectedObjectLocation != null)
                {
                    var build_file_info = new FileInfo(this.new_build_filename);

                    if (build_file_info.Length <= (uint)this.build_dialog.SelectedObjectLocation.size_bytes)
                    {
                        this.set_button_visibility(Visibility.Hidden);

                        this.progress_window = new NoReturnProgressWindow(0);
                        this.progress_window.Loaded += progress_window_loaded;
                        this.progress_window.Unloaded += progress_window_unloaded;
                        this.progress_window.Owner = this.build_dialog;
                        this.progress_window.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("The build image file goes beyond the build location boundry!  Try to find a smaller file.");
                    }
                }
                else
                {
                    MessageBox.Show("You must choose a valid build image location!");
                }
            }
            else
            {
                MessageBox.Show("You must choose a build image file!");
            }
        }

        public void reset_build_page(string pane_prefix)
        {
            if (pane_prefix == "new")
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_file_path")).Text = "";
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_file_size")).Text = "-";
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_file_size")).Foreground = Brushes.Black;
                ((CheckBox)this.build_dialog.FindName(pane_prefix + "_build_use_calculated_code_checksum")).IsChecked = true;
                ((CheckBox)this.build_dialog.FindName(pane_prefix + "_build_use_calculated_romfs_checksum")).IsChecked = true;
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_collation")).Text = "-";
            }

            if (pane_prefix == "info")
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_file_size")).Text = "-";
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_file_size")).Foreground = Brushes.Black;
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_collation")).Text = "-";
            }

            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_number")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_flags")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_checksum")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_checksum")).Foreground = Brushes.Black;
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_length")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_length")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_jump_offset")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_base")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_base")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_checksum")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_checksum")).Foreground = Brushes.Black;
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_calculated_romfs_checksum")).Text = "-";
            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_size")).Text = "-";
        }

        public void style_build_pane(WebTVBuildInfo build, string pane_prefix, string filename = "", ulong file_size_bytes = 0, ulong file_offset_bytes = 0, DiskByteTransform collation = DiskByteTransform.NOSWAP)
        {
            reset_build_page(pane_prefix);

            var build_header = build.get_build_info();
            this.calculated_romfs_checksum = 0;
            this.calculated_code_checksum = 0;

            if (pane_prefix == "new" || (pane_prefix == "info" && filename != ""))
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_file_path")).Text = filename;

                ((TextBox)this.build_dialog.FindName(pane_prefix + "_file_size")).Text = BytesToString.bytes_to_iec(file_size_bytes);

                var build_location = this.build_dialog.partition_build_locations.SelectedItem as ObjectLocation;

                if (pane_prefix == "new" && build_location != null && file_size_bytes > build_location.size_bytes)
                {
                    ((TextBox)this.build_dialog.FindName(pane_prefix + "_file_size")).Foreground = Brushes.Red;
                }

                var collation_description = "";
                switch (collation)
                {
                    case DiskByteTransform.BIT16SWAP:
                        collation_description = "16-bit swapped";
                        break;

                    case DiskByteTransform.BIT1632SWAP:
                        collation_description = "16+32-bit swapped";
                        break;

                    case DiskByteTransform.BIT32SWAP:
                        collation_description = "32-bit swapped";
                        break;

                    case DiskByteTransform.NOSWAP:
                    default:
                        collation_description = "no swapping";
                        break;
                }

                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_collation")).Text = collation_description;
            }

            var build_flags = new List<string>();

            if ((build_header.build_flags & 0x04) != 0)
            {
                build_flags.Add("Debug");
            }
            if ((build_header.build_flags & 0x20) != 0)
            {
                build_flags.Add("Satellite?");
            }
            if ((build_header.build_flags & 0x10) != 0)
            {
                build_flags.Add("Windows CE?");
            }
            if ((build_header.build_flags & 0x01) != 0)
            {
                build_flags.Add("Compressed Heap Data");
            }
            else
            {
                build_flags.Add("Raw Heap Data");
            }

            if (build_flags.Count > 0)
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_flags")).Text = String.Join(", ", build_flags);
            }
            else
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_flags")).Text = "-";
            }

            if (!build_header.is_classic_build && build_header.build_number > 0 && build_header.dword_code_length > 0 && build_header.jump_offset > 4)
            {
                this.calculated_code_checksum = build.calculate_code_checksum();
            }
            else if (pane_prefix == "new")
            {
                ((CheckBox)this.build_dialog.FindName(pane_prefix + "_build_use_calculated_code_checksum")).IsChecked = false;
                ((CheckBox)this.build_dialog.FindName(pane_prefix + "_build_use_calculated_romfs_checksum")).IsChecked = false;
            }

            if (build_header.build_number == 0)
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_number")).Text = "?";
            }
            else
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_number")).Text = build_header.build_number.ToString();
            }

            if (build_header.is_classic_build)
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_checksum")).Text = "Classic build?";
            }
            else
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_checksum")).Text = build_header.code_checksum.ToString("X");

                if (build_header.jump_offset > 4 && this.calculated_code_checksum != build_header.code_checksum)
                {
                    ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_checksum")).Foreground = Brushes.Red;
                }
            }

            if (build_header.jump_offset == 4)
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_calculated_code_checksum")).Text = "Compressed build?";
            }
            else
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_calculated_code_checksum")).Text = this.calculated_code_checksum.ToString("X");
            }


            if (build_header.is_classic_build || build_header.dword_length == 0)
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_length")).Text = "?";
            }
            else
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_length")).Text = this.get_size_string(build_header.dword_length);
            }

            if (build_header.is_classic_build || build_header.dword_code_length == 0)
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_length")).Text = "?";
            }
            else
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_length")).Text = this.get_size_string(build_header.dword_code_length);
            }

            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_jump_offset")).Text = this.get_offset_string((build_header.build_base_address + build_header.jump_offset), build_header.build_base_address, file_offset_bytes);

            ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_base")).Text = this.get_offset_string(build_header.build_base_address, build_header.build_base_address, file_offset_bytes);

            if (build_header.romfs_base_address == 0)
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_base")).Text = "No ROMFS";

                if (pane_prefix == "new")
                {
                    ((CheckBox)this.build_dialog.FindName(pane_prefix + "_build_use_calculated_romfs_checksum")).IsChecked = false;
                }
            }
            else if(!build_header.is_classic_build)
            {
                this.calculated_romfs_checksum = build.calculate_romfs_checksum();

                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_base")).Text = this.get_offset_string(build_header.romfs_base_address, build_header.build_base_address, file_offset_bytes);

                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_checksum")).Text = build_header.romfs_checksum.ToString("X");
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_calculated_romfs_checksum")).Text = this.calculated_romfs_checksum.ToString("X");
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_size")).Text = this.get_size_string(build_header.dword_romfs_size);

                if (this.calculated_romfs_checksum != build_header.romfs_checksum)
                {
                    ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_romfs_checksum")).Foreground = Brushes.Red;
                }
            }

            if (!build_header.is_classic_build)
            {
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_heap_size")).Text = this.get_size_string(build_header.dword_heap_data_size + build_header.dword_heap_free_size);
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_heap_data_offset")).Text = this.get_offset_string(build_header.heap_data_address, build_header.build_base_address, file_offset_bytes);
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_heap_data_compressed_size")).Text = this.get_size_string(build_header.dword_heap_compressed_data_size);
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_heap_data_size")).Text = this.get_size_string(build_header.dword_heap_data_size);

                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_shrunk_offset")).Text = this.get_offset_string(build_header.code_compressed_address, build_header.build_base_address, file_offset_bytes);
                ((TextBox)this.build_dialog.FindName(pane_prefix + "_build_code_shrunk_size")).Text = this.get_size_string(build_header.dword_code_compressed_size);
            }
        }

        public string get_offset_string(ulong offset, ulong build_base, ulong stream_offset)
        {
            var offset_string = offset.ToString("X");

            if (offset >= build_base)
            {
                offset_string += " {" + (stream_offset + (offset - build_base)).ToString("X") + "}";
            }

            return offset_string;
        }

        public string get_size_string(uint dword_size)
        { 
            var size = dword_size * WORD.DWORD_SIZE_BYTES;

            return BytesToString.bytes_to_iec(size) + " (" + dword_size.ToString("X") + ")";
        }

        public void set_build_info()
        {
            this.set_current_build_info();
            this.set_new_build_info();
        }

        public void set_new_build_info()
        {
            if (this.new_build_filename != null && this.new_build_filename != "")
            {
                try
                {
                    string pane_prefix = "new";

                    var build = new WebTVBuildInfo(this.new_build_filename);

                    if ((bool)this.build_dialog.OnlyInfo)
                    {
                        pane_prefix = "info";
                    }

                    style_build_pane(build, pane_prefix, this.new_build_filename, (ulong)build.io.Length, 0, build.byte_converter.byte_transform);

                    build.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error! " + e.Message);
                }
            }
        }

        public void set_current_build_info()
        {
            this.build_dialog.SelectedObjectLocation = this.build_dialog.partition_build_locations.SelectedItem as ObjectLocation;

            if(this.part != null)
            {
                string pane_prefix = "current";

                var build = new WebTVBuildInfo(this.part, this.build_dialog.SelectedObjectLocation);

                if ((bool)this.build_dialog.OnlyInfo)
                {
                    pane_prefix = "info";
                }

                style_build_pane(build, pane_prefix, "", this.build_dialog.SelectedObjectLocation.size_bytes, this.build_dialog.SelectedObjectLocation.offset);

                build.Close();
            }
        }

        public void set_build_locations()
        {
            this.build_locations = new ObjectLocationCollection();

            if (this.part != null)
            {
                this.build_locations.add_enumerated_build(this.part);
            }

            if ((bool)this.build_dialog.OnlyInfo)
            {
                this.build_locations.Add(new ObjectLocation()
                {
                    type = ObjectLocationType.FILE_LOCATION,
                    offset = 0,
                    size_bytes = 0,
                });
            }

            this.build_dialog.partition_build_locations.ItemsSource = this.build_locations;
        }

        public void style_partition_info()
        {
            if (this.part != null)
            {
                this.build_dialog.partition_name.Content = this.part.name;
                this.build_dialog.partition_size.Content = (new PartitionSizeConverter()).Convert(this.part, null, null, null).ToString();
            }
        }

        public BuildViewModel(BuildView build_dialog, WebTVPartition part, bool? only_info)
        {
            this.build_dialog = build_dialog;
            this.part = part;

            this.build_dialog.OnlyInfo = only_info;

            style_partition_info();

            set_build_locations();
            set_current_build_info();
        }
    }
}
