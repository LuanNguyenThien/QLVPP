CREATE DATABASE QuanLyVPP
GO

USE QuanLyVPP
GO
--Tạo bảng--
--Tạo bảng NSX--
CREATE TABLE NhaSanXuat(
	MaNSX int IDENTITY(1,1) NOT NULL CONSTRAINT PK_NhaSanXuat PRIMARY KEY,
	TenNSX nvarchar(255) NOT NULL,
	DiaChi nvarchar(255) NOT NULL,
	Sdt char(10) UNIQUE NOT NULL 
)
--Tạo bảng Loại SP--
CREATE TABLE LoaiSanPham(
	MaLoaiSP int IDENTITY(1,1) NOT NULL CONSTRAINT PK_LoaiSanPham PRIMARY KEY,
	TenLoaiSP nvarchar(255) NOT NULL
)
--Tạo bảng SP--
CREATE TABLE SanPham(
	MaSP int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Sanpham PRIMARY KEY,
	MaLoaiSP int CONSTRAINT FK_SP_LSP FOREIGN KEY REFERENCES LoaiSanPham(MaLoaiSP), 
	TenSP nvarchar(255) NOT NULL,
	DonViTinh nvarchar(20) NOT NULL, 
	GiaTien int NOT NULL CHECK(GiaTien > 0),
	GiaNhap int	NOT NULL CHECK(GiaNhap > 0),
	SoLuong int NOT NULL CHECK(SoLuong > 0),
	LoiNhuan int NOT NULL CHECK(LoiNhuan > 0),
	KhuyenMai int NOT NULL CHECK(KhuyenMai >= 0),
	TinhTrang nvarchar(21) NOT NULL CHECK(TinhTrang IN (N'Ngưng bán', N'Còn bán')),
	HinhAnh image
)
--Tạo bảng cung cấp--
CREATE TABLE CungCap(
	MaSP int CONSTRAINT FK_CungCap_SP FOREIGN KEY REFERENCES SanPham(MaSP),
	MaNSX int CONSTRAINT FK_CungCap_NSX FOREIGN KEY REFERENCES NhaSanXuat(MaNSX),
	CONSTRAINT PK_CungCap PRIMARY KEY (MaSP, MaNSX)
)
--Tạo bảng đơn nhập hàng--
CREATE TABLE DonNhapHang(
	MaDNH int IDENTITY(1,1) NOT NULL CONSTRAINT PK_DonNhapHang PRIMARY KEY,
	NgayNhap date NOT NULL CHECK (DATEDIFF(day, NgayNhap, GETDATE()) >= 0),
    TrangThai nvarchar(20) NOT NULL CHECK (TrangThai = N'Đã thanh toán' or TrangThai = N'Chưa thanh toán'),
	TriGia float NOT NULL CHECK (TriGia >= 0)
)
--tạo bảng nhập hàng--
CREATE TABLE NhapHang(
	MaDNH int CONSTRAINT FK_NH_DNH FOREIGN KEY REFERENCES DonNhapHang(MaDNH),
	MaSP int CONSTRAINT FK_NH_SP FOREIGN KEY REFERENCES SanPham(MaSP),
	SoLuong int CHECK(SoLuong > 0),
	TongTien int CHECK(TongTien > 0),
	CONSTRAINT PK_NhapHang PRIMARY KEY (MaDNH, MaSP)
)
-- tạo bảng ứng dụng--
CREATE TABLE UngDung(
	MaUD int IDENTITY(1,1) NOT NULL CONSTRAINT PK_UngDung PRIMARY KEY,
	TenUD nvarchar(50) NOT NULL,
	ChietKhau float NOT NULL check(ChietKhau >= 0 and ChietKhau <= 1)
)
--tạo bảng hóa đơn trực tuyến--
CREATE TABLE HoaDonTrucTuyen(
	MaHDOnl int IDENTITY(1,1) NOT NULL CONSTRAINT PK_HDOnl  PRIMARY KEY,
	MaUD int CONSTRAINT FK_HDOnl_UD FOREIGN KEY REFERENCES UngDung(MaUD),
	NgayDat date NOT NULL CHECK (DATEDIFF(day, NgayDat, GETDATE()) >= 0),
	TrangThai nvarchar(30) NOT NULL CHECK (TrangThai = N'Đã thanh toán' or TrangThai = N'Chưa thanh toán'),
	TriGiaHoaDon int NOT NULL CHECK (TriGiaHoaDon >= 0)
)
--Tạo bảng chi tiết bán trực tuyến
CREATE TABLE ChiTietBanTrucTuyen(
	MaHDOnl int CONSTRAINT FK_CTBOnl_HDOnl FOREIGN KEY REFERENCES HoaDonTrucTuyen(MaHDOnl),
	MaSP int CONSTRAINT FK_CTBOnl_SP FOREIGN KEY REFERENCES SanPham(MaSP),
	Soluong int NOT NULL CHECK (SoLuong > 0),
	TongTien int NOT NULL CHECK (TongTien > 0),
	CONSTRAINT PK_CTBOnl PRIMARY KEY (MaHDOnl, MaSP)
)
--Tạo bảng khách hàng--
CREATE TABLE KhachHang(
	MaKH int IDENTITY(1,1) NOT NULL CONSTRAINT PK_KhachHang PRIMARY KEY,
	Hoten nvarchar(255) NOT NULL,
	Sdt char(10) NOT NULL,
	DiemTichLuy int NOT NULL CHECK(DiemTichLuy >= 0)
)
-- Tạo bảng Tài khoản--
CREATE TABLE TaiKhoan(
	TenTK int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Taikhoan PRIMARY KEY,
	MatKhau nvarchar(31) NOT NULL CHECK(len(MatKhau) >= 8)
)
--Tạo bảng CV--
CREATE TABLE CongViec(
	MaCV int IDENTITY(1,1) NOT NULL constraint PK_CongViec primary key,
	TenCV varchar(255) not null,
	Luong int not null check (Luong > 0)
)
-- Tạo bảng Ca làm--
CREATE TABLE CaLam (
	MaCa int IDENTITY(1,1) NOT NULL,
	NgayLam date NOT NULL,
	LoaiCa varchar(10) CHECK (LoaiCa = 'Fulltime' or LoaiCa = 'Parttime'),
	GioBatDau int CHECK (GioBatDau >= 7 or GioBatDau <= 23),
	GioKetThuc int CHECK (GioKetThuc >= 7 or GioKetThuc <= 23),
	CHECK (GioKetThuc - GioBatDau >= 4),
	CONSTRAINT PK_CaLam PRIMARY KEY (MaCa, NgayLam),
)
--Tạo bảng nhân viên--
create table NhanVien(
	MaNV int IDENTITY(1,1) NOT NULL constraint PK_NhanVien primary key,
	HoTen nvarchar(255) not null,
	NgaySinh date not null check (datediff(year, NgaySinh, getdate()) >= 18),
	Sdt char(10) unique not null,
	GioiTinh nvarchar(3) check (GioiTinh = 'Nam' or GioiTinh = N'Nữ'),
	DiaChi nvarchar(255) not null,
	NgayTuyenDung date not null check (datediff(day, NgayTuyenDung, getdate()) >= 0),
	TrangThai nvarchar(20) check (TrangThai = N'Đang làm' or TrangThai = N'Đã nghỉ'),
	MaCV int not null constraint PK_NhanVien_CongViec foreign key references CongViec(MaCV),
	TenTK int constraint FK_NV_TK foreign key references TaiKhoan(TenTK)
)
--Tạo bảng phân ca--
CREATE TABLE PhanCa(
	MaNV int constraint FK_PC_NV foreign key references NhanVien(MaNV),
	MaCa int,
	NgayLam date,
	CONSTRAINT PK_PhanCa PRIMARY KEY (MaNV, MaCa, NgayLam),
	CONSTRAINT FK_PC_CL foreign key (MaCa, NgayLam) references CaLam(MaCa, NgayLam),
)
--Tạo bảng hóa đơn trực tiếp--
CREATE TABLE HoaDonTrucTiep(
	MaHD int IDENTITY(1,1) NOT NULL CONSTRAINT PK_HDTT PRIMARY KEY,
	MaNV int CONSTRAINT FK_HDTT_NV FOREIGN KEY REFERENCES NhanVien(MaNV),
	MaKH int CONSTRAINT FK_HDTT_KH FOREIGN KEY REFERENCES KhachHang(MaKH),
	NgayBanHang date CHECK (DATEDIFF(day, NgayBanHang, GETDATE()) >= 0),
	TrangThai nvarchar(21) NOT NULL CHECK(TrangThai IN (N'Đã thanh toán', N'Chưa Thanh Toán')),
	TriGiaHoaDon int NOT NULL CHECK(TriGiaHoaDon >= 0)
)
--Tạo bảng chi tiết bán trực tuyến--
CREATE TABLE ChiTietBanTrucTiep (
	MaHD int CONSTRAINT FK_CTBTT_HDTT FOREIGN KEY REFERENCES HoaDonTrucTiep(MaHD),
	MaSP int CONSTRAINT FK_CTBTT_SP FOREIGN KEY REFERENCES SanPham(MaSP),
	Soluong int NOT NULL CHECK (SoLuong > 0),
	TongTien int NOT NULL CHECK (TongTien > 0),
	CONSTRAINT PK_CTBTT PRIMARY KEY (MaHD, MaSP)
)
--Triggers--
--Trigger tính giá bán sau khi nhập giá nhập, lợi nhuận, khuyến mãi --
CREATE TRIGGER TinhGiaBan
ON SanPham 
INSTEAD OF INSERT AS  
BEGIN 
    DECLARE @MaSP int, @MaLoaiSP int, @TenSP nvarchar(255), @DonViTinh nvarchar(20), @GiaTien int, @GiaNhap int, @SoLuong int, @LoiNhuan int, @KhuyenMai int, @TinhTrang nvarchar(21), @HinhAnh varbinary(max) 
