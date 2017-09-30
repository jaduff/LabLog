using System;
using System.ComponentModel.DataAnnotations;
using LabLog.Models;
using System.Collections.Generic;
namespace LabLog
{
    public class RoomModel
    {
        [Required]
        [Display(Name = "Room Identifier")]
        public string Name {get; set;}
        public Guid Id {get; set;}
        public int version {get; set;}
        public List<ComputerModel> Computers {get; set;}



        /* some way of representing room layout? Image blob? URL to image? something fancy like a floor plan using drag and drop? */
        
    }
}