#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="DNNUtils.cs" company="DNN Corp®">
//      DNN Corp® - http://www.dnnsoftware.com Copyright (c) 2002-2013 by DNN Corp®
//
//      Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
//      associated documentation files (the "Software"), to deal in the Software without restriction,
//      including without limitation the rights to use, copy, modify, merge, publish, distribute,
//      sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//      furnished to do so, subject to the following conditions:
//
//      The above copyright notice and this permission notice shall be included in all copies or
//      substantial portions of the Software.
//
//      THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
//      NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//      NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//      DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//      OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
////------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Journal;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// DNN Utilities Class
    /// </summary>
    public class DNNUtils
    {
        /// <summary>
        /// Sends the notifications.
        /// </summary>
        /// <param name="uow">The Unit Of Work.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="ipaddress">The IP Address.</param>
        public static void SendNotifications(
            UnitOfWork uow,
            Topic topic,
            string name,
            string email,
            string comment,
            string ipaddress)
        {
            if (topic != null)
            {
                List<string> lstEmailsAddresses = new TopicBO(uow).GetNotificationEmails(topic);

                if (lstEmailsAddresses.Count > 0)
                {
                    DotNetNuke.Entities.Portals.PortalSettings objPortalSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings();
                    string strResourceFile = Globals.ApplicationPath + "/DesktopModules/Wiki/Views/" + Localization.LocalResourceDirectory + "/" + Localization.LocalSharedResourceFile;
                    string strSubject = Localization.GetString("NotificationSubject", strResourceFile);
                    string strBody = Localization.GetString("NotificationBody", strResourceFile);

                    string redirectUrl = DotNetNuke.Common.Globals.NavigateURL(objPortalSettings.ActiveTab.TabID, objPortalSettings, string.Empty, "topic=" + WikiMarkup.EncodeTitle(topic.Name));
                    strBody = strBody.Replace("[URL]", redirectUrl);

                    strBody = strBody.Replace("[NAME]", name);
                    strBody = strBody.Replace("[EMAIL]", email);
                    strBody = strBody.Replace("[COMMENT]", comment);
                    strBody = strBody.Replace("[IP]", string.Empty);

                    System.Text.StringBuilder usersToEmailSB = new System.Text.StringBuilder();
                    foreach (string userToEmail in lstEmailsAddresses)
                    {
                        usersToEmailSB.Append(userToEmail);
                        usersToEmailSB.Append(";");
                    }

                    // remove the last ;
                    usersToEmailSB.Remove(usersToEmailSB.Length - 1, 1);

                    // Services.Mail.Mail.SendMail(objPortalSettings.Email, objPortalSettings.Email,
                    //// sbUsersToEmail.ToString, strSubject, strBody, "", "", "", "", "", "")

                    Mail.SendMail(
                        objPortalSettings.Email,
                        usersToEmailSB.ToString(),
                        string.Empty,
                        string.Empty,
                        MailPriority.Normal,
                        strSubject,
                        MailFormat.Html,
                        Encoding.UTF8,
                        strBody,
                        string.Empty,
                        Host.SMTPServer,
                        Host.SMTPAuthentication,
                        Host.SMTPUsername,
                        Host.SMTPPassword,
                        Host.EnableSMTPSSL);
                }
            }
        }

        /// <summary>
        /// Posts the topic comment to DNN journal.
        /// </summary>
        /// <param name="summary">The summary.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="linkToTopic">The link to topic.</param>
        /// <param name="currentTab">The current tab.</param>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="journalType">Type of the journal.</param>
        public static void PostTopicCommentToJournal(
            string summary,
            string title,
            string description,
            string linkToTopic,
            int currentTab,
            string topicName,
            SharedEnum.DNNJournalType journalType)
        {
            if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserInfo user = UserController.GetCurrentUserInfo();
                if (user != null)
                {
                    // Post to DotnetNuke Journal
                    var journalController = JournalController.Instance;
                    var journalItem = new JournalItem();
                    journalItem.Summary = summary;
                    journalItem.PortalId = PortalSettings.Current.PortalId;
                    journalItem.ProfileId = user.UserID;
                    journalItem.SocialGroupId = Null.NullInteger;
                    journalItem.UserId = user.UserID;

                    journalItem.ItemData = new ItemData()
                    {
                        Description = description,
                        Title = title,
                        Url = linkToTopic
                    };

                    journalItem.JournalTypeId = (int)journalType;
                    journalItem.ObjectKey = string.Empty;
                    journalItem.SecuritySet = "F,";
                    //// http: //www.dnnsoftware.com/wiki/loc/history/Page/Journal/Revision/11

                    journalItem.Title = topicName;
                    journalController.SaveJournalItem(journalItem, currentTab); // saving
                }
            }
        }
    }
}