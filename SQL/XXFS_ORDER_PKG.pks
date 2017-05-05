create or replace PACKAGE XXFS_ORDER_PKG
AS
  /* TODO enter package declarations (types, exceptions, methods etc) here */
  PROCEDURE sp_insert_order(
      ip_quantity     NUMBER,
      ip_customer_id  NUMBER,
      ip_sub_total    VARCHAR2,
      ip_total        VARCHAR2,
      ip_firstname    VARCHAR2,
      ip_lastname     VARCHAR2,
      ip_address      VARCHAR2,
      ip_city         VARCHAR2,
      ip_state        VARCHAR2,
      ip_country      VARCHAR2,
      ip_zipcode      VARCHAR2,
      ip_phone        VARCHAR2,
      ip_email        VARCHAR2,
      ip_payment_type VARCHAR2,
      op_order_no OUT NUMBER,
      op_orders OUT sys_refcursor,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2);
  PROCEDURE sp_insert_order_item(
      ip_prod_id  NUMBER,
      ip_price    NUMBER,
      ip_quantity NUMBER,
      ip_order_no NUMBER,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2);
  PROCEDURE sp_get_order(
      ip_order_no NUMBER,
      /*op_order_num OUT NUMBER,
      op_order_status OUT VARCHAR2,
      op_sub_total OUT NUMBER,
      op_total OUT NUMBER,
      op_created_date OUT DATE,
      op_quantity OUT NUMBER,
      op_firstname OUT VARCHAR2,
      op_lastname OUT VARCHAR2,
      op_address OUT VARCHAR2,
      op_city OUT VARCHAR2,
      op_state OUT VARCHAR2,
      op_country OUT VARCHAR2,
      op_zipcode OUT VARCHAR2,
      op_phone OUT VARCHAR2,
      op_email OUT VARCHAR2,
      op_payment_type OUT VARCHAR2,*/
      op_customer_name OUT VARCHAR2,
      op_email OUT VARCHAR2,
      op_orders OUT sys_refcursor,
      op_order_items OUT sys_refcursor,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2);
END XXFS_ORDER_PKG;