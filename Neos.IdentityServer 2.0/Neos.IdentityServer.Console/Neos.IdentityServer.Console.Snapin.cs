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
using System.ComponentModel;
using System.Security.Permissions;
using Microsoft.ManagementConsole;
using Neos.IdentityServer.MultiFactor.Administration;
using System.Diagnostics;
using System.Drawing;
using Microsoft.ManagementConsole.Advanced;
using System.Windows.Forms;

namespace Neos.IdentityServer.Console
{
    /// <summary>
    /// Provides the main entry point for the creation of a snap-in. 
    /// </summary>
    [SnapInSettings("{9627F1F3-A6D2-4cf8-90A2-10F85A7A4EE7}", DisplayName = "MFA", Description = "You can use ADFS MFA to define and configure secure access to ADFS with second authentication factor like email, sms or TOTP application (Google Authenticator, Microsoft Authenticator).", Vendor="Neos-Sdi")]
    [SnapInAbout("Neos.IdentityServer.Console.NativeResources.dll", IconId = 100, VersionId=101, DescriptionId=102, DisplayNameId=103)]
    public class ADFSSnapIn : SnapIn
    {
        private ScopeNode ServiceNode;
        private ScopeNode ServiceGeneralNode;
        private ScopeNode ServiceSQLNode;
        private ScopeNode ServiceADDSNode;
        private ScopeNode ServiceSMTPNode;
        private ScopeNode ServiceSMSNode;
        private ScopeNode ServiceSecurityNode;
        private ScopeNode UsersNode;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ADFSSnapIn()
        {
            // Root Node
            this.RootNode = new RootScopeNode();
            FormViewDescription fvr = new FormViewDescription();
            fvr.DisplayName = "MFA Platform";
            fvr.ControlType = typeof(RootViewControl);
            fvr.ViewType = typeof(RootFormView);
            this.RootNode.ViewDescriptions.Add(fvr);
            this.RootNode.ViewDescriptions.DefaultIndex = 0;

            // Service Node
            this.ServiceNode = new ServiceScopeNode();
            FormViewDescription fvc = new FormViewDescription();
            fvc.DisplayName = "MFA Platform Service";
            fvc.ControlType = typeof(ServiceViewControl);
            fvc.ViewType = typeof(ServiceFormView);
            this.ServiceNode.ViewDescriptions.Add(fvc);
            this.ServiceNode.ViewDescriptions.DefaultIndex = 0;

            // General Scope
            this.ServiceGeneralNode = new ServiceGeneralScopeNode();
            FormViewDescription fvs = new FormViewDescription();
            fvs.DisplayName = "MFA Platform General Properties";
            fvs.ControlType = typeof(GeneralViewControl);
            fvs.ViewType = typeof(GeneralFormView);
            this.ServiceGeneralNode.ViewDescriptions.Add(fvs);
            this.ServiceGeneralNode.ViewDescriptions.DefaultIndex = 0;

            // ADDS Scope
            this.ServiceADDSNode = new ServiceADDSScopeNode();
            FormViewDescription fadds = new FormViewDescription();
            fadds.DisplayName = "MFA Platform Active Directory Properties";
            fadds.ControlType = typeof(ADDSViewControl);
            fadds.ViewType = typeof(ServiceADDSFormView);
            this.ServiceADDSNode.ViewDescriptions.Add(fadds);
            this.ServiceADDSNode.ViewDescriptions.DefaultIndex = 0;

            // SQL Scope
            this.ServiceSQLNode = new ServiceSQLScopeNode();
            FormViewDescription fsql = new FormViewDescription();
            fsql.DisplayName = "MFA Platform SQL Server Properties";
            fsql.ControlType = typeof(SQLViewControl);
            fsql.ViewType = typeof(ServiceSQLFormView);
            this.ServiceSQLNode.ViewDescriptions.Add(fsql);
            this.ServiceSQLNode.ViewDescriptions.DefaultIndex = 0;

            // SMTP Scope
            this.ServiceSMTPNode = new ServiceSMTPScopeNode();
            FormViewDescription fsmtp = new FormViewDescription();
            fsmtp.DisplayName = "MFA Platform SMTP Properties";
            fsmtp.ControlType = typeof(SMTPViewControl);
            fsmtp.ViewType = typeof(ServiceSMTPFormView);
            this.ServiceSMTPNode.ViewDescriptions.Add(fsmtp);
            this.ServiceSMTPNode.ViewDescriptions.DefaultIndex = 0;

            // SMS Scope
            this.ServiceSMSNode = new ServicePhoneScopeNode();
            FormViewDescription fsms = new FormViewDescription();
            fsms.DisplayName = "MFA Platform SMS Properties";
            fsms.ControlType = typeof(SMSViewControl);
            fsms.ViewType = typeof(ServiceSMSFormView);
            this.ServiceSMSNode.ViewDescriptions.Add(fsms);
            this.ServiceSMSNode.ViewDescriptions.DefaultIndex = 0;

            // Parameters Scope
            this.ServiceSecurityNode = new ServiceSecurityScopeNode();
            FormViewDescription fvp = new FormViewDescription();
            fvp.DisplayName = "MFA Platform Security Properties";
            fvp.ControlType = typeof(ServiceSecurityViewControl);
            fvp.ViewType = typeof(ServiceSecurityFormView);
            this.ServiceSecurityNode.ViewDescriptions.Add(fvp);
            this.ServiceSecurityNode.ViewDescriptions.DefaultIndex = 0;

            // Users Scope
            this.UsersNode = new UsersScopeNode();
            FormViewDescription fvu = new FormViewDescription();
            fvu.DisplayName = "MFA Platform Users";
            fvu.ControlType = typeof(UsersListView);
            fvu.ViewType = typeof(UsersFormView);
            this.UsersNode.ViewDescriptions.Add(fvu);
            this.UsersNode.ViewDescriptions.DefaultIndex = 0;

            this.RootNode.Children.Add(this.ServiceNode);

            this.RootNode.Children.Add(this.ServiceGeneralNode);
            this.RootNode.Children.Add(this.ServiceADDSNode);
            this.RootNode.Children.Add(this.ServiceSQLNode);
            this.RootNode.Children.Add(this.ServiceSMTPNode);
            this.RootNode.Children.Add(this.ServiceSMSNode);
            this.RootNode.Children.Add(this.ServiceSecurityNode);
            this.RootNode.Children.Add(this.UsersNode);

            this.IsModified = true;
            this.SmallImages.Add(Neos_IdentityServer_Console_Snapin.folder16, Color.Black);
            this.LargeImages.Add(Neos_IdentityServer_Console_Snapin.folder32, Color.Black);

        }

