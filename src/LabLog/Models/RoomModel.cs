using System;
using LabLog.Models;
using System.Collections.Generic;
namespace LabLog
{
    public class RoomModel
    {
        public string Name {get; set;}
        public List<ComputerModel> Computers {get; set;}

        public void createComputers(int computerCount) {
            Computers = new List<ComputerModel>();
            for (int i =1; i<computerCount+1; i++){
               ComputerModel computer = new ComputerModel();
               computer.ComputerNumber = i;
               Computers.Add(computer);
            }

        }
        /* some way of representing room layout? Image blob? URL to image? something fancy like a floor plan using drag and drop? */
        
    }
}