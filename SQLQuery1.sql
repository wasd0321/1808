create database Month11Week04db
use Month11Week04db
create table RuKuTable
(
    Rid         int primary key identity,
	YeWu        varchar(50),
	CaiGoHao    varchar(50),
	Typeid      int,
	BuMen        varchar(50),
	CengBenType varchar(50),
	HeTongHao   varchar(50),
	RuKuHao     varchar(50),
	ShuiLv      varchar(50),
	ZhiDanUser  varchar(50),
	HeTongName  varchar(50),
	CangKuId    int,
	RuKuDate    datetime,
	ChanPinUser varchar(50),
	RuKuMoney   varchar(50),
	JiaCheng    VARCHAR(50),
	ZhuangTai   varchar(50),
	BeiZhu      varchar(100)
)
select * from CangKu join RuKuTable on RuKuTable.CangKuId=CangKu.Cid join DanJuType on RuKuTable.Typeid=DanJuType.Did


insert into RuKuTable values('A��˾','TS20210909-001',1,'�ɹ���','ֽ��','A��ͬ���','R-xnik20060202','1.05','AԱ��','A��ͬ����',1,'2021-04-12','����','144','2','�����','�ǺǺ�')
insert into RuKuTable values('A��˾','TS20210909-002',2,'�ɹ���','ֽ��','A��ͬ���','R-xnik20060203','1.05','AԱ��','A��ͬ����',2,'2021-04-12','����','144','2','�����','�ǺǺ�')
insert into RuKuTable values('A��˾','TS20210909-003',1,'�ɹ���','ֽ��','A��ͬ���','R-xnik20060204','1.05','AԱ��','A��ͬ����',3,'2021-04-12','����','144','2','���ǩ��','�ǺǺ�')
insert into RuKuTable values('A��˾','TS20210909-004',1,'�ɹ���','ֽ��','A��ͬ���','R-xnik20060205','1.05','AԱ��','A��ͬ����',2,'2021-04-12','����','144','2','�����','�ǺǺ�')
insert into RuKuTable values('A��˾','TS20210909-005',2,'�ɹ���','ֽ��','A��ͬ���','R-xnik20060206','1.05','AԱ��','A��ͬ����',3,'2021-04-12','����','144','2','���ǩ��','�ǺǺ�')
insert into RuKuTable values('C��˾','TS20210909-006',1,'�ɹ���','ֽ��','A��ͬ���','R-xnik20060207','1.05','AԱ��','A��ͬ����',2,'2021-04-12','����','144','2','�����','�ǺǺ�')
insert into RuKuTable values('B��˾','TS20210909-007',1,'�ɹ���','ֽ��','A��ͬ���','R-xnik20060208','1.05','AԱ��','A��ͬ����',1,'2021-04-12','����','144','2','���ǩ��','�ǺǺ�')
create table RuKuMingXiTable
(
     Mid         int primary key identity,
	 Ruid        int,
	 MingName    varchar(50),
	 MingBian    varchar(50),
	 MingXian    varchar(50),
     MingCailIa    varchar(50),
	 Num         int,
	 HanPrice    int,
	 BuPrice     int,
	 HanCount    int,
	 BuCount     int,
	 JianBiLi    int,
	 JiaPrice    money,
	 BuJiaPrice  money   
)
insert into RuKuMingXiTable values(1,'����·����','NE20E','3435321234','ֽ��',12,2,1,24,12,1,0.1,0.05)
insert into RuKuMingXiTable values(1,'����·����','NE21E','3435321234','����',12,2,1,24,12,1,0.1,0.05)
insert into RuKuMingXiTable values(1,'����·����','NE22E','3435321234','ֽ��',12,2,1,24,12,1,0.1,0.05)
insert into RuKuMingXiTable values(1,'����·����','NE23E','3435321234','����',12,2,1,24,12,1,0.1,0.05)
insert into RuKuMingXiTable values(1,'����·����','NE24E','3435321234','ֽ��',12,2,1,24,12,1,0.1,0.05)
insert into RuKuMingXiTable values(1,'����·����','NE25E','3435321234','����',12,2,1,24,12,1,0.1,0.05)
select * from RuKuMingXiTable
create table CangKu
(
    Cid int primary key identity,
	Cname varchar(50),
)
insert into CangKu values('A�ֿ�'),('B�ֿ�'),('C�ֿ�')
create table DanJuType
(
    Did int primary key identity,
	Dname varchar(50),
)
insert into DanJuType values('�ɹ����'),('�˻����')
