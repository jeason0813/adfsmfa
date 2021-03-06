﻿//******************************************************************************************************************************************************************************************//
// Copyright (c) 2017 Neos-Sdi (http://www.neos-sdi.com)                                                                                                                                    //                        
//                                                                                                                                                                                          //
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),                                       //
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,   //
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:                                                                                   //
//                                                                                                                                                                                          //
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.                                                           //
//                                                                                                                                                                                          //
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,                                      //
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,                            //
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                               //
//                                                                                                                                                                                          //
// https://adfsmfa.codeplex.com                                                                                                                                                             //
// https://github.com/neos-sdi/adfsmfa                                                                                                                                                      //
//                                                                                                                                                                                          //
//******************************************************************************************************************************************************************************************//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Neos.IdentityServer.MultiFactor.Administration;
using Neos.IdentityServer.MultiFactor;

namespace Neos.IdentityServer.Console
{
    public partial class UserCommonPropertiesControl : UserControl, IUserPropertiesDataObject
    {
        private UserPropertyPage userPropertyPage;
        private bool _syncdisabled = false;

        /// <summary>
        /// UserPropertiesControl Constructor
        /// </summary>
        public UserCommonPropertiesControl(UserPropertyPage parentPropertyPage)
        {
            InitializeComponent();
            userPropertyPage = parentPropertyPage;
        }

        /// <summary>
        /// Load event implmentation
        /// </summary>
        private void UserCommonPropertiesControl_Load(object sender, EventArgs e)
        {
            MethodSource.DataSource = new UsersPreferredMethodList(false);
        }


        #region IUserPropertiesDataObject
        /// <summary>
        /// SyncDisabled property implmentation
        /// </summary>
        public bool SyncDisabled
        {
            get { return _syncdisabled; }
            set { _syncdisabled = value; }
        }

        /// <summary>
        /// GetUserControlData method implmentation
        /// </summary>
        public MMCRegistrationList GetUserControlData(MMCRegistrationList lst)
        {
            foreach (MMCRegistration obj in lst)
            {
                ((MMCRegistration)obj).Enabled = this.cbEnabled.Checked;
                ((MMCRegistration)obj).PreferredMethod = (RegistrationPreferredMethod)((int)this.CBMethod.SelectedValue);
            }
            return lst;
        }

        /// <summary>
        /// SetUserControlData method implementation
        /// </summary>
        public void SetUserControlData(MMCRegistrationList lst, bool disablesync)
        {
            SyncDisabled = disablesync;
            try
            {
                bool isset = false;
                this.listUsers.Items.Clear();
                foreach (MMCRegistration obj in lst)
                {
                    this.listUsers.Items.Add(((MMCRegistration)obj).UPN);
                    if (!isset)
                    {
                        this.cbEnabled.Checked = ((MMCRegistration)obj).Enabled;
                        this.CBMethod.SelectedValue = (UsersPreferredMethod)(((MMCRegistration)obj).PreferredMethod);
                        isset = true;
                    }
                }
            }
            finally
            {
                SyncDisabled = false;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// cbEnabled_CheckedChanged event
        /// </summary>
        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!SyncDisabled)
                userPropertyPage.SyncSharedUserData(this, true);
        }

        /// <summary>
        /// CBMethod_SelectedIndexChanged event
        /// </summary>
        private void CBMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SyncDisabled)
                userPropertyPage.SyncSharedUserData(this, true);
        }

        /// <summary>
        /// BTNReinit_Click event
        /// </summary>
        private void BTNReinit_Click(object sender, EventArgs e)
        {
            MMCRegistrationList lst = userPropertyPage.GetSharedUserData();
            foreach (MMCRegistration reg in lst)
            {
                KeysManager.NewKey(reg.UPN);
            }
            if (!SyncDisabled)
                userPropertyPage.SyncSharedUserData(this, true);

        }

        /// <summary>
        /// BTNSendByMail_Click event
        /// </summary>
        private void BTNSendByMail_Click(object sender, EventArgs e)
        {
            Cursor crs = this.Cursor;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                MMCRegistrationList lst = userPropertyPage.GetSharedUserData();
                foreach (MMCRegistration reg in lst)
                {
                    string secret = KeysManager.EncodedKey(reg.UPN);
                    ManagementAdminService.SendKeyByEmail(reg.MailAddress, reg.UPN, secret);
                }
            }
            finally
            {
                this.Cursor = crs;
            }
        }
        #endregion
    }
}
