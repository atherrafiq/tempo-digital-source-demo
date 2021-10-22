using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class MembershipType
    {
        public byte MembershipTypeId { get; set; }
        public short SignUpFee { get; set; }
        public Byte DurationInMonths { get; set; }
        public byte DiscountRate { get; set; }

    }
}