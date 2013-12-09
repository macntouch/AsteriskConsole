﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VisualAsterisk.Asterisk.Config.Configuration;

namespace VisualAsterisk.Main.Gui.Configuration
{
    public partial class OptionsConfigPage : DockPage
    {
        public OptionsConfigPage()
        {
            InitializeComponent();
            this.localExtenLengthComboBox.Items.Add(new LocalExtenLength(2));
            this.localExtenLengthComboBox.Items.Add(new LocalExtenLength(3));
            this.localExtenLengthComboBox.Items.Add(new LocalExtenLength(4));
            this.localExtenLengthComboBox.Items.Add(new LocalExtenLength(5));
            this.localExtenLengthComboBox.Items.Add(new LocalExtenLength(0));
            this.localExtenLengthComboBox.SelectionChangeCommitted += new EventHandler(localExtenLengthComboBox_SelectionChangeCommitted);
        }

        void localExtenLengthComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }

        public override VisualAsterisk.Asterisk.IAsteriskServer Server
        {
            get
            {
                return base.Server;
            }
            set
            {
                base.Server = value;
                selectedLocalExtenLength = new LocalExtenLength(configManager.GeneralUser.LocalExtenLength);
                this.localExtenLengthComboBox.DataBindings.Clear();
                this.localExtenLengthComboBox.DataBindings.Add(new Binding("SelectedItem", this, "SelectedLocalExtenLength"));
                this.optionsBindingSource.DataSource = configManager.Options;
                this.optionsBindingSource.ListChanged += new ListChangedEventHandler(optionsBindingSource_ListChanged);
            }
        }

        void optionsBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    break;
                case ListChangedType.ItemChanged:
                    this.buttonSubmit.Enabled = true;
                    this.buttonCancel.Enabled = true;
                    break;
                case ListChangedType.ItemDeleted:
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    break;
                case ListChangedType.Reset:
                    break;
                default:
                    break;
            }
        }

        private LocalExtenLength selectedLocalExtenLength;
        public LocalExtenLength SelectedLocalExtenLength
        {
            get { return selectedLocalExtenLength; }
            set 
            { 
                selectedLocalExtenLength = value;
                configManager.GeneralUser.LocalExtenLength = selectedLocalExtenLength.ExtenLength;
            }
        }

        public struct LocalExtenLength
        {
            public LocalExtenLength(int extenLength)
            {
                this.extenLength = extenLength;
            }
            private int extenLength;

            public int ExtenLength
            {
                get { return extenLength; }
                set { extenLength = value; }
            }

            public override string ToString()
            {
                if (extenLength == 0)
                {
                    return "Varying";
                }
                return extenLength.ToString() + " digits";
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            // currently only general User updated
            configManager.UpdateUser(configManager.GeneralUser);
            this.buttonSubmit.Enabled = false;
            this.buttonCancel.Enabled = false;
            // TODO: Extensions.conf
            // exten = 6200,1,agentlogin()
            // exten = 6201,1,agentcallbacklogin()
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            configManager.GeneralUser.CancelEdit();
            this.buttonSubmit.Enabled = true;
            this.buttonCancel.Enabled = false;
            // TODO: Extensions.conf
            // exten = 6200,1,agentlogin()
            // exten = 6201,1,agentcallbacklogin()
        }
    }
}
