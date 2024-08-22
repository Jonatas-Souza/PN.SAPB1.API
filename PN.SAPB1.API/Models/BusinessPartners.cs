using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PN.SAPB1.API.Models
{
    public class BusinessPartners
    {
        [Key]
        public string CardCode { get; set; }
        public string? CardName { get; set; }
        public string? CardType { get; set; }
        public string? Phone1 { get; set; }
        public string? EmailAddress { get; set; }
        public List<Bpaddress> BPAddresses { get; set; } = new List<Bpaddress>();
        public List<Bpfiscaltaxidcollection> BPFiscalTaxIDCollection { get; set; } = new List<Bpfiscaltaxidcollection>();
    }

    public class Bpaddress
    {
        public string? AddressName { get; set; }
        public string? Street { get; set; }
        public string? Block { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? FederalTaxID { get; set; }
        public string? TaxCode { get; set; }
        public string? BuildingFloorRoom { get; set; }
        public string? AddressType { get; set; }
        public string? AddressName2 { get; set; }
        public string? AddressName3 { get; set; }
        public string? TypeOfAddress { get; set; }
        public string? StreetNo { get; set; }
        [ForeignKey("BusinessPartners")]
        public string? BPCode { get; set; }
        [NotMapped]
        [JsonIgnore]
        public BusinessPartners? BusinessPartner { get; set; }
    }

    public class Bpfiscaltaxidcollection
    {
        public string? Address { get; set; }
        public int? CNAECode { get; set; }
        public string? TaxId0 { get; set; }
        public string? TaxId1 { get; set; }
        public string? TaxId2 { get; set; }
        public string? TaxId3 { get; set; }
        public string? TaxId4 { get; set; }
        public string? TaxId5 { get; set; }
        public string? TaxId6 { get; set; }
        public string? TaxId7 { get; set; }
        public string? TaxId8 { get; set; }
        public string? TaxId9 { get; set; }
        public string? TaxId10 { get; set; }
        public string? TaxId11 { get; set; }
        [ForeignKey("BusinessPartners")]
        public string? BPCode { get; set; }
        public string? AddrType { get; set; }
        public string? TaxId12 { get; set; }
        public string? TaxId13 { get; set; }
        public string? AToRetrNFe { get; set; }
        public string? TaxId14 { get; set; }
        [NotMapped]
        [JsonIgnore]
        public BusinessPartners? BusinessPartner { get; set; }
    }


}
