
CREATE TABLE dbo.ProfileMatch(
	ProfileId int identity(1,1) not null,
	UserId varchar(max) not null,
	FirstName varchar(255) not null,
	MiddleName varchar(255) null,
	LastName varchar(255) not null,
	Gender varchar(1) not null,
	Email varchar(500) not null,
	Phone varchar(100) null,
	City varchar(100) null,
	StateName varchar(100) null,
    Country varchar(100) null,
	ZipCode varchar(23) null,
	Education varchar(1000) null,
	Profession varchar(1000) null,
	Interest varchar(1000) null,
	Expectation varchar(1000) null,
	YearOfBirth varchar(1000) null,
	UpdatedDate DateTime2(7) not null
)

INSERT INTO dbo.ProfileMatch VALUES (
	NEWID(),
	'Christy',
	'R' ,
	'Ramanan' ,-- varchar(255) not null,
	'F', -- varchar(1) not null,
	'ravindranraghu@yahoo.com',
	'+1 980 222 1122',
	'Tampa', -- varchar(100) null,
	'Florida',  --varchar(100) null,
    'US', 
	'11228', 
	'Medicine', 
	'Doctor',
	'Love Community Singing' ,
	'No drinking and smoking',
	'2000' ,
	getdate() 
)

CREATE TABLE dbo.ProfileApproved(
ApprovalId int Identity(1,1) not null,
UserId varchar (max) not null,
ApprovedDate DateTime2(7) not null
)

CREATE TABLE dbo.ProfileReview(
	ReviewId int Identity(1,1) not null,
	UserId varchar (max) not null,
	ImpactedUserId varchar (max) not null,
	ImpactedType varchar(1) not null,
	ReviewedDate DateTime2(7) not null
)