        /// <summary>
        /// OnInitialize method implmentation
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            try
            {
                ManagementAdminService.Initialize(true);
            }
            catch (Exception ex)
            {
                MessageBoxParameters msgp = new MessageBoxParameters();
                msgp.Caption = "MFA Error";
                msgp.Text = ex.Message;
                msgp.Buttons = MessageBoxButtons.OK;
                msgp.Icon = MessageBoxIcon.Error;
                this.Console.ShowDialog(msgp);
                this.RootNode.Children.Clear();
            }
        }

        /// <summary>
        /// OnLoadCustomData method implmentation
        /// </summary>
        protected override void OnLoadCustomData(AsyncStatus status, byte[] persistenceData)
        {
            try
            {
                if (persistenceData != null)
                    ManagementAdminService.Filter = (UsersFilterObject)persistenceData;
            }
            catch (Exception ex)
            {
                MessageBoxParameters msgp = new MessageBoxParameters();
                msgp.Text = ex.Message;
                msgp.Buttons = MessageBoxButtons.OK;
                msgp.Icon = MessageBoxIcon.Error;
                this.Console.ShowDialog(msgp);
            }

        }

        /// <summary>
        /// OnSaveCustomData method implmentation
        /// </summary>
        protected override byte[] OnSaveCustomData(SyncStatus status)
        {
            try
            {
                return (byte[])ManagementAdminService.Filter;
            }
            catch (Exception ex)
            {
                MessageBoxParameters msgp = new MessageBoxParameters();
                msgp.Text = ex.Message;
                msgp.Buttons = MessageBoxButtons.OK;
                msgp.Icon = MessageBoxIcon.Error;
                this.Console.ShowDialog(msgp);
                return null;
            }
        }
    }
} 