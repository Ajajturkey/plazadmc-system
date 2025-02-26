using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Line.Models
{
    public class ParticipantProfile
    {
        public ParticipantProfile()
        {
            SessionsToAttend = new List<SessionAttendModel>();
            QuestionsGroups = new List<QuestionAnswerGroupsModel>();
        }

        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobPosition { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }

        public string ParticipantType { get; set; }

        public string OrganisationName { get; set; }
        public string OrganisationType { get; set; }
        public string OrganisationWebsite { get; set; }
        public string OrganisationDescription { get; set; }
        public string OrganisationLogo { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public string topics { get; set; }

        public List<SessionAttendModel> SessionsToAttend { get; set; }
        public List<QuestionAnswerGroupsModel> QuestionsGroups { get; set; }

        public string Sessions { get; set; }

        [UIHint("Picture")]
        public int ProfilePictureID { get;  set; }
        [UIHint("Picture")]
        public int OrganisationLogoID { get;  set; }
    }

    public class QuestionAnswerGroupsModel
    {
        public QuestionAnswerGroupsModel()
        {
            Questions = new List<QuestionAnswersModel>();
        }
        public int id { get; set; }
        public string name { get; set; }
        public string desciption { get; set; }
        public List<QuestionAnswersModel> Questions { get; set; }

    }

    public class QuestionAnswersModel
    {
        public QuestionAnswersModel()
        {
            options = new List<QuestionOptionModel>();

        }
        public int id { get; set; }
        public string Title { get; set; }
        public string helpDesc { get; set; }

        public string Answer { get; set; }
        public string Type { get; set; }
        public List<QuestionOptionModel> options { get; set; }
    }

    public class QuestionOptionModel
    {
        public int id { get; set; }
        public string option { get; set; }
        public bool Ischecked { get; set; }
    }

    public class SessionAttendModel
    {
        public int id { get; set; }
        public string  Name { get; set; }
        public string  Date { get; set; }
        public string  Time { get; set; }
        public string  Location { get; set; }
        public string  Type { get; set; }
        public bool willgo { get; set; }

    }
}
