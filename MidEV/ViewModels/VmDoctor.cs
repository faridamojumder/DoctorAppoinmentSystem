using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidEV.ViewModels
{
    public class VmDoctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string Gender { get; set; }
        public IFormFile Prescription { get; set; }
        public int? SpecialistId { get; set; }
    }
}
