CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

--Food
--Table
--FoodCategory
--Account
--Bill
--BillInfo

CREATE TABLE TableFood
(
	id		INT IDENTITY PRIMARY KEY,
	name	NVARCHAR(50)	NOT NULL	DEFAULT N'Bàn chưa có tên',
	status	NVARCHAR(50)	NOT NULL	DEFAULT N'Bàn trống'	--trống || có người
)
GO

CREATE TABLE Account
(
	DisplayName	NVARCHAR(50) NOT NULL	DEFAULT N'User',
	UserName	NVARCHAR(50)			PRIMARY KEY,
	PassWord	NVARCHAR(50) NOT NULL	DEFAULT 0,
	Type		INT	NOT NULL			DEFAULT 0	--1: admin && 0: staff
)
GO

CREATE TABLE FoodCategory
(
	id		INT IDENTITY PRIMARY KEY,
	name	NVARCHAR(50)	NOT NULL	DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Food
(
	id			INT IDENTITY PRIMARY KEY,
	name		NVARCHAR(50)	NOT NULL	DEFAULT N'Chưa đặt tên',
	idCategory	INT				NOT NULL,
	Price		FLOAT			NOT NULL	DEFAULT 0

	FOREIGN KEY (idCategory)	REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	id				INT IDENTITY PRIMARY KEY,
	DateCheckIn		DATE	NOT NULL	DEFAULT GETDATE(),
	DateCheckOut	DATE,	
	idTable			INT		NOT NULL,
	status			INT		NOT NULL	DEFAULT 0	-- 1: THANH TOÁN && 0: CHƯA THANH TOÁN

	FOREIGN KEY (idTable)	REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo
(
	id		INT IDENTITY PRIMARY KEY,
	idBill	INT	NOT NULL,
	idFood	INT NOT NULL,
	count	INT NOT NULL	DEFAULT 0

	FOREIGN KEY (idBill)	REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood)	REFERENCES dbo.Food(id)
)
GO

-------------------------------------------------------------------------------------------------------------------------
INSERT INTO dbo.Account
(
	UserName, 
	DisplayName,
	PassWord,  
	Type
)
VALUES
(
	N'admin', -- UserName - nvarchar(50)
	N'Master', -- DisplayName - nvarchar(50)
	N'1',	-- PassWord - nvarchar(50)
	1 -- Type - int
)

INSERT INTO dbo.Account
(
	UserName, 
	DisplayName,
	PassWord,  
	Type
)
VALUES
(
	N'staff', -- UserName - nvarchar(50)
	N'staff', -- DisplayName - nvarchar(50)
	N'1',	-- PassWord - nvarchar(50)
	0 -- Type - int
)
GO

CREATE PROC USP_GetAccountByUserName
@userName nvarchar(50)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO

EXEC dbo.USP_GetAccountByUserName @userName = N'admin' -- nvarchar(50)
GO

CREATE PROC USP_Login
@userName nvarchar(50), @passWord nvarchar(50)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
END
GO

--thêm bàn
DECLARE @i INT = 0
WHILE @i <= 20
BEGIN
		INSERT INTO dbo.tableFood (name)	VALUES	(N'Bàn ' + CAST(@i AS nvarchar(50)))
		SET @i = @i + 1
END
GO

CREATE PROC USP_GetTableList
AS SELECT * FROM dbo.tableFood
GO

UPDATE dbo.tableFood SET status = N'Có người' WHERE id = 8
EXEC dbo.USP_GetTableList

--thêm category
INSERT dbo.FoodCategory(name)	VALUES (N'Chocolate')
INSERT dbo.FoodCategory(name)	VALUES (N'Beakfast')
INSERT dbo.FoodCategory(name)	VALUES (N'Cocktails')
INSERT dbo.FoodCategory(name)	VALUES (N'Coffee')
INSERT dbo.FoodCategory(name)	VALUES (N'Drinks')

--thêm food
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Hot Chocolate', 1, 40000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Chocolate Ice Blender', 1, 50000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Chocolate Coconut', 1, 50000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Bún bò', 2, 50000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Phở bò', 2, 50000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Cơm chiên gà rán', 2, 35000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Tequila Sunrise', 3, 77000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Bloody Mary', 3, 77000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Blue Hawaii', 3, 77000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Black Coffee', 4, 20000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Milk Coffee', 4, 25000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Sinh tố bơ', 5, 30000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Sinh tố dâu', 5, 30000)
INSERT dbo.Food (name, idCategory, Price)	VALUES (N'Sữa tươi', 5, 30000)

--thêm bill
INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status)	VALUES (GETDATE(), NULL, 1, 0)
INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status)	VALUES (GETDATE(), NULL, 2, 0)
INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status)	VALUES (GETDATE(), GETDATE(), 2, 1)