SELECT  
	@MaLoaiSP = Inserted.MaLoaiSP,  
	@TenSP = Inserted.TenSP, 
	@DonViTinh = Inserted.DonViTinh,
	@GiaNhap = Inserted.GiaNhap,
	@SoLuong = Inserted.SoLuong,
	@LoiNhuan = Inserted.LoiNhuan,
	@KhuyenMai = Inserted.KhuyenMai,
	@TinhTrang = Inserted.TinhTrang,
	@HinhAnh = Inserted.HinhAnh
	FROM Inserted
	SET @GiaTien = @GiaNhap + @GiaNhap * @LoiNhuan / 100
	INSERT INTO SanPham(MaLoaiSP, TenSP, DonViTinh, GiaTien, GiaNhap, SoLuong, LoiNhuan, KhuyenMai, TinhTrang, HinhAnh)  
	VALUES (@MaLoaiSP, @TenSP, @DonViTinh, @GiaTien, @GiaNhap, @SoLuong, @LoiNhuan, @KhuyenMai, @TinhTrang, @HinhAnh) 
END 

--Trigger đặt trạng thái và trị giá ban đầu cho hóa đơn trực tiếp --
CREATE TRIGGER TG_HoaDonTrucTiep_TrangThai 
ON HoaDonTrucTiep 
INSTEAD OF INSERT AS  
BEGIN 
--INSERT INTO HoaDonTrucTiep(MaHD, MaNV, MaKH, NgayBanHang, TrangThai, TriGiaHoaDon)  
INSERT INTO HoaDonTrucTiep(MaNV, MaKH, NgayBanHang, TrangThai, TriGiaHoaDon) 
    SELECT  
        --Inserted.MaHD,  
        Inserted.MaNV,  
		Inserted.MaKH, 
		Inserted.NgayBanHang, 
        N'Chưa Thanh Toán',  -- Đặt trạng thái là "Chưa thanh toán" 
        0  -- Đặt trị giá là 0 
    FROM Inserted; 
