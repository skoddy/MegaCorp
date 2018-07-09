drop database if exists megacorp;
create database megacorp;
use megacorp;

create table employees (
	Id int primary key auto_increment,
    FirstName varchar(100),
    LastName varchar(100),
    BDate date
);