--thêm bill info
INSERT dbo.BillInfo (idBill, idFood, count)	VALUES (1, 1, 2)
INSERT dbo.BillInfo (idBill, idFood, count)	VALUES (1, 3, 4)
INSERT dbo.BillInfo (idBill, idFood, count)	VALUES (1, 5, 1)
INSERT dbo.BillInfo (idBill, idFood, count)	VALUES (2, 1, 2)
INSERT dbo.BillInfo (idBill, idFood, count)	VALUES (2, 6, 2)
INSERT dbo.BillInfo (idBill, idFood, count)	VALUES (3, 5, 2)

---------------------------------------------------------------------------------------
select * from dbo.BillInfo where idBill = 3

select f.name, bi.count, f.price, f.price*bi.count as totalPrice from dbo.BillInfo as bi, dbo.Bill as b, dbo.Food as f
where bi.idBill = b.id and bi.idFood = f.id and b.status = 0 and b.idTable = 5

GO
---------------------------------------------------------------------------------------
ALTER TABLE dbo.Bill ADD discount INT

UPDATE dbo.Bill SET discount = 0

CREATE PROC USP_InsertBill
@idTable INT
AS
BEGIN
	INSERT dbo.Bill (DateCheckIn, 
					DateCheckOut, 
					idTable, 
					status,
					discount
					)
	VALUES (GETDATE(), --DateCheckIn - date
			NULL,		--DateCheckOut - date
			@idTable,	--idTable - int
			0,		-- status - int
			0
			)
END
GO
----------------------------------------------------------------------------------
CREATE PROC USP_InsertBillInfo
@idBill INT, @idFood INT, @count INT
AS
BEGIN
	DECLARE @isExitsBillInfo INT
	DECLARE @foodCount INT = 1

	SELECT @isExitsBillInfo = id, @foodCount = b.count 
	FROM dbo.BillInfo AS b
	WHERE idBill = @idBill	AND idFood = @idFood

	IF (@isExitsBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF(@newCount > 0)
			UPDATE dbo.BillInfo SET count = @foodCount + @count WHERE idFood = @idFood
		ELSE
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE
	BEGIN
		INSERT dbo.BillInfo
		(idBill, idFood, count)	
	VALUES (@idBill, @idFood, @count)
	END
END
GO
-------------------------------------------------------------------------------------
CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
		DECLARE @idBill INT
		SELECT @idBill = idBill FROM inserted

		DECLARE @idTable INT
		SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status = 0

		DECLARE @count INT
		SELECT @count = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idBill
		IF(@count > 0)
		BEGIN
			PRINT @idTable
			PRINT @idBill
			PRINT @count
			UPDATE dbo.TableFood SET status = N'Có người' WHERE id = @idTable
		END
		ELSE
		BEGIN
		PRINT @idTable
			PRINT @idBill
			PRINT @count
			UPDATE dbo.TableFood SET status = N'Bàn Trống' WHERE id = @idTable
		END
END
GO
---------------------------------------------------------------------
CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
		DECLARE @idBill INT
		SELECT @idBill = id FROM inserted

		DECLARE @idTable INT
		SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill

		DECLARE @count INT = 0
		SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable = @idTable AND status = 0

		IF(@count = 0)
			UPDATE dbo.TableFood SET status = N'Bàn Trống' WHERE id = @idTable
END
GO
------------------------------------------------------------------
CREATE PROC USP_SwitchTable
@idTable1 INT, @idTable2 INT
AS
BEGIN
	DECLARE @idFirstBill INT
	DECLARE @idSecondBill INT

	DECLARE @isFisrtTableEmty INT = 0
	DECLARE @isSecondTableEmty INT = 0

	SELECT @idSecondBill = id FROM dbo.Bill WHERE idTable = @idTable2 and status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 and status = 0

	IF(@idFirstBill IS NULL)
	BEGIN
		INSERT dbo.Bill (	DateCheckIn, 
							DateCheckOut, 
							idTable, 
							status				
						)
		VALUES (	GETDATE(), --DateCheckIn - date
				NULL,		--DateCheckOut - date
				@idTable1,	--idTable - int
				0		-- status - int			
				)
			SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
	END
	SELECT @isFisrtTableEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idFirstBill

	IF(@idSecondBill IS NULL)
	BEGIN
		INSERT dbo.Bill (	DateCheckIn, 
							DateCheckOut, 
							idTable, 
							status				
						)
		VALUES (	GETDATE(), --DateCheckIn - date
				NULL,		--DateCheckOut - date
				@idTable2,	--idTable - int
				0		-- status - int			
				)
			SELECT @idSecondBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	END
	SELECT @isSecondTableEmty = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idSecondBill

	SELECT id INTO IDBillIndoTable FROM dbo.BillInfo WHERE idBill = @idSecondBill
	UPDATE dbo.BillInfo SET idBill = @idSecondBill WHERE idBill = @idFirstBill
	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillIndoTable)
	DROP TABLE IDBillIndoTable

	IF(@isFisrtTableEmty = 0)
		UPDATE dbo.TableFood SET status = N'Bàn Trống' WHERE id = @idTable2

	IF(@isSecondTableEmty = 0)
		UPDATE dbo.TableFood SET status = N'Bàn Trống' WHERE id = @idTable1
