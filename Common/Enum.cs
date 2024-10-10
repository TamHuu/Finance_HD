using Finance_HD.Common.Attributes;

namespace Finance_HD.Common
{
    public enum Gender
    {
        Male = 1,
        Female = 2
    }

    public enum HinhThucThuChi
    {
        TienMat = 1,
        TaiKhoanCaNhan = 2,
        NganHang = 3
    }

    public enum TrangThaiChungTu
    {
        LapPhieu = 1,
        DaDuyet = 2,
        DaThu = 3,
        DaChi = 4
    }

    public enum PhongBan
    {
        [EnumGuid("C065405F-DCC5-4BC9-96F2-0E4A39E4CBAF")]
        PHONGKYTHUAT = 1,

        [EnumGuid("F79AC5AF-85B1-43E4-964B-25515F302C86")]
        KhuVuiChoi = 2,
    }

   
}
