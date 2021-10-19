-- Your SQL script to set up your database should go here

DELETE FROM dbo.project_employee;
DELETE FROM dbo.project;
DELETE FROM dbo.employee;
DELETE FROM dbo.department;

SET IDENTITY_INSERT dbo.department ON

INSERT INTO dbo.department
	(department_id, name)
VALUES
	(1, 'SuperDepartment')

SET IDENTITY_INSERT dbo.department OFF

SET IDENTITY_INSERT dbo.employee ON

INSERT INTO dbo.employee 
	(employee_id, department_id, first_name, last_name, job_title, birth_date, hire_date)
VALUES
	(1, 1, 'Johnson', 'Leo', 'Superhero', '1970-01-05', '2013-01-02')

SET IDENTITY_INSERT dbo.employee OFF

SET IDENTITY_INSERT dbo.project ON

INSERT INTO dbo.project
	(project_id, name, from_date, to_date)
VALUES
	(1, 'SuperKale', '2015-09-09', '2022-07-07')

SET IDENTITY_INSERT dbo.project OFF


INSERT INTO dbo.project_employee 
	(project_id, employee_id) 
VALUES
	(1, 1)
