USE [QLNV]
GO
/****** Object:  Table [dbo].[NHANVIEN]    Script Date: 16/05/2021 10:33:44 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NHANVIEN](
	[MANV] [nchar](20) NOT NULL,
	[HO] [nvarchar](50) NULL,
	[TEN] [nvarchar](50) NULL,
	[PHAI] [nvarchar](50) NULL,
	[DIACHI] [nvarchar](50) NULL,
	[NGAYSINH] [date] NULL,
	[LUONG] [int] NULL,
	[MACN] [nchar](20) NULL,
	[TRANGTHAI] [bit] NULL,
 CONSTRAINT [PK_NHANVIEN] PRIMARY KEY CLUSTERED 
(
	[MANV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Sp_DeleteNV]    Script Date: 16/05/2021 10:33:44 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Sp_DeleteNV](@MANV nchar(50))
AS
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRAN;
	BEGIN TRY
		SET @MANV = UPPER(@MANV);
		IF NOT EXISTS (SELECT * FROM NHANVIEN WHERE MANV = @MANV AND TRANGTHAI = 'False') RAISERROR ('MANV not exist or deleted before!', 16, 1);

		UPDATE NHANVIEN SET	TRANGTHAI = '1'
		WHERE MANV = @MANV;
		COMMIT
	END TRY
	BEGIN CATCH
		
			ROLLBACK;

	DECLARE @Message varchar(MAX) = ERROR_MESSAGE(),
        @Severity int = ERROR_SEVERITY(),
        @State smallint = ERROR_STATE()
 
	RAISERROR (@Message, @Severity, @State)
	END CATCH
GO
/****** Object:  StoredProcedure [dbo].[Sp_ThemNV]    Script Date: 16/05/2021 10:33:44 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Sp_ThemNV](@MANV nchar(20), @HO nvarchar(50), @TEN nvarchar(50), @PHAI nvarchar(50), @DIACHI nvarchar(50),
				@NGAYSINH date, @LUONG int, @MACN nchar(20))
AS
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRAN;
BEGIN TRY
	IF(@MANV = ' ') RAISERROR ('MANV empty!', 16, 1);
	IF EXISTS (SELECT * FROM NHANVIEN WHERE MANV = @MANV) RAISERROR ('MANV exist!', 16, 1);
	INSERT INTO NHANVIEN VALUES(@MANV, @HO, @TEN, @PHAI, @DIACHI, @NGAYSINH, @LUONG, 'TPHCM', 0); 
	COMMIT;
END TRY
BEGIN CATCH
	ROLLBACK;

	DECLARE @Message varchar(MAX) = ERROR_MESSAGE(),
        @Severity int = ERROR_SEVERITY(),
        @State smallint = ERROR_STATE()
 
   RAISERROR (@Message, @Severity, @State)
END CATCH

GO
/****** Object:  StoredProcedure [dbo].[Sp_UpdateNV]    Script Date: 16/05/2021 10:33:44 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[Sp_UpdateNV](@MANV nvarchar(50),@HO nvarchar(50), @TEN nvarchar(50), @PHAI nvarchar(50), @DIACHI nvarchar(50),
				@NGAYSINH date, @LUONG int, @MACN nchar(20))
AS
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRAN;
	BEGIN TRY
	SET @MANV = UPPER(@MANV);
	IF NOT EXISTS (SELECT * FROM NHANVIEN WHERE MANV = @MANV AND TRANGTHAI = 'False') RAISERROR ('MANV not exist or deleted before!', 16, 1);

	UPDATE NHANVIEN 
		SET	HO = @HO, TEN = @TEN, PHAI = @PHAI, DIACHI = @DIACHI, NGAYSINH = @NGAYSINH, LUONG = @LUONG
		WHERE MANV = @MANV;
	COMMIT;
	END TRY
	BEGIN CATCH
			ROLLBACK;

			DECLARE @Message varchar(MAX) = ERROR_MESSAGE(),
				@Severity int = ERROR_SEVERITY(),
				@State smallint = ERROR_STATE()
 
			RAISERROR (@Message, @Severity, @State)
	END CATCH
GO
