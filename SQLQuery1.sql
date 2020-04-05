
SELECT * FROM dbo.ProfileMatch

SELECT * FROM dbo.ProfileApproved

exec sp_rename 'dbo.ProfileMatch.UserId', 'ProfileUserId'

ALTER TABLE dbo.ProfileMatch alter column UpdatedById varchar(255) not null

ALTER TABLE dbo.ProfileMatch alter column UpdatedByName varchar(255) not null

update dbo.ProfileMatch set UpdatedById ='1' , UpdatedByName = '';

DELETE dbo.ProfileMatch where ProfileUserId = 'f8e8b239-7bd3-4b39-844c-eff00a7c7ce9'


CREATE TABLE dbo.ProfileMatch(
	ProfileId int identity(1,1) not null,
	ProfileUserId varchar(255) not null,
	FirstName varchar(255) not null,
	MiddleName varchar(255) null,
	LastName varchar(255) not null,
	Gender varchar(1) not null,
	Email varchar(255) not null,
	Phone varchar(100) null,
	City varchar(100) null,
	StateName varchar(100) null,
    Country varchar(100) null,
	ZipCode varchar(23) null,
	Education varchar(1000) null,
	Profession varchar(1000) null,
	Interest varchar(1000) null,
	Expectation varchar(1000) null,
	YearOfBirth varchar(255) null,
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

DROP TABLE dbo.ProfileApproved

CREATE TABLE dbo.ProfileApproved(
	ApprovalId int Identity(1,1) not null,
	ProfileUserId varchar (255) not null,	
	ApprovedByName varchar(255) not null,
	ApprovedById varchar(255) not null,
	ApprovedDate DateTime2(7) not null
)

INSERT INTO DBO.ProfileApproved VALUES(
'0908a696-0ce7-43fe-a1a6-1ff163d61867',
'Ravindran Raghu',
'1189D78B-A8AF-4EC7-AC1F-C94307E1E51F',
 getdate()
)

Drop table  dbo.ProfileReview
CREATE TABLE dbo.ProfileReview(
	ReviewId int Identity(1,1) not null,
	ProposedByUserId varchar (max) not null,
	ProposedToUserId varchar (max) not null,
	HasInitiatedDiscussion bit not null,
	DateInitiatedDiscussion datetime2(7) not null,
	HasAcceptedDiscussion bit not null,
	DateAcceptedDiscussion datetime2(7) not null,
	HasMadeProposal bit not null,
	DateMadeProposal datetime2(7) not null,
	HasAcceptedProposal bit not null,
	DateAcceptedProposal datetime2(7) not null
)

select * from dbo.ProfileMatch


insert into dbo.ProfileApproved 
select top 3  UserId, getdate() from  dbo.ProfileMatch 

--DROP TABLE dbo.UserBasic

CREATE TABLE dbo.UserBasic(
	UserIdId int Identity(1,1) not null,
	FirstName varchar (255) not null,
	MiddleName varchar (255) not null,
	LastName varchar (255) not null,
	Email varchar (255) not null,
	UserIdSystem varchar (MAX) not null,
	PdSystem varchar (MAX) not null,
	UpdatedDate datetime2(7) not null
)

drop table dbo.UserRoleMap 

CREATE TABLE dbo.UserRoleMap(
	MapId int Identity(1,1) not null,
	UserIdSystem varchar (255) not null,
	IsAdmin bit not null,
	UpdateByName varchar(255) not null,
	UpdateById varchar(255) not null,
	UpdatedDate datetime2(7) not null
)

SELECT * FROM UserBasic

-- update UserBasic set Email = 'ravindranraghu23@gmail.com' where Email = 'ravindranraghu23@gmaiil.com'


 INSERT INTO dbo.UserRoleMap
 values (
 '659d8089-62fb-4ef4-9964-fc6a729cb283',
 1,
 'Ravindran Raghu',
 '659d8089-62fb-4ef4-9964-fc6a729cb283',
 GETDATE()
 )