END; 

--Trigger đặt trạng thái và trị giá ban đầu cho hóa đơn online--
CREATE TRIGGER TG_HoaDonTrucTuyen_TrangThai 
ON HoaDonTrucTuyen 
INSTEAD OF INSERT AS  
BEGIN 
--INSERT INTO HoaDonTrucTuyen(MaHDOnl, MaUD, NgayDat, TrangThai, TriGiaHoaDon)
	INSERT INTO HoaDonTrucTuyen(MaUD, NgayDat, TrangThai, TriGiaHoaDon)
    SELECT  
        --Inserted.MaHDOnl,  
        Inserted.MaUD,  
		Inserted.NgayDat, 
        N'Chưa Thanh Toán',  -- Đặt trạng thái là "Chưa thanh toán" 
        0  -- Đặt trị giá là 0 
    FROM Inserted; 
END; 
--Trigger đặt trạng thái và trị giá ban đầu cho đơn nhập hàng --
CREATE TRIGGER TG_DonNhapHang_TrangThai 
ON DonNhapHang 
INSTEAD OF INSERT AS  
--AFTER INSERT AS  
BEGIN 
--INSERT INTO DonNhapHang(MaDNH, NgayNhap, TrangThai, TriGia) 
INSERT INTO DonNhapHang(NgayNhap, TrangThai, TriGia)
    SELECT  
        --Inserted.MaDNH,    
        Inserted.NgayNhap,  
        N'Chưa thanh toán',  -- Đặt trạng thái là "Chưa thanh toán" 
        0  -- Đặt trị giá là 0 
    FROM Inserted; 
END;


--Trigger thay đổi số lượng sản phẩm còn lại sau khi bán trực tiếp và 
--kiểm tra sản phẩm trong kho còn đủ cho đơn hàng không--
CREATE TRIGGER TG_ThayDoiSLTrucTiep 
ON ChiTietBanTrucTiep 
AFTER INSERT 
AS 
BEGIN 
    DECLARE @SoLuongCu1 INT, @SoLuongCu2 INT;        
    SELECT @SoLuongCu1 = SoLuong 
    FROM SanPham 
    WHERE MaSP IN (SELECT MaSP FROM inserted); 
	SELECT @SoLuongCu2 = inserted.Soluong 
	FROM inserted, SanPham 
	WHERE SanPham.MaSP = inserted.MaSP 
    IF @SoLuongCu1 - @SoLuongCu2 >= 0 
    BEGIN 
        UPDATE SanPham 
        SET SoLuong = @SoLuongCu1 - @SoLuongCu2 
        WHERE SanPham.MaSP IN (SELECT MaSP FROM inserted) 
    END 
    ELSE 
    BEGIN 
        RAISERROR ('Không thể sản phẩm vào thêm đơn hàng vì số lượng hiện tại không đủ', 16, 1) 
    END 
