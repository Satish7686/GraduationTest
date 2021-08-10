using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker
{
    public partial class GraduationTracker
    {
        public delegate Tuple<bool, Standing> HasGraduatedDelegate(Diploma d,Student s);
        
        public static Tuple<bool, Standing>  HasGraduated(Diploma diploma, Student student)
        {
            var credits = 0;
            var average = 0;
        //Looping through 
            foreach (var d in diploma.Requirements)
            {
                foreach (var s in student.Courses)
                {
                    var requirement = Repository.GetRequirement(d);

                    foreach (var r in requirement.Courses)
                    {
                        try
                        {
                            if (r == s.Id)
                            {
                                average += s.Mark;
                                if (s.Mark > requirement.MinimumMark)
                                {
                                    credits += requirement.Credits;
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            throw(e);
                        }
                       
                    }
                }
            }

            average = average / student.Courses.Length;

            var standing = average switch
            {
                < 50 => Standing.Remedial,
                < 80 => Standing.Average,
                < 95 => Standing.MagnaCumLaude,
                _ => Standing.MagnaCumLaude
            };

            return standing switch
            {
                Standing.Remedial => new Tuple<bool, Standing>(false, standing),
                Standing.Average => new Tuple<bool, Standing>(true, standing),
                Standing.SumaCumLaude => new Tuple<bool, Standing>(true, standing),
                Standing.MagnaCumLaude => new Tuple<bool, Standing>(true, standing),
                _ => new Tuple<bool, Standing>(false, standing)
            };
        }
    }
}
