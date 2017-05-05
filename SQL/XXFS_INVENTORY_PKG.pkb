create or replace PACKAGE BODY XXFS_INVENTORY_PKG
AS
  PROCEDURE sp_women_products(
      op_products OUT sys_refcursor)
  AS
  BEGIN
    -- TODO: Implementation required for PROCEDURE XXFS_INVENTORY_PKG.sp_women_products
    OPEN op_products FOR SELECT * FROM XXFS_PRODUCT WHERE type = 'W' AND active = 1;
END sp_women_products;
  PROCEDURE sp_men_products(
      op_products OUT sys_refcursor)
  AS
  BEGIN
    -- TODO: Implementation required for PROCEDURE XXFS_INVENTORY_PKG.sp_women_products
    OPEN op_products FOR SELECT * FROM XXFS_PRODUCT WHERE type = 'M' AND active = 1;
END sp_men_products;
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
      op_message     VARCHAR2)
  AS
  BEGIN
    IF NVL(ip_prod_id, 0) = 0 THEN
      INSERT
      INTO xxfs_product
        (
          prod_id,
          type,
          Product_Name,
          Description,
          Product_Image,
          quantity,
          Price,
          Start_date,
          End_date,
          Active
        )
        VALUES
        (
          xxfs_prod_id_seq.NEXTVAL,
          ip_type,
          ip_prod_name,
          ip_description,
          ip_image,
          ip_quantity,
          ip_price,
          sysdate,
          NULL,
          ip_active
        );
    ELSE
      UPDATE xxfs_product
      SET type        = ip_type,
        Product_Name  = ip_prod_name,
        Description   = ip_description,
        Product_Image = ip_image,
        quantity      = ip_quantity,
        Price         = ip_price,
        active        = ip_active
      WHERE prod_id   = ip_prod_id;
    END IF;
    COMMIT;
  END sp_insert_products;
END XXFS_INVENTORY_PKG;