END 
--Trigger thay đổi số lượng sản phẩm còn lại sau khi bán online và 
--kiểm tra sản phẩm trong kho còn đủ cho đơn hàng không 
CREATE TRIGGER TG_ThayDoiSLTrucTuyen 
ON ChiTietBanTrucTuyen 
AFTER INSERT 
AS 
BEGIN 
    DECLARE @SoLuongCu1 INT, @SoLuongCu2 INT;              
    SELECT @SoLuongCu1 = SoLuong 
    FROM SanPham 
    WHERE MaSP IN (SELECT MaSP FROM inserted); 
	SELECT @SoLuongCu2 = inserted.Soluong 
	FROM inserted, SanPham 
	WHERE SanPham.MaSP = inserted.MaSP 
    IF @SoLuongCu1 - @SoLuongCu2 >= 0 
    BEGIN 
        UPDATE SanPham 
        SET SoLuong = @SoLuongCu1 - @SoLuongCu2 
        WHERE SanPham.MaSP IN (SELECT MaSP FROM inserted) 
    END 
    ELSE 
    BEGIN 
        RAISERROR ('Không thể sản phẩm vào thêm đơn hàng vì số lượng hiện tại không đủ', 16, 1) 
    END 
END 
--Trigger thay đổi số lượng sản phẩm sau khi nhập hàng --
CREATE TRIGGER TG_NhapHang 
ON NhapHang 
AFTER INSERT, UPDATE 
AS 
BEGIN 
    UPDATE SanPham 
    SET SoLuong = SoLuong + (SELECT SoLuong FROM inserted WHERE SanPham.MaSP = inserted.MaSP) 
    WHERE SanPham.MaSP IN (SELECT MaSP FROM inserted) 
END 
--Trigger tính tổng tiền chi tiết hóa đơn trực tiếp --
CREATE TRIGGER TongTienBanTrucTiep 
ON ChiTietBanTrucTiep  
INSTEAD OF INSERT AS  
BEGIN 
    DECLARE @MaHD varchar(10), @MaSP varchar(10), @SoLuong int, @GiaTien int, @tongTien int 
SELECT  
	@MaHD = Inserted.MaHD, 
	@MaSP = Inserted.MaSP,  
	@SoLuong = Inserted.Soluong, 
	@GiaTien = SanPham.GiaTien 
	FROM Inserted, SanPham 
	WHERE Inserted.MaSP = SanPham.MaSP 
	SET @tongTien = @SoLuong * @GiaTien 
	INSERT INTO ChiTietBanTrucTiep(MaHD, MaSP, SoLuong, TongTien)  
	VALUES (@MaHD, @MaSP, @SoLuong, @tongTien) 
END 
--Trigger tính tổng tiền chi tiết hóa đơn online --
CREATE TRIGGER TongTienDonHangOnl   
ON ChiTietBanTrucTuyen  
INSTEAD OF INSERT AS  
BEGIN 
    DECLARE @MaHDOnl varchar(10), @MaSP varchar(10), @SoLuong int, @GiaTien int, @tongTien int 
SELECT  
	@MaHDOnl = Inserted.MaHDOnl, 
	@MaSP = Inserted.MaSP,  
	@SoLuong = Inserted.Soluong, 
	@GiaTien = SanPham.GiaTien 
	FROM Inserted, SanPham 
	WHERE Inserted.MaSP = SanPham.MaSP 
	SET @tongTien = @SoLuong * @GiaTien 
	INSERT INTO ChiTietBanTrucTuyen(MaHDOnl, MaSP, SoLuong, TongTien)  
	VALUES (@MaHDOnl, @MaSP, @SoLuong, @tongTien) 
END 
--Trigger tính tổng tiền chi tiết hóa đơn nhập hàng --
CREATE TRIGGER TongTienDonNhap  
ON NhapHang  
INSTEAD OF INSERT AS  
BEGIN 
   DECLARE @MaHDNhap varchar(10), @MaSP varchar(10), @SoLuong int, @GiaTienNhap int, @tongTien int 
SELECT  
	@MaHDNhap = Inserted.MaDNH, 
	@MaSP = Inserted.MaSP,  
	@SoLuong = Inserted.Soluong, 
	@GiaTienNhap = SanPham.GiaNhap
	FROM Inserted, SanPham 
	WHERE Inserted.MaSP = SanPham.MaSP 
	SET @tongTien = @SoLuong * @GiaTienNhap 
	INSERT INTO NhapHang(MaDNH, MaSP, SoLuong, TongTien)  
	VALUES (@MaHDNhap, @MaSP, @SoLuong, @tongTien) 
