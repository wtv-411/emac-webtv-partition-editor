/***************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2014 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  *************************************************************************************/

namespace Xceed.Wpf.Toolkit.LiveExplorer.Samples.Text.Views
{
  /// <summary>
  /// Interaction logic for MaskedTextboxView.xaml
  /// </summary>
  public partial class MaskedTextboxView : DemoView
  {
    public MaskedTextboxView()
    {
      InitializeComponent();
      _mask.TextChanged += new System.Windows.Controls.TextChangedEventHandler( _mask_TextChanged );
    }

    void _mask_TextChanged( object sender, System.Windows.Controls.TextChangedEventArgs e )
    {
      _maskedTextBox.Value = null;
    }
  }
}
