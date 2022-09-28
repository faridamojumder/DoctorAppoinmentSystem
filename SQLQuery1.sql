drop database CoreMid
Create database CoreMid
Use CoreMid

create table Specialist
(
	SpecialistID int identity(1,1) primary key,
	SpecialistName Varchar(50) NULL
);

create table Doctor
(
	DoctorID int identity(1,1) primary key,
	DoctorName Varchar(50) NULL,
	ScheduleDate datetime NULL,
	Gender varchar(50) NULL,
	Prescription nvarchar(500) NULL,
	TotalPatient int NULL,
	SpecialistID int
);

