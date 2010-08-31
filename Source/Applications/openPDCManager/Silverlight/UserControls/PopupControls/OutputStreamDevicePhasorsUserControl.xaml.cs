﻿//******************************************************************************************************
//  OutputStreamDevicePhasorsUserControl.xaml.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/23/2010 - Mehulbhai P Thakkar
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using openPDCManager.Utilities;
using openPDCManager.ModalDialogs;
#if SILVERLIGHT
using openPDCManager.PhasorDataServiceProxy;
using System.ServiceModel;
using System.Windows.Media.Animation;
#else
using openPDCManager.Data.Entities;
using System.Windows.Media.Imaging;
#endif

namespace openPDCManager.UserControls.PopupControls
{
    public partial class OutputStreamDevicePhasorsUserControl : UserControl
    {
        #region [ Members ]

        public int m_sourceOutputStreamDeviceID;
        public string m_sourceOutputStreamDeviceAcronym;
        bool m_inEditMode = false;
        int m_outputStreamDevicePhasorID = 0;       

        #endregion

        #region [ Constructor ]

        public OutputStreamDevicePhasorsUserControl()
        {
            InitializeComponent();
            Initialize();
            Loaded += new RoutedEventHandler(OutputStreamDevicePhasors_Loaded);
#if !SILVERLIGHT
            ButtonSave.Content = new BitmapImage(new Uri(@"images/Save.png", UriKind.Relative));
            ButtonClear.Content = new BitmapImage(new Uri(@"images/Cancel.png", UriKind.Relative));
#endif
            ButtonClear.Click += new RoutedEventHandler(ButtonClear_Click);
            ButtonSave.Click += new RoutedEventHandler(ButtonSave_Click);            
            ListBoxOutputStreamDevicePhasorList.SelectionChanged += new SelectionChangedEventHandler(ListBoxOutputStreamDevicePhasorList_SelectionChanged);
        }

        #endregion        

        #region [ Controls Event Handlers ]

        void ListBoxOutputStreamDevicePhasorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxOutputStreamDevicePhasorList.SelectedIndex >= 0)
            {
                OutputStreamDevicePhasor selectedOutputStreamDevicePhasor = ListBoxOutputStreamDevicePhasorList.SelectedItem as OutputStreamDevicePhasor;
                GridOutputStreamDevicePhasorDetail.DataContext = selectedOutputStreamDevicePhasor;
                ComboboxPhase.SelectedItem = new KeyValuePair<string, string>(selectedOutputStreamDevicePhasor.Phase, selectedOutputStreamDevicePhasor.PhaseType);
                ComboboxType.SelectedItem = new KeyValuePair<string, string>(selectedOutputStreamDevicePhasor.Type, selectedOutputStreamDevicePhasor.PhasorType);
                m_inEditMode = true;
                m_outputStreamDevicePhasorID = selectedOutputStreamDevicePhasor.ID;
            }
        }

        void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
#if SILVERLIGHT
            Storyboard sb = new Storyboard();
            sb = Application.Current.Resources["ButtonPressAnimation"] as Storyboard;
            sb.Completed += new EventHandler(delegate(object obj, EventArgs es) { sb.Stop(); });
            Storyboard.SetTarget(sb, ButtonSaveTransform);
            sb.Begin();
#endif           

            if (IsValid())
            {
                OutputStreamDevicePhasor outputStreamDevicePhasor = new OutputStreamDevicePhasor();
                App app = (App)Application.Current;
                outputStreamDevicePhasor.NodeID = app.NodeValue;
                outputStreamDevicePhasor.OutputStreamDeviceID = m_sourceOutputStreamDeviceID;
                outputStreamDevicePhasor.Label = TextBoxLabel.Text.CleanText();
                outputStreamDevicePhasor.Type = ((KeyValuePair<string, string>)ComboboxType.SelectedItem).Key;
                outputStreamDevicePhasor.Phase = ((KeyValuePair<string, string>)ComboboxPhase.SelectedItem).Key;
                outputStreamDevicePhasor.LoadOrder = TextBoxLoadOrder.Text.ToInteger();
                outputStreamDevicePhasor.ScalingValue = TextBoxScalingValue.Text.ToInteger();

                if (m_inEditMode == true && m_outputStreamDevicePhasorID > 0)
                {
                    outputStreamDevicePhasor.ID = m_outputStreamDevicePhasorID;
                    SaveOutputStreamDevicePhasor(outputStreamDevicePhasor, false);
                }
                else
                    SaveOutputStreamDevicePhasor(outputStreamDevicePhasor, true);
            }
        }

        void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
