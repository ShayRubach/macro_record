﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MacroRecorderGUI
{
    public partial class MainWindow : Window
    {
        private InjectAndCaptureDLL.iac_dll_capture_event_cb captureEventCbDelegate;
        public ObservableCollection<string> eventsObsColl = new ObservableCollection<string>();
        private void Iac_Dll_Capture_Event_Cb(string evt)
        {
            Dispatcher.Invoke(()=> eventsObsColl.Add(evt));
        }

        public MainWindow()
        {
            InitializeComponent();
            InjectAndCaptureDLL.iac_dll_init();
            listBox.ItemsSource = eventsObsColl;
            captureEventCbDelegate = Iac_Dll_Capture_Event_Cb;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StartRecord_Click(object sender, RoutedEventArgs e)
        {
            //(new Thread(() => {
                InjectAndCaptureDLL.iac_dll_start_capture(captureEventCbDelegate);
            //})).Start();
        }

        private void StopRecordButton_Click(object sender, RoutedEventArgs e)
        {
            InjectAndCaptureDLL.iac_dll_stop_capture();
        }

        private void InjectButton_Click(object sender, RoutedEventArgs e)
        {
            (new Thread(() =>
            {
                foreach (var evt in eventsObsColl)
                {
                    Thread.Sleep(5);
                    InjectAndCaptureDLL.iac_dll_inject_event(evt);
                }
            })).Start();
        }

        private void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            eventsObsColl.Add("{m:280,280,0,0,0,1,0}");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            eventsObsColl.Clear();
        }
    }
}
