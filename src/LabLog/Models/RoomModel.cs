using System;
using LabLog.Models;
namespace LabLog
{
    class RoomModel
    {
        public string name {get; set;}
        public ComputerModel[] computers {get; set;}
        /* some way of representing room layout? Image blob? URL to image? something fancy like a floor plan using drag and drop? */
        
    }
}