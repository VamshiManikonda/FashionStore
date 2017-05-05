create or replace PACKAGE XXFS_INVENTORY_PKG
AS
  /* TODO enter package declarations (types, exceptions, methods etc) here */
  PROCEDURE sp_women_products(
      op_products OUT sys_refcursor);
  PROCEDURE sp_men_products(
      op_products OUT sys_refcursor);
  PROCEDURE sp_insert_products(
      ip_prod_id     NUMBER,
      ip_prod_name   VARCHAR2,
      ip_description VARCHAR2,
      ip_image       VARCHAR2,
      ip_quantity    NUMBER,
      ip_price       NUMBER,
      ip_active      NUMBER,
      ip_type        VARCHAR2,
      op_status      NUMBER,
      op_message     VARCHAR2);
END XXFS_INVENTORY_PKG;