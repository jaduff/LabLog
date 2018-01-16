using System;

namespace LabLog
{
    public class ComputerUserModel
    {
        public string UsernameAssigned { get; set; }
        public DateTime TimeAssigned {get; set;}
        public string DetectedUsername {get; set;}

        public ComputerUserModel()
        {

        }

        public ComputerUserModel(DateTime timeAssigned, string usernameAssigned)
        {
            UsernameAssigned = usernameAssigned;
            TimeAssigned = timeAssigned;
        }
    }
}