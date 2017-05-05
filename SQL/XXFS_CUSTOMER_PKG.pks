create or replace PACKAGE XXFS_CUSTOMER_PKG
AS
  /* TODO enter package declarations (types, exceptions, methods etc) here */
  PROCEDURE sp_register(
      ip_username   VARCHAR2,
      ip_password   VARCHAR2,
      ip_firstname  VARCHAR2,
      ip_lastname   VARCHAR2,
      ip_address    VARCHAR2,
      ip_city       VARCHAR2,
      ip_state      VARCHAR2,
      ip_country    VARCHAR2,
      ip_zipcode    VARCHAR2,
      ip_phone      VARCHAR2,
      ip_email      VARCHAR2,
      ip_admin_user VARCHAR2,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2);
  PROCEDURE sp_login(
      ip_username VARCHAR2,
      ip_password VARCHAR2,
      op_id OUT NUMBER,
      op_firstname OUT VARCHAR2,
      op_lastname OUT VARCHAR2,
      op_address OUT VARCHAR2,
      op_city OUT VARCHAR2,
      op_state OUT VARCHAR2,
      op_country OUT VARCHAR2,
      op_zipcode OUT VARCHAR2,
      op_phone OUT VARCHAR2,
      op_email OUT VARCHAR2,
      op_admin_user OUT NUMBER,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2);
  PROCEDURE sp_reset(
      ip_username VARCHAR2,
      ip_password VARCHAR2,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2);
  PROCEDURE sp_update(
      ip_id NUMBER,
      ip_firstname  VARCHAR2,
      ip_lastname   VARCHAR2,
      ip_address    VARCHAR2,
      ip_city       VARCHAR2,
      ip_state      VARCHAR2,
      ip_country    VARCHAR2,
      ip_zipcode    VARCHAR2,
      ip_phone      VARCHAR2,
      ip_email      VARCHAR2,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2);    
END XXFS_CUSTOMER_PKG;