#if SILVERLIGHT
            Storyboard sb = new Storyboard();
            sb = Application.Current.Resources["ButtonPressAnimation"] as Storyboard;
            sb.Completed += new EventHandler(delegate(object obj, EventArgs es) { sb.Stop(); });
            Storyboard.SetTarget(sb, ButtonClearTransform);
            sb.Begin();
#endif
            ClearForm();
        }

        #endregion
        
        #region [ Page Event Handlers ]

        void OutputStreamDevicePhasors_Loaded(object sender, RoutedEventArgs e)
        {   
            ComboboxPhase.Items.Add(new KeyValuePair<string, string>("+", "Positive"));
            ComboboxPhase.Items.Add(new KeyValuePair<string, string>("-", "Negative"));
            ComboboxPhase.Items.Add(new KeyValuePair<string, string>("A", "Phase A"));
            ComboboxPhase.Items.Add(new KeyValuePair<string, string>("B", "Phase B"));
            ComboboxPhase.Items.Add(new KeyValuePair<string, string>("C", "Phase C"));
            ComboboxPhase.SelectedIndex = 0;

            ComboboxType.Items.Add(new KeyValuePair<string, string>("V", "Voltage"));
            ComboboxType.Items.Add(new KeyValuePair<string, string>("I", "Current"));
            ComboboxType.SelectedIndex = 0;

            ClearForm();

            GetOutputStreamDevicePhasorList();
        }

        #endregion

        #region [ Methods ]

        bool IsValid()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(TextBoxLabel.Text.CleanText()))
            {
                isValid = false;
                SystemMessages sm = new SystemMessages(new Message() { UserMessage = "Invalid Phasor Label", SystemMessage = "Please provide valid Phasor Label.", UserMessageType = MessageType.Error },
                        ButtonType.OkOnly);
                sm.Closed += new EventHandler(delegate(object sender, EventArgs e)
                {
                    TextBoxLabel.Focus();
                });
#if !SILVERLIGHT
                sm.Owner = Window.GetWindow(this);
                sm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
#endif
                sm.ShowPopup();
                return isValid;
            }

            if (!TextBoxLoadOrder.Text.IsInteger())
            {
                isValid = false;
                SystemMessages sm = new SystemMessages(new Message() { UserMessage = "Invalid Load Order", SystemMessage = "Please provide valid integer value for Load Order.", UserMessageType = MessageType.Error },
                    ButtonType.OkOnly);
                sm.Closed += new EventHandler(delegate(object sender, EventArgs e)
                {
                    TextBoxLoadOrder.Text = "0";
                    TextBoxLoadOrder.Focus();
                });
#if !SILVERLIGHT
                sm.Owner = Window.GetWindow(this);
                sm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
#endif
                sm.ShowPopup();
                return isValid;
            }

            if (!TextBoxScalingValue.Text.IsInteger())
            {
                isValid = false;
                SystemMessages sm = new SystemMessages(new Message() { UserMessage = "Invalid Scaling Value", SystemMessage = "Please provide valid integer value for Scaling Value.", UserMessageType = MessageType.Error },
                    ButtonType.OkOnly);
                sm.Closed += new EventHandler(delegate(object sender, EventArgs e)
                {
                    TextBoxScalingValue.Text = "0";
                    TextBoxScalingValue.Focus();
                });
#if !SILVERLIGHT
                sm.Owner = Window.GetWindow(this);
                sm.WindowStartupLocation = WindowStartupLocation.CenterOwner;
#endif
                sm.ShowPopup();
                return isValid;
            }

            return isValid;
        }

        void ClearForm()
        {
            GridOutputStreamDevicePhasorDetail.DataContext = new OutputStreamDevicePhasor();
            if (ComboboxPhase.Items.Count > 0)
                ComboboxPhase.SelectedIndex = 0;
            if (ComboboxType.Items.Count > 0)
                ComboboxType.SelectedIndex = 0;
            m_inEditMode = false;
            m_outputStreamDevicePhasorID = 0;
            ListBoxOutputStreamDevicePhasorList.SelectedIndex = -1;
        }

        #endregion
    }
}
