using System;
using System.ComponentModel.DataAnnotations;

namespace mascotas.validations
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute()
          : base(typeof(DateTime), DateTime.Now.AddYears(-20).ToShortDateString(), DateTime.Now.ToShortDateString()) { }
    }
}