END 
--Trigger tính trị giá hóa đơn trực tiếp--
CREATE TRIGGER TriGiaHoaDonOff 
ON ChiTietBanTrucTiep
AFTER INSERT AS  
BEGIN 
    DECLARE  
    @MaHD int,
    @TriGiaOld int,  
    @TongTienNew int,  
    @TriGiaNew int 

    -- Lấy MaHDOnl từ dòng đã chèn
    SELECT @MaHD = MaHD FROM inserted

    -- Tính toán TriGiaNew cho MaHDOnl cụ thể
    SELECT
    @TriGiaOld = HDTT.TriGiaHoaDon, 
    @TongTienNew = CTBTT.TongTien 
    FROM  
    HoaDonTrucTiep HDTT,  
    ChiTietBanTrucTiep CTBTT 
    WHERE HDTT.MaHD = CTBTT.MaHD AND HDTT.MaHD = @MaHD

    -- Cập nhật TriGiaHoaDon cho MaHDOnl cụ thể
    SET @TriGiaNew = @TriGiaOld + @TongTienNew 
    UPDATE HoaDonTrucTiep 
    SET TriGiaHoaDon = @TriGiaNew 
    WHERE MaHD = @MaHD
END 
--Trigger tính trị giá hóa đơn online --
CREATE TRIGGER TriGiaHoaDonOnl 
ON ChiTietBanTrucTuyen 
AFTER INSERT AS  
BEGIN 
    DECLARE  
    @MaHD int,
    @TriGiaOld int,  
    @TongTienNew int,  
    @TriGiaNew int 

    -- Lấy MaHDOnl từ dòng đã chèn
    SELECT @MaHD = MaHDOnl FROM inserted

    -- Tính toán TriGiaNew cho MaHDOnl cụ thể
    SELECT
    @TriGiaOld = HDTT.TriGiaHoaDon, 
    @TongTienNew = CTBTT.TongTien 
    FROM  
    HoaDonTrucTuyen HDTT,  
    ChiTietBanTrucTuyen CTBTT 
    WHERE HDTT.MaHDOnl = CTBTT.MaHDOnl AND HDTT.MaHDOnl = @MaHD

    -- Cập nhật TriGiaHoaDon cho MaHDOnl cụ thể
    SET @TriGiaNew = @TriGiaOld + @TongTienNew 
    UPDATE HoaDonTrucTuyen 
    SET TriGiaHoaDon = @TriGiaNew 
    WHERE MaHDOnl = @MaHD
END 
--Trigger tính trị giá hóa đơn nhập hàng --
CREATE TRIGGER TriGiaHoaDonNhap
ON NhapHang
AFTER INSERT AS  
BEGIN 
    DECLARE  
    @MaHDNhap int,
    @TriGiaOld int,  
    @TongTienNew int,  
    @TriGiaNew int 

    -- Lấy MaHDOnl từ dòng đã chèn
    SELECT @MaHDNhap = MaDNH FROM inserted

    -- Tính toán TriGiaNew cho MaHDOnl cụ thể
    SELECT
    @TriGiaOld = DNH.TriGia, 
    @TongTienNew = NH.TongTien 
    FROM  
    DonNhapHang DNH,  
    NhapHang NH 
    WHERE DNH.MaDNH = NH.MaDNH AND DNH.MaDNH = @MaHDNhap

    -- Cập nhật TriGiaHoaDon cho MaHDOnl cụ thể
    SET @TriGiaNew = @TriGiaOld + @TongTienNew 
    UPDATE DonNhapHang 
    SET TriGia = @TriGiaNew 
    WHERE MaDNH = @MaHDNhap
END 

