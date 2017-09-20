using System;
using LabLog.Models;
using System.Collections.Generic;
namespace LabLog
{
    class RoomModel
    {
        public string Name {get; set;}
        public List<ComputerModel> Computers {get; set;}

        public void createComputers(int computerCount) {
            Computers = new List<ComputerModel>();
            for (int i =0; i<computerCount; i++){
               ComputerModel computer = new ComputerModel();
               computer.ComputerNumber = i+1;
               Computers.Add(computer);
            }

        }
        /* some way of representing room layout? Image blob? URL to image? something fancy like a floor plan using drag and drop? */
        
    }
}