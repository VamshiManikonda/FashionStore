create or replace PACKAGE BODY XXFS_ORDER_PKG
AS
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
      op_message OUT VARCHAR2)
  AS
  BEGIN
    SELECT xxfs_order_seq.nextval INTO op_order_no FROM dual;
    INSERT
    INTO xxfs_order
      (
        ORDER_NO,
        quantity,
        CUSTOMER_ID,
        STATUS,
        sub_total,
        total,
        SHIP_FIRST_NAME,
        SHIP_LAST_NAME,
        SHIP_ADDRESS,
        SHIP_CITY,
        SHIP_STATE,
        SHIP_ZIPCODE,
        SHIP_COUNTRY,
        SHIP_PHONE,
        SHIP_EMAIL,
        LAST_UPDATED,
        CARD_TYPE
      )
      VALUES
      (
        op_order_no,
        ip_quantity,
        ip_customer_id,
        'In process',
        ip_sub_total,
        ip_total,
        ip_firstname,
        ip_lastname,
        ip_address,
        ip_city,
        ip_state,
        ip_zipcode,
        ip_country,
        ip_phone,
        ip_email,
        sysdate,
        ip_payment_type
      );
    COMMIT;
    OPEN op_orders FOR SELECT * FROM xxfs_order WHERE customer_id = ip_customer_id;
  END;
  PROCEDURE sp_insert_order_item
    (
      ip_prod_id  NUMBER,
      ip_price    NUMBER,
      ip_quantity NUMBER,
      ip_order_no NUMBER,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2
    )
  AS
    v_quantity NUMBER := 0;
  BEGIN
    INSERT
    INTO xxfs_order_item VALUES
      (
        xxfs_order_item_seq.nextval,
        ip_prod_id,
        ip_price,
        ip_quantity,
        ip_order_no
      );
    COMMIT;
    SELECT QUANTITY INTO v_quantity FROM xxfs_product WHERE prod_id = ip_prod_id;
    v_quantity := v_quantity - ip_quantity;
    UPDATE xxfs_product SET QUANTITY = v_quantity WHERE prod_id = ip_prod_id;
    COMMIT;
  END;
  PROCEDURE sp_get_order(
      ip_order_no NUMBER,
      op_customer_name OUT VARCHAR2,
      op_email OUT VARCHAR2,
      op_orders OUT sys_refcursor,
      op_order_items OUT sys_refcursor,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2)
  AS
  BEGIN
    /*SELECT ORDER_NO,
    quantity,
    CREATED_DATE STATUS,
    sub_total,
    total,
    SHIP_FIRST_NAME,
    SHIP_LAST_NAME,
    SHIP_ADDRESS,
    SHIP_CITY,
    SHIP_STATE,
    SHIP_ZIPCODE,
    SHIP_COUNTRY,
    SHIP_PHONE,
    SHIP_EMAIL,
    CARD_TYPE
    INTO op_order_num,
    op_quantity,
    op_created_date,
    op_order_status,
    op_sub_total,
    op_total,
    op_firstname,
    op_lastname,
    op_address,
    op_city,
    op_state,
    op_zipcode,
    op_country,
    op_phone,
    op_email,
    op_payment_type
    FROM xxfs_order
    WHERE order_no                                                       = ip_order_no;*/
    select first_name || ' ' || last_name, email into op_customer_name, op_email from xxfs_customer where customer_id in (select customer_id from xxfs_order WHERE order_no = ip_order_no);
    OPEN op_orders FOR SELECT      * FROM xxfs_order WHERE order_no      = ip_order_no;
    OPEN op_order_items FOR SELECT * FROM xxfs_order_item WHERE order_no = ip_order_no;
  END;
END XXFS_ORDER_PKG;