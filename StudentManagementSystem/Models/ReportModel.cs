
namespace StudentManagementSystem.Models
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            count1 = 0;
            count2 = 0;
            count3 = 0;
            count4 = 0;
            count5 = 0;
            count6 = 0;
            count7 = 0;
            count8 = 0;
            count9 = 0;
            count10 = 0;
            count11 = 0;
            count12 = 0;
        }

        public Student LastAddedStudent { get; set; }
        public Student LastMonthAddedStudent { get; set; }

        public int count1 { get; set; }
        public int count2 { get; set; }
        public int count3 { get; set; }
        public int count4 { get; set; }
        public int count5 { get; set; }
        public int count6 { get; set; }
        public int count7 { get; set; }
        public int count8 { get; set; }
        public int count9 { get; set; }
        public int count10 { get; set; }
        public int count11 { get; set; }
        public int count12 { get; set; }
    }
}