END
GO

EXEC dbo.USP_SwitchTable @idTable1 = 1,
						@idTable2 = 5
									
------------------------------------------------------------------
CREATE PROC USP_UpdateAccount
@userName NVARCHAR(50), @displayName NVARCHAR(50), @password NVARCHAR(50), @newPass NVARCHAR(50)
AS
BEGIN
	DECLARE @isRightPass INT = 0
	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE UserName = @userName AND PassWord = @password
	IF(@isRightPass = 1)
	BEGIN
		IF(@newPass = NULL OR @newPass = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END
		ELSE
			UPDATE dbo.Account SET DisplayName = @displayName, PassWord = @newPass WHERE UserName = @userName
	END
END
GO

-----------------------------------------------------------------------
ALTER TABLE Bill ADD totalPrice FLOAT

CREATE PROC USP_GetListBillByDate
@CheckIn date, @CheckOut date
AS
BEGIN
	SELECT t.name, DateCheckIn, DateCheckOut, discount, b.totalPrice
	FROM Bill AS b, TableFood AS t
	WHERE DateCheckIn >= @CheckIn AND DateCheckOut <= @CheckOut AND b.status = 1
	AND t.id = b.idTable 
END
GO
---------------------------------------------------------------------
CREATE TRIGGER UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
AS
BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = id, @idBill = deleted.idBill FROM deleted

	DECLARE @idTable INT
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill

	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM dbo.BillInfo AS bi, dbo.Bill AS b WHERE b.id = bi.idBill AND b.id = @idBill AND b.status = 0

	IF(@count = 0)
		UPDATE dbo.TableFood SET status = N'Bàn Trống' WHERE id = @idTable
END
GO

-----------------------------------------------------------------
CREATE FUNCTION [dbo].[non_unicode_convert](@inputVar NVARCHAR(MAX) )
RETURNS NVARCHAR(MAX)
AS
BEGIN    
    IF (@inputVar IS NULL OR @inputVar = '')  RETURN ''
   
    DECLARE @RT NVARCHAR(MAX)
    DECLARE @SIGN_CHARS NCHAR(256)
    DECLARE @UNSIGN_CHARS NCHAR (256)
 
    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' + NCHAR(272) + NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
 
    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
   
    SET @COUNTER = 1
    WHILE (@COUNTER <= LEN(@inputVar))
    BEGIN  
        SET @COUNTER1 = 1
        WHILE (@COUNTER1 <= LEN(@SIGN_CHARS) + 1)
        BEGIN
            IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@inputVar,@COUNTER ,1))
            BEGIN          
                IF @COUNTER = 1
                    SET @inputVar = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@inputVar, @COUNTER+1,LEN(@inputVar)-1)      
                ELSE
                    SET @inputVar = SUBSTRING(@inputVar, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@inputVar, @COUNTER+1,LEN(@inputVar)- @COUNTER)
                BREAK
            END
            SET @COUNTER1 = @COUNTER1 +1
        END
        SET @COUNTER = @COUNTER +1
    END
    -- SET @inputVar = replace(@inputVar,' ','-')
    RETURN @inputVar
END
GO

-------------------------------------------------------------------

