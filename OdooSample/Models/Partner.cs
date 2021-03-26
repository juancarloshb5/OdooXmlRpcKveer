using System;

namespace OdooSample.Models
{
    public class Partner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public bool Active { get; set; }
        public int? UserId { get; set; }
        public bool IsCompany { get; set; }

        public string Email { get; set; }

        //public DateTime? WriteDate { get; set; }
        public DateTime? Date { get; set; }

        public int? CompanyId { get; set; }

        public int? ParentId { get; set; }

        public int? CountryId { get; set; }
    }
}
