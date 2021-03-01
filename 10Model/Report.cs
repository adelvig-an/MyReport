using System;

namespace _10Model
{
    public class Report
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime? VulationDate { get; set; }
        public DateTime? CompilationDate { get; set; }
        public DateTime? InspectionDate { get; set; }
        public string InspectionFeaures { get; set; }
    }
}
