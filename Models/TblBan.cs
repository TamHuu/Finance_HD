using System;

namespace Finance_HD.Models
{
    public class TblBan
    {
        public Guid Ma { get; set; } 
        public Guid? MaChiNhanh { get; set; } // Khóa ngoại đến chi nhánh
        public string? Code { get; set; } 
        public string? Ten { get; set; } 
        public bool? Status { get; set; } 
        public Guid? UserCreated { get; set; } 
        public DateTime? CreatedDate { get; set; }
        public Guid? UserModified { get; set; } 
        public DateTime? ModifiedDate { get; set; } 
        public bool? Deleted { get; set; } 
        public Guid? UserDeleted { get; set; } 
        public DateTime? DeletedDate { get; set; }
    }
}
