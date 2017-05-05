---------------------------------
---------CUSTOMER TABLE----------
---------------------------------
CREATE TABLE xxfs_customer (
  customer_id number(10),
  First_Name varchar2(50) NOT NULL,
  Last_Name varchar2(50) NOT NULL,
  Address varchar2(500),
  City varchar2(50),
  State varchar2(50),
  Zip_Code varchar2(15),
  Country varchar2(50),
  Phone varchar2(50),
  Fax varchar2(50),
  Email varchar2(100) NOT NULL,
  UserName varchar2(100) NOT NULL,
  Password varchar2(100) NOT NULL,
  Cookie number(10) DEFAULT 0,
  created_date date DEFAULT SYSDATE,  
  admin_user number(10) DEFAULT 0,
  CONSTRAINT customer_id_pk PRIMARY KEY(customer_id),
  CONSTRAINT username_uk UNIQUE(username),
  CONSTRAINT email_uk UNIQUE(email));

create sequence xxfs_customer_seq
  start with 2;

/*Admin user*/  
insert into xxfs_customer(customer_id,First_Name,Last_Name,Address,City,State,Zip_Code,Country,Phone,Fax,Email,UserName,Password,admin_user)
    values (1, 'Bhavya', 'Evandu', '701 Don Mills Road', 'Toronto',
            'ON','M3C 1R7', 'Canada', '+16478297770', NULL, 'bhavyaevandu@gmail.com', 'bhavyaevandu',
            'sweetie', 1); 
commit;


----------------------------------
---------INVENTORY TABLE----------
----------------------------------
CREATE TABLE xxfs_product (
	prod_id number(9),
	Product_Name varchar2(50) not null,
	Description varchar2(2000) not null,
	Product_Image varchar2(200) not null,
  quantity number(9) not null,
	Price number(6,2) not null,
	Start_date date,
	End_date date,
	Active number(1),
  Type char(1) not null,
	 CONSTRAINT prod_id_pk PRIMARY KEY(prod_id));

CREATE SEQUENCE xxfs_prod_id_seq start with 1;


----------------------------------
---------ORDER TABLE--------------
----------------------------------		
CREATE TABLE xxfs_order (
	order_no number(5),
	Quantity number(2) not null,
	CUSTOMER_ID NUMBER(10,0) not null,
	status varchar2(100) not null,
	Sub_Total number(7,2) not null,
	Total number(7,2) not null,
	Shipping number(5,2),
	Tax number(5,2),
	created_date date DEFAULT SYSDATE,
	Ship_First_Name varchar2(50) not null,
	Ship_Last_Name varchar2(50) not null,
	Ship_Address varchar2(500) not null,
	Ship_City varchar2(50) not null,
	Ship_State varchar2(50) not null,
	Ship_ZipCode varchar2(15) not null,
    Ship_country varchar2(50) not null,
	Ship_Phone varchar2(50) not null,
	Ship_Fax varchar2(50),
	Ship_Email varchar2(100) not null,
	last_updated date,
	Card_Type varchar2(100) not null,
	Card_Number varchar2(100),
	Exp_Month char(2),
    Exp_Year char(4),
	Card_Name varchar2(100),
        CONSTRAINT order_no_pk PRIMARY KEY(order_no),
         CONSTRAINT CUSTOMER_ID_fk FOREIGN KEY (CUSTOMER_ID)
           REFERENCES xxfs_customer(CUSTOMER_ID) );

Create sequence xxfs_order_seq
   start with 1;	
   
   
---------------------------------------
---------ORDER ITEM TABLE--------------
---------------------------------------
CREATE TABLE xxfs_order_item (
	order_item_id number(2),
	prod_id number(2),
	Price number(6,2),
	Quantity number(2),
	order_no number(5),
	CONSTRAINT order_item_id_pk PRIMARY KEY (order_item_id),
        CONSTRAINT order_no_fk FOREIGN KEY (order_no) 
          REFERENCES xxfs_order(order_no),
        CONSTRAINT product_id_fk FOREIGN KEY (prod_id) 
          REFERENCES xxfs_product(prod_id) );

Create sequence xxfs_order_item_seq
  start with 1;	  
  
  
/*COMMIT*/
COMMIT;  