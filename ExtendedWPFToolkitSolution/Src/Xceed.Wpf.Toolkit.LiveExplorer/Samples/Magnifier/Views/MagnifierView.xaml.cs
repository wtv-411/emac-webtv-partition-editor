/***************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2014 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ************************************************************************************/

using System;
using System.IO;
using System.Windows;
using System.Windows.Resources;

namespace Xceed.Wpf.Toolkit.LiveExplorer.Samples.Magnifier.Views
{
  /// <summary>
  /// Interaction logic for MagnifierView.xaml
  /// </summary>
  public partial class MagnifierView : DemoView
  {
    public MagnifierView()
    {
      InitializeComponent();

      // Load and display the RTF file.
      Uri uri = new Uri( "pack://application:,,,/Xceed.Wpf.Toolkit.LiveExplorer;component/Samples/Magnifier/Resources/SampleText.rtf" );
      StreamResourceInfo info = Application.GetResourceStream( uri );
      using( StreamReader txtReader = new StreamReader( info.Stream ) )
      {
        _txtContent.Text = txtReader.ReadToEnd();
      }
    }

  }
}
