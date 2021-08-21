
namespace challenge.Models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
    
        public ReportingStructure RefreshStructure()
        {
            if(Employee == null)
            {
                return null;
            }

            this.NumberOfReports = FindAllReports(Employee) - 1;
            return this;
        }

        private int FindAllReports(Employee employee)
        {
            int reportsToProvide = 1;

            if(employee.DirectReports == null)
            {
                return reportsToProvide;
            }

            foreach(Employee underling in employee.DirectReports)
            {
                reportsToProvide += FindAllReports(underling);
            }

            return reportsToProvide;
        }
    }
}
