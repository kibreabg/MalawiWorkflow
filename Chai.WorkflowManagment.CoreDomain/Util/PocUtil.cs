using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Util
{
    public class PocUtil
    {
        public static string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public static int GetMonthValue(string mname)
        {
            for (int i = 0; i < 12; i++)
            {
                if (Months[i] == mname)
                    return i + 1;
            }
            return 1;
        }

        public static T[] EnumToArray<T>()
        {
            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T must be a System.Enum");
            }
            return (Enum.GetValues(enumType) as IEnumerable<T>).ToArray();
        }

        public static DateTime CorrectDateTime(DateTime durationdt)
        {
            DateTime date = new DateTime();
            if (durationdt.Day > 28)
            {
                if (durationdt.Month == 12)
                    date = new DateTime(durationdt.Year + 1, 1, 01);
                else
                    date = new DateTime(durationdt.Year, durationdt.Month + 1, 01);
            }
            else
            {
                date = durationdt;
            }
            return date;
        }
    }
}
