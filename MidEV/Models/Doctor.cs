using System;
using System.Collections.Generic;

#nullable disable

namespace MidEV.Models
{
    public partial class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string Gender { get; set; }
        public string Prescription { get; set; }
        public int? TotalPatient { get; set; }
        public int? SpecialistId { get; set; }
    }
}