--Tạo csdl--
--Add LoaiSP--
INSERT INTO LoaiSanPham (TenLoaiSP)
VALUES
(N'Keo dán'),
(N'Cục tẩy'),
(N'Màu vẽ'),
(N'Sổ tay'),
(N'Tập hồ sơ'),
(N'Thước kẻ'),
(N'Tập vở'),
(N'Giấy'),
(N'Bút chì'),
(N'Bút bi');
--Add NSX--
INSERT INTO NhaSanXuat (TenNSX, DiaChi, Sdt) VALUES (N'Nhà sản xuất A', N'1 Hai Bà Trưng', '0999999991')
INSERT INTO NhaSanXuat (TenNSX, DiaChi, Sdt) VALUES (N'Nhà sản xuất B', N'2 Hai Bà Trưng', '0999999992')
INSERT INTO NhaSanXuat (TenNSX, DiaChi, Sdt) VALUES (N'Nhà sản xuất C', N'3 Hai Bà Trưng', '0999999993')
INSERT INTO NhaSanXuat (TenNSX, DiaChi, Sdt) VALUES (N'Nhà sản xuất D', N'4 Hai Bà Trưng', '0999999994')
INSERT INTO NhaSanXuat (TenNSX, DiaChi, Sdt) VALUES (N'Nhà sản xuất E', N'5 Hai Bà Trưng', '0999999995')
--Add SP--
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (1, N'Keo dán 502 A1', N'Chai', 10000, 10, 0, 100,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (1, N'Keo dán 502 B1', N'Chai', 11000, 40, 30, 200, NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (1, N'Keo dán 502 A2', N'Chai', 12000, 40, 0,300, NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (1, N'Keo dán 502 A3', N'Chai', 13000, 40, 0, 400, NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (1, N'Keo dán 502 C1', N'Chai', 14000, 40, 0, 500, NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (2, N'Cục tẩy D1', N'Cái', 3000, 20, 5, 200,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (2, N'Cục tẩy D2', N'Cái', 4000, 20, 5, 190,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (2, N'Cục tẩy D3', N'Cái', 5000,20, 5,180,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (2, N'Cục tẩy B1', N'Cái', 6000,20, 5,170,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (2, N'Cục tẩy B2', N'Cái', 7000,20, 5,160,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (3, N'Màu vẽ A1', N'Hộp', 30000,50,0, 50,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (3, N'Màu vẽ B1', N'Hộp', 31000,50,0, 40,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (3, N'Màu vẽ B2', N'Hộp', 32000,50,0, 30,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (3, N'Màu vẽ C1', N'Hộp', 33000,50,0, 20,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (3, N'Màu vẽ C2', N'Hộp', 34000,50,0, 10,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (4, N'Sổ tay B1', N'Cuốn', 10000,40, 0, 500,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (4, N'Sổ tay B2', N'Cuốn', 11000,40,0, 400,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (4, N'Sổ tay D1', N'Cuốn', 12000,40,0, 300,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (4, N'Sổ tay D2', N'Cuốn', 13000,40,0, 200,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (4, N'Sổ tay D3', N'Cuốn', 14000,40,0, 100,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (5, N'Tập hồ sơ E1', N'Tập', 10000,40,0, 500,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (5, N'Tập hồ sơ E2', N'Tập', 11000,40,0, 400,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (5, N'Tập hồ sơ E3', N'Tập', 12000,40,0, 300,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (5, N'Tập hồ sơ E4', N'Tập', 13000,40,0, 200,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (5, N'Tập hồ sơ E5', N'Tập', 14000,40,0, 100,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (6, N'Thước kẻ C1', N'Cái', 5000,30,0, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (6, N'Thước kẻ C2', N'Cái', 6000,30,0, 4000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (6, N'Thước kẻ C3', N'Cái', 7000,30,0, 3000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (6, N'Thước kẻ E1', N'Cái', 8000,30,0, 2000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (6, N'Thước kẻ E2', N'Cái', 9000,30,0, 1000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (7, N'Tập vở A1', N'Lốc', 50000,30,0, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai, SoLuong, HinhAnh, TinhTrang) VALUES (7, N'Tập vở A2', N'Lốc', 60000,30,0, 4000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (7, N'Tập vở A3', N'Lốc', 70000,30,0, 3000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (7, N'Tập vở A4', N'Lốc', 80000,30,0, 2000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (7, N'Tập vở A5', N'Lốc', 90000,30,0, 1000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (8, N'Giấy A1', N'Lốc', 50000,15,0, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (8, N'Giấy A2', N'Lốc', 60000,15,0, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (8, N'Giấy A3', N'Lốc', 70000,15,0, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (8, N'Giấy A4', N'Lốc', 80000,15,0, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (8, N'Giấy A5', N'Lốc', 90000,15,0, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (9, N'Bút bi D1', N'Hộp', 50000,10,0, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (9, N'Bút bi D2', N'Hộp', 55000,10,0, 4000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (9, N'Bút bi D3', N'Hộp', 56000,10,0, 3000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (9, N'Bút bi D4', N'Hộp', 60000,10,0, 3000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (9, N'Bút bi D5', N'Hộp', 70000,10,0, 2000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (10, N'Bút chì C1', N'Hộp', 50000,30,10, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (10, N'Bút chì C2', N'Hộp', 51000,30,10, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (10, N'Bút chì C3', N'Hộp', 52000,30,10, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (10, N'Bút chì C4', N'Hộp', 53000,30,10, 5000,NULL, N'Còn bán')
INSERT INTO SanPham (MaLoaiSP, TenSP, DonViTinh, GiaNhap, LoiNhuan, KhuyenMai,SoLuong, HinhAnh, TinhTrang) VALUES (10, N'Bút chì C5', N'Hộp', 54000,30,10, 5000,NULL, N'Còn bán')
--Add cung cấp--
INSERT INTO CungCap(MaSP, MaNSX) VALUES (1,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (2,2)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (3,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (4,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (5,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (6,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (7,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (8,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (9,2)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (10,2)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (11,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (12,2)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (13,2)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (14,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (15,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (16,2)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (17,2)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (18,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (19,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (20,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (21,5)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (22,5)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (23,5)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (24,5)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (25,5)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (26,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (27,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (28,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (29,5)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (30,5)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (31,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (32,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (33,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (34,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (35,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (36,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (37,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (38,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (39,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (40,1)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (41,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (42,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (43,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (44,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (45,4)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (46,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (47,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (48,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (49,3)
INSERT INTO CungCap(MaSP, MaNSX) VALUES (50,3)
--Add Khách hàng--
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 1', '0888888881', 100)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 2', '0888888881', 200)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 3', '0888888881', 100)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 4', '0888888881', 500)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 5', '0888888881', 300)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 6', '0888888881', 400)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 7', '0888888881', 700)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 8', '0888888881', 100)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 9', '0888888881', 900)
INSERT INTO KhachHang(Hoten, Sdt, DiemTichLuy) VALUES (N'Khách hàng 10', '0888888881', 1000)
--Add ứng dụng--
INSERT INTO UngDung(TenUD, ChietKhau) VALUES (N'Ứng dụng 1', 0.1)
INSERT INTO UngDung(TenUD, ChietKhau) VALUES (N'Ứng dụng 2', 0.2)
INSERT INTO UngDung(TenUD, ChietKhau) VALUES (N'Ứng dụng 3', 0.11)
INSERT INTO UngDung(TenUD, ChietKhau) VALUES (N'Ứng dụng 4', 0.15)
INSERT INTO UngDung(TenUD, ChietKhau) VALUES (N'Ứng dụng 5', 0.25)
--Add tài khoản--
INSERT INTO TaiKhoan(MatKhau) VALUES ('11111111')
INSERT INTO TaiKhoan(MatKhau) VALUES ('22222222')
INSERT INTO TaiKhoan(MatKhau) VALUES ('33333333')
INSERT INTO TaiKhoan(MatKhau) VALUES ('44444444')
INSERT INTO TaiKhoan(MatKhau) VALUES ('55555555')
INSERT INTO TaiKhoan(MatKhau) VALUES ('66666666')
INSERT INTO TaiKhoan(MatKhau) VALUES ('77777777')
--Add cv--
INSERT INTO CongViec(TenCV, Luong) VALUES (N'Quản lý 1', 10000000)
INSERT INTO CongViec(TenCV, Luong) VALUES (N'Nhân viên Full-Time 2', 6000000)
INSERT INTO CongViec(TenCV, Luong) VALUES (N'Bảo vệ 3', 5000000)
INSERT INTO CongViec(TenCV, Luong) VALUES (N'Nhân viên Part-Time 4', 25000)
--Add nv--
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Quản lý 1', '2003-1-1', '0777777771', N'Nam', N'1 Lê Văn Chí', '2023-1-1', N'Đang làm', 1, 1)
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Nhân viên full-time 2', '2003-1-1', '0777777772', N'Nam', N'2 Lê Văn Chí', '2023-1-1', N'Đang làm', 2, 2)
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Nhân viên full-time 3', '2003-1-1', '0777777773', N'Nữ', N'3 Lê Văn Chí', '2023-1-1', N'Đã nghỉ', 2, 3)
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Bảo vệ 4', '2003-1-1', '0777777774', N'Nam', N'4 Lê Văn Chí', '2023-1-1', N'Đang làm', 3, null)
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Bảo vệ 5', '2003-1-1', '0777777775', N'Nam', N'5 Lê Văn Chí', '2023-1-1', N'Đang làm', 3, null)
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Nhân viên part-time 6', '2003-1-1', '0777777776', N'Nam', N'6 Lê Văn Chí', '2023-1-1', N'Đã nghỉ', 4, 4)
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Nhân viên part-time 7', '2003-1-1', '0777777777', N'Nam', N'7 Lê Văn Chí', '2023-1-1', N'Đang làm', 4, 5)
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Nhân viên part-time 8', '2003-1-1', '0777777778', N'Nữ', N'8 Lê Văn Chí', '2023-1-1', N'Đang làm', 4, 6)
INSERT INTO NhanVien(HoTen, NgaySinh, Sdt, GioiTinh, DiaChi, NgayTuyenDung, TrangThai, MaCV, TenTK) VALUES (N'Nhân viên part-time 9', '2003-1-1', '0777777779', N'Nữ', N'9 Lê Văn Chí', '2023-1-1', N'Đang làm', 4, 7)
--Add ca làm--
--Quản lý 2--
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-30', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-31', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-1', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-2', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-3', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-4', N'Fulltime', 7, 15)
--Nhân viên fulltime 3--
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-31', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-1', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-2', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-3', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-4', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-5', N'Fulltime', 15, 23)
--Bảo vệ 5--
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-30', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-31', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-1', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-2', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-3', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-4', N'Fulltime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-5', N'Fulltime', 7, 15)
--Bảo vệ 6--
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-30', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-31', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-1', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-2', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-3', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-4', N'Fulltime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-5', N'Fulltime', 15, 23)
--Nhân viên part-time 8--
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-30', N'Parttime', 7, 11)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-31', N'Parttime', 11, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-1', N'Parttime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-3', N'Parttime', 7, 11)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-5', N'Parttime', 7, 11)
--Nhân viên part-time 9--
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-30', N'Parttime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-10-31', N'Parttime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-1', N'Parttime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-3', N'Parttime', 15, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-5', N'Parttime', 15, 23)
--Nhân viên part-time 10--
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-2', N'Parttime', 7, 19)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-3', N'Parttime', 7, 15)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-4', N'Parttime', 7, 23)
INSERT INTO CaLam(NgayLam, LoaiCa, GioBatDau, GioKetThuc) VALUES ('2023-11-5', N'Parttime', 7, 15)
--Add phân ca--
--Phân ca quản lý 1--
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (2,1,'2023-10-30')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (2,2,'2023-10-31')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (2,3,'2023-11-1')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (2,4,'2023-11-2')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (2,5,'2023-11-3')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (2,6,'2023-11-4')
--Phân ca nv fulltime 3--
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (3,7,'2023-10-31')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (3,8,'2023-11-1')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (3,9,'2023-11-2')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (3,10,'2023-11-3')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (3,11,'2023-11-4')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (3,12,'2023-11-5')
--Phân ca Bảo vệ 5--
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (5,13,'2023-10-30')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (5,14,'2023-10-31')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (5,15,'2023-11-1')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (5,16,'2023-11-2')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (5,17,'2023-11-3')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (5,18,'2023-11-4')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (5,19,'2023-11-5')
--Phân ca Bảo vệ 6--
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (6,20,'2023-10-30')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (6,21,'2023-10-31')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (6,22,'2023-11-1')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (6,23,'2023-11-2')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (6,24,'2023-11-3')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (6,25,'2023-11-4')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (6,26,'2023-11-5')
--Phân ca NV parttime 8--
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (8,27,'2023-10-30')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (8,28,'2023-10-31')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (8,29,'2023-11-1')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (8,30,'2023-11-3')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (8,31,'2023-11-5')
--Phân ca NV parttime 9--
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (9,32,'2023-10-30')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (9,33,'2023-10-31')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (9,34,'2023-11-1')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (9,35,'2023-11-3')
INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (9,36,'2023-11-5')
--Phân ca NV parttime 10--
--INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (10,37,'2023-11-2')
--INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (10,38,'2023-11-3')
--INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (10,39,'2023-11-4')
--INSERT INTO PhanCa(MaNV,MaCa,NgayLam) VALUES (10,40,'2023-11-5')

--Add Hóa đơn trực tuyến--
INSERT INTO HoaDonTrucTuyen(MaUD, NgayDat) VALUES (2,'2023-9-30')
INSERT INTO HoaDonTrucTuyen(MaUD, NgayDat) VALUES (3,'2023-10-1')
INSERT INTO HoaDonTrucTuyen(MaUD, NgayDat) VALUES (1,'2023-10-2')

--Add Chi tiết bán trực tuyến--
INSERT INTO ChiTietBanTrucTuyen(MaHDOnl, MaSP, Soluong) VALUES (6, 2, 1)
INSERT INTO ChiTietBanTrucTuyen(MaHDOnl, MaSP, Soluong) VALUES (5, 3, 10)
INSERT INTO ChiTietBanTrucTuyen(MaHDOnl, MaSP, Soluong) VALUES (4, 2, 1)
INSERT INTO ChiTietBanTrucTuyen(MaHDOnl, MaSP, Soluong) VALUES (2, 15, 11)

--Add Đơn nhập hàng--
INSERT INTO DonNhapHang(NgayNhap) VALUES ('2023-10-29')
INSERT INTO DonNhapHang(NgayNhap) VALUES ('2023-10-30')
INSERT INTO DonNhapHang(NgayNhap) VALUES ('2023-10-31')

--Add Nhập hàng--
INSERT INTO NhapHang(MaDNH, MaSP, Soluong) VALUES (2, 15, 11)
INSERT INTO NhapHang(MaDNH, MaSP, Soluong) VALUES (2, 11, 11)
INSERT INTO NhapHang(MaDNH, MaSP, Soluong) VALUES (2, 1, 11)
INSERT INTO NhapHang(MaDNH, MaSP, Soluong) VALUES (2, 2, 1000)
INSERT INTO NhapHang(MaDNH, MaSP, Soluong) VALUES (3, 3, 1000)
INSERT INTO NhapHang(MaDNH, MaSP, Soluong) VALUES (1, 15, 11)
INSERT INTO NhapHang(MaDNH, MaSP, Soluong) VALUES (3, 15, 11)


--Add Hóa đơn trực tiếp--
INSERT INTO HoaDonTrucTiep(MaNV, MaKH, NgayBanHang) VALUES (3,3,'2023-9-27')
INSERT INTO HoaDonTrucTiep(MaNV, MaKH, NgayBanHang) VALUES (2,5,'2023-9-29')
INSERT INTO HoaDonTrucTiep(MaNV, MaKH, NgayBanHang) VALUES (8,1,'2023-9-30')

--Add Chi tiết trực tiếp--
INSERT INTO ChiTietBanTrucTiep(MaHD, MaSP, Soluong) VALUES (5, 2, 1)
INSERT INTO ChiTietBanTrucTiep(MaHD, MaSP, Soluong) VALUES (5, 3, 2)
INSERT INTO ChiTietBanTrucTiep(MaHD, MaSP, Soluong) VALUES (6, 7, 1)
INSERT INTO ChiTietBanTrucTiep(MaHD, MaSP, Soluong) VALUES (7, 35, 2)

