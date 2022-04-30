using Eventor.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventor.Models
{
    public partial class LanguageSelectorModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<Language>();
        }

        public IList<Language> AvailableLanguages { get; set; }

        public int CurrentLanguageId { get; set; }

        public bool UseImages { get; set; }
    }

    public class LocaleStringResourcesModel
    {
        public LocaleStringResourcesModel()
        {
            Resources = new List<LocaleStringResource>();
        }
        public int LanguageId { get; set; }
        public int RecourceId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public List<LocaleStringResource> Resources { get; set; }
    }

    public class EventModel
    {
        public string Name { get; set; }
        public string website { get; set; }
        public string location { get; set; }
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public string Topics { get; set; }
    }


    public class EventInformationModel
    {
        public string EventTitle { get; set; }

        public string EventDescription { get; set; }

        public string Topic { get; set; } // Topic can hold multible value we may use , later we can split it to array or list

        public string EventLocation { get; set; } // your next task is to implement this from google its maybe called google geolocation service

        public string Imprint { get; set; }

        public string Terms { get; set; }

        public string WiFiname { get; set; }

        public string WiFiPassword { get; set; }

        public string GoogleTrackingID { get; set; }
    }

    public class ContentSnippetsModel
    {
        public string AgendaHeader { get; set; }
    }

    public class RegistraionSettingsModel
    {
        public RegistraionSettingsModel()
        {
            sessionMapping = new List<session>();
        }
        public DateTime RegistrationBeginDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public bool ParticipantActivationAuto { get; set; }

        public List<session> sessionMapping { get; set; }



    }

    public class session
    {

        public session()
        {
            perticipents = new List<partecepentmapping>();
        }

        public int sessionID { get; set; }
        public string sessionName { get; set; }

        public List<partecepentmapping> perticipents { get; set; }

    }

    public class partecepentmapping
    {

        public int partecepentID { get; set; }
        public string partecepentName { get; set; }
        public bool Regiterd { get; set; }

    }

    public class Participant
    {
        public int participantID { get; set; }
        public string title { get; set; }
        public int state { get; set; }
        public int participantCount { get; set; }
    }

    public class ParticipantTypesModel
    {
        public ParticipantTypesModel()
        {
            participants = new List<Participant>();
        }
        public List<Participant> participants { get; set; }
    }

    public class Organisation
    {
        public int OrganisationID { get; set; }
        public string name { get; set; }
        public int participantCount { get; set; }
    }

    public class OrganisationTypesModel
    {
        public OrganisationTypesModel()
        {
            organisations = new List<Organisation>();
        }
        public List<Organisation> organisations { get; set; }
    }

    public class Topic
    {
        public Topic()
        {
            AreaOfActivities = new List<AreaOfActivity>();
        }
        public int topicID { get; set; }
        public string name { get; set; }
        public int position { get; set; }
        public List<AreaOfActivity> AreaOfActivities { get; set; }

    }
    public class AreaOfActivity
    {
        public int AreaOfActivityID { get; set; } // no its needed for update ....
        public string name { get; set; }
        public int position { get; set; }
    }
    public class AreasOfActivityModel
    {
        public AreasOfActivityModel()
        {
            Topics = new List<Topic>();
        }
        public List<Topic> Topics { get; set; }
    }


}
