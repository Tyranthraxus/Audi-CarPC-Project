using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CarPCSolutions.SystemTrayApplication.Forms
{
    public partial class ConfigurationPageForm : Form
    {
        //RegistryKey RNSMessageProcessorRegistryKey;
        //RegistryKey SynchSettingsRegKey;
 
        public ConfigurationPageForm()
        {
            InitializeComponent();

            // Load General settings
            tbFFRWDelay.Text = RegistrySettings.ButtonHoldDelay.ToString();
            tbDelayBetweenTVEnableMsgs.Text = RegistrySettings.DelayBetweenTVEnableMessages.ToString();
            tbAcceptanceCode.Text = RegistrySettings.CANUSBAcceptanceCode.ToString();
            tbAcceptanceMask.Text = RegistrySettings.CANUSBAcceptanceMask.ToString();
            tbMessageReaderDelay.Text = RegistrySettings.MessageProcessorInterval.ToString();
            cbAlwaysSendTVEnable.Checked = RegistrySettings.AlwaysSendTVEnable;

        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        
        private void SaveSettings()
        {
            // General settings.
            RegistrySettings.ButtonHoldDelay = Convert.ToInt32(tbFFRWDelay.Text);
            RegistrySettings.DelayBetweenTVEnableMessages = Convert.ToInt32(tbDelayBetweenTVEnableMsgs.Text);
            RegistrySettings.AlwaysSendTVEnable = cbAlwaysSendTVEnable.Checked;

            // Save settings for the CAN USB Interface.
            RegistrySettings.CANUSBAcceptanceCode = tbAcceptanceCode.Text;
            RegistrySettings.CANUSBAcceptanceMask = tbAcceptanceMask.Text;
            RegistrySettings.MessageProcessorInterval = Convert.ToInt32(tbMessageReaderDelay.Text);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        

        private void ConfigurationPageForm_Load(object sender, EventArgs e)
        {

        }
    }
}
