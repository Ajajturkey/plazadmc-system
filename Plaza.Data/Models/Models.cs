using Line.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Models
{

    public partial class Picture
    {
        
        public int Id { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
    }

    public partial class LoginModel
    {
        public string Username { get; set; }
        public string password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool rememberMe { get; set; }
    }

    public partial class PasswordRecoverModel
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string token { get; set; }
    }

    public partial class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool AgreeTerms { get; set; }

    }


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

   

    public class EventModel
    {
        public int VisitorId { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public string website { get; set; }
        public string location { get; set; }
        public DateTime datefrom { get; set; }
        public DateTime dateto { get; set; }
        public string Topics { get; set; }
        public DateTime Regform { get; set; }
        public DateTime RegTo { get; set; }
        public bool IsMatch { get; set; }
        public DateTime Bookfrom { get; set; }
        public DateTime Bookto { get; set; }
    }


    public class EventInformationModel
    {
        public int Eventid { get; set; }

        public string EventTitle { get; set; }

        public string EventDescription { get; set; }

        public string Topic { get; set; } // Topic can hold multible value we may use , later we can split it to array or list

        public string EventLocation { get; set; } // your next task is to implement this from google its maybe called google geolocation service

        public string Imprint { get; set; }

        public string Terms { get; set; }

        public string WiFiname { get; set; }

        public string WiFiPassword { get; set; }

        public string GoogleTrackingID { get; set; }

        public string Timezone { get; set; }

        public DateTime datefrom { get; set; }
        public DateTime dateto { get; set; }
    }

    public class ContentSnippetsModel
    {
        
        public int Event_Id { get; set; }
        public string AgendaHeader { get; set; }
    }

    public class RegistraionSettingsModel
    {
        public RegistraionSettingsModel()
        {
            RegistraionRules = new List<RegistraionRulesModel>();
        }
        public int EventId { get; set; }
        public DateTime RegistrationBeginDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public bool ParticipantActivationAuto { get; set; }

        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }


        public List<RegistraionRulesModel> RegistraionRules { get; set; }



    }

    public class RegistraionRulesModel
    {

        public RegistraionRulesModel()
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

    public class ParticipantModel
    {
        public int TypeId { get; set; }
        public string title { get; set; }
        public bool state { get; set; }
        public int participantCount { get; set; }
    }

    public class ParticipantTypesModel
    {
        public ParticipantTypesModel()
        {
            participants = new List<ParticipantModel>();
        }

        public int EventId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int maxMeetimgs { get; set; }
        public bool showinRegistration { get; set; }

        public List<ParticipantModel> participants { get; set; }
    }

    public class Organisation
    {
        public int OrganisationID { get; set; }
        public string name { get; set; }
        public int participantCount { get; set; }
    }


    public class OrganisationModel
    {
        public string name { get; set; }
        public int   count { get; set; }
        public int   position { get; set; }
        public int   Id { get; set; }
    }
    public class OrganisationTypesModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public int   Position { get; set; }

        public OrganisationTypesModel()
        {
            OrganisationTypes = new List<OrganisationModel>();
        }



        public List<OrganisationModel> OrganisationTypes { get; set; }
    }


   
}
