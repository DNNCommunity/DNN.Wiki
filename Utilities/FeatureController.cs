#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="FeatureController.cs" company="DNN Corp®">
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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Search;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;
using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Xml;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// <para>The Controller class for DNNModule1</para> <para>The FeatureController class is
    /// defined as the BusinessController in the manifest file (.dnn) DotNetNuke will poll this
    /// class to find out which Interfaces the class implements.</para> <para>The IPortable
    /// interface is used to import/export content from a DNN module.</para> <para>The ISearchable
    /// interface is used by DNN to index the content of a module.</para> <para>The IUpgradeable
    /// interface allows module developers to execute code during the upgrade process for a module.
    /// </para> <para>Below you will find stubbed out implementations of each, uncomment and
    /// populate with your own data.</para> <para>uncomment the interfaces to add the support.
    /// </para>
    /// </summary>
    public class FeatureController : IPortable, ISearchable // , IUpgradeable
    {
        //// Implements IUpgradeable

        #region Variables

        private string mSharedResourceFile =
            DotNetNuke.Common.Globals.ApplicationPath + "/DesktopModules/Wiki/Views/" + Localization.LocalResourceDirectory + "/" + Localization.LocalSharedResourceFile;

        #endregion Variables

        #region Methods

        /// <summary>
        /// Gets the search items.
        /// </summary>
        /// <param name="modInfo">The module information.</param>
        /// <returns>Topics that meet the search criteria.</returns>
        public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            using (UnitOfWork uOw = new UnitOfWork())
            {
                TopicBO topicBo = new TopicBO(uOw);

                SearchItemInfoCollection searchItemCollection = new SearchItemInfoCollection();
                var topics = topicBo.GetAllByModuleID(modInfo.ModuleID);
                UserController uc = new UserController();

                foreach (var topic in topics)
                {
                    SearchItemInfo searchItem = new SearchItemInfo();

                    string strContent = null;
                    string strDescription = null;
                    string strTitle = null;
                    if (!string.IsNullOrWhiteSpace(topic.Title))
                    {
                        strTitle = topic.Title;
                    }
                    else
                    {
                        strTitle = topic.Name;
                    }

                    if (topic.Cache != null)
                    {
                        strContent = topic.Cache;
                        strContent += " " + topic.Keywords;
                        strContent += " " + topic.Description;

                        strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(HttpUtility.HtmlDecode(topic.Cache), false), 100,
                            Localization.GetString("Dots", this.mSharedResourceFile));
                    }
                    else
                    {
                        strContent = topic.Content;
                        strContent += " " + topic.Keywords;
                        strContent += " " + topic.Description;

                        strDescription = HtmlUtils.Shorten(HtmlUtils.Clean(HttpUtility.HtmlDecode(topic.Content), false), 100,
                            Localization.GetString("Dots", this.mSharedResourceFile));
                    }

                    int userID = 0;

                    userID = Null.NullInteger;
                    if (topic.UpdatedByUserID != -9999)
                    {
                        userID = topic.UpdatedByUserID;
                    }

                    searchItem = new SearchItemInfo(strTitle, strDescription, userID, topic.UpdateDate, modInfo.ModuleID, topic.Name, strContent, "topic=" + WikiMarkup.EncodeTitle(topic.Name));

                    //// New SearchItemInfo(ModInfo.ModuleTitle & "-" & strTitle, strDescription,
                    //// userID, topic.UpdateDate, ModInfo.ModuleID, topic.Name, strContent, _
                    //// "topic=" & WikiMarkup.EncodeTitle(topic.Name))

                    searchItemCollection.Add(searchItem);
                }

                return searchItemCollection;
            }
        }

        /// <summary>
        /// Exports the module.
        /// </summary>
        /// <param name="moduleID">The module unique identifier.</param>
        /// <returns>XML String of the module data</returns>
        public string ExportModule(int moduleID)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                TopicBO topicBo = new TopicBO(uow);
                var topics = topicBo.GetAllByModuleID(moduleID);

                ModuleController mc = new ModuleController();
                Hashtable settings = mc.GetModuleSettings(moduleID);

                StringWriter strXML = new StringWriter();
                XmlWriter writer = new XmlTextWriter(strXML);
                writer.WriteStartElement("Wiki");

                writer.WriteStartElement("Settings");
                foreach (DictionaryEntry item in settings)
                {
                    writer.WriteStartElement("Setting");
                    writer.WriteAttributeString("Name", Convert.ToString(item.Key));
                    writer.WriteAttributeString("Value", Convert.ToString(item.Value));
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.WriteStartElement("Topics");
                foreach (var topic in topics)
                {
                    writer.WriteStartElement("Topic");
                    writer.WriteAttributeString("AllowDiscussions", topic.AllowDiscussions.ToString());
                    writer.WriteAttributeString("AllowRatings", topic.AllowRatings.ToString());
                    writer.WriteAttributeString("Content", topic.Content);
                    writer.WriteAttributeString("Description", topic.Description);
                    writer.WriteAttributeString("Keywords", topic.Keywords);
                    writer.WriteAttributeString("Name", topic.Name);
                    writer.WriteAttributeString("Title", topic.Title);
                    writer.WriteAttributeString("UpdateDate", topic.UpdateDate.ToString("g"));
                    writer.WriteAttributeString("UpdatedBy", topic.UpdatedBy);
                    writer.WriteAttributeString("UpdatedByUserID", topic.UpdatedByUserID.ToString("g"));
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.Close();

                return strXML.ToString();
            }
        }

        /// <summary>
        /// Imports the module.
        /// </summary>
        /// <param name="moduleID">The module unique identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="version">The version.</param>
        /// <param name="userID">The user unique identifier.</param>
        public void ImportModule(int moduleID, string content, string version, int userID)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                XmlNode node = null;
                XmlNode nodes = Globals.GetContent(content, "Wiki");
                ModuleController objModules = new ModuleController();
                foreach (XmlNode node_loopVariable in nodes.SelectSingleNode("Settings"))
                {
                    node = node_loopVariable;
                    objModules.UpdateModuleSetting(moduleID, node.Attributes["Name"].Value, node.Attributes["Value"].Value);
                }

                TopicBO topicBo = new TopicBO(uow);

                // clean up
                var topics = topicBo.GetAllByModuleID(moduleID);
                foreach (var topic in topics)
                {
                    // TODO - On the old version topics where deleted via the SPROC
                    // [Wiki_TopicDelete], it should be dropped in this new version
                    topicBo.Delete(new Topic { TopicID = topic.TopicID });
                }

                try
                {
                    foreach (XmlNode node_loopVariable in nodes.SelectNodes("Topics/Topic"))
                    {
                        node = node_loopVariable;
                        var topic = new Topic();
                        topic.PortalSettings = PortalController.GetCurrentPortalSettings();
                        topic.AllowDiscussions = bool.Parse(node.Attributes["AllowDiscussions"].Value);
                        topic.AllowRatings = bool.Parse(node.Attributes["AllowRatings"].Value);
                        topic.Content = node.Attributes["Content"].Value;
                        topic.Description = node.Attributes["Description"].Value;
                        topic.Keywords = node.Attributes["Keywords"].Value;
                        topic.ModuleId = moduleID;
                        //// Here we need to define the TabID otherwise the import won't work until
                        //// the content is saved again.
                        ModuleController mc = new ModuleController();
                        ModuleInfo mi = mc.GetModule(moduleID, -1);
                        topic.TabID = mi.TabID;

                        topic.Name = node.Attributes["Name"].Value;
                        topic.RatingOneCount = 0;
                        topic.RatingTwoCount = 0;
                        topic.RatingThreeCount = 0;
                        topic.RatingFourCount = 0;
                        topic.RatingFiveCount = 0;
                        topic.RatingSixCount = 0;
                        topic.RatingSevenCount = 0;
                        topic.RatingEightCount = 0;
                        topic.RatingNineCount = 0;
                        topic.RatingTenCount = 0;
                        topic.Title = node.Attributes["Title"].Value;
                        topic.UpdateDate = DateTime.Parse(node.Attributes["UpdateDate"].Value);
                        topic.UpdatedBy = node.Attributes["UpdatedBy"].Value;
                        topic.UpdatedByUserID = int.Parse(node.Attributes["UpdatedByUserID"].Value);
                        topicBo.Add(topic);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        //// Public Function UpgradeModule(ByVal Version As String) As String Implements
        //// IUpgradeable.UpgradeModule InitPermissions() Return Version End Function

        //// Private Sub InitPermissions() Dim EditContent As Boolean

        //// Dim moduleDefId As Integer Dim pc As New PermissionController Dim permissions As
        //// ArrayList = pc.GetPermissionByCodeAndKey("WIKI", Nothing) Dim dc As New
        //// DesktopModuleController Dim desktopInfo As DesktopModuleInfo desktopInfo =
        //// dc.GetDesktopModuleByModuleName("Wiki") Dim mc As New ModuleDefinitionController Dim
        //// mInfo As ModuleDefinitionInfo mInfo =
        //// mc.GetModuleDefinitionByName(desktopInfo.DesktopModuleID, "Wiki") moduleDefId =
        //// mInfo.ModuleDefID For Each p As PermissionInfo In permissions If p.PermissionKey =
        //// "EDIT_CONTENT" And p.ModuleDefID = moduleDefId Then _ EditContent = True Next If Not
        //// EditContent Then Dim p As New PermissionInfo p.ModuleDefID = moduleDefId
        //// p.PermissionCode
        //// = "WIKI" p.PermissionKey = "EDIT_CONTENT" p.PermissionName = "Edit Content"
        //// pc.AddPermission(p) End If End Sub

        #endregion Methods
    }
}