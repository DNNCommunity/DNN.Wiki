using DotNetNuke.Entities.Modules;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;

namespace DotNetNuke.Wiki.Utilities
{
    public interface IDNNUtils
    {
        void PostTopicCommentToJournal(string summary, string title, string description, string linkToTopic, int currentTab, string topicName, SharedEnum.DNNJournalType journalType, ModuleInfo moduleInfo);
        void SendNotifications(UnitOfWork uow, Topic topic, string name, string email, string comment, string ipaddress);
    }
}