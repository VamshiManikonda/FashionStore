create or replace PACKAGE BODY XXFS_CUSTOMER_PKG
AS
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
      op_message OUT VARCHAR2)
  AS
    v_user_count  NUMBER(9) := 0;
    v_email_count NUMBER(9) := 0;
    v_customer_id NUMBER;
  BEGIN
    SELECT COUNT(*)
    INTO v_user_count
    FROM xxfs_customer
    WHERE upper(USERNAME) = upper(ip_username);
    IF v_user_count       > 0 THEN
      op_status          := 1;
      op_message         := 'Username: ' || ip_username || ' is already registered.';
      RETURN;
    END IF;
    SELECT COUNT(*)
    INTO v_email_count
    FROM xxfs_customer
    WHERE upper(email) = upper(ip_email);
    IF v_email_count   > 0 THEN
      op_status       := 1;
      op_message      := 'Email: ' || ip_email || ' is already registered.';
      RETURN;
    END IF;
    SELECT xxfs_customer_seq.nextval INTO v_customer_id FROM dual;
    INSERT
    INTO xxfs_customer
      (
        customer_id,
        First_Name,
        Last_Name,
        Address,
        City,
        State,
        Zip_Code,
        Country,
        Phone,
        Fax,
        Email,
        UserName,
        Password,
        admin_user
      )
      VALUES
      (
        v_customer_id,
        ip_firstname,
        ip_lastname,
        ip_address,
        ip_city,
        ip_state,
        ip_zipcode,
        ip_country,
        ip_phone,
        NULL,
        ip_email,
        ip_username,
        ip_password,
        DECODE(ip_admin_user, 'Y', 1, 0)
      );
    COMMIT;
  END sp_register;
  PROCEDURE sp_login
    (
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
      op_message OUT VARCHAR2
    )
  AS
    v_user_count NUMBER
    (
      9
    )
    := 0;
  BEGIN
    SELECT COUNT(*)
    INTO v_user_count
    FROM xxfs_customer
    WHERE upper(USERNAME) = upper(ip_username)
    AND upper(password)   = upper(ip_password);
    IF v_user_count       > 0 THEN
      SELECT customer_id,
        First_Name,
        Last_Name,
        Address,
        City,
        State,
        Zip_Code,
        Country,
        Phone,
        Email,
        admin_user
      INTO op_id,
        op_firstname,
        op_lastname,
        op_address,
        op_city,
        op_state,
        op_zipcode,
        op_country,
        op_phone,
        op_email,
        op_admin_user
      FROM xxfs_customer
      WHERE upper(USERNAME) = upper(ip_username)
      AND upper(password)   = upper(ip_password);
    ELSE
      op_status  := 1;
      op_message := 'Invalid User/Password.';
      RETURN;
    END IF;
  END sp_login;
  PROCEDURE sp_reset(
      ip_username VARCHAR2,
      ip_password VARCHAR2,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2)
  IS
    v_user_count NUMBER(9) := 0;
  BEGIN
    SELECT COUNT(*)
    INTO v_user_count
    FROM xxfs_customer
    WHERE upper(USERNAME) = upper(ip_username);
    IF v_user_count       = 0 THEN
      op_status          := 1;
      op_message         := 'Username: ' || ip_username || ' is not registered.';
      RETURN;
    END IF;
    UPDATE xxfs_customer
    SET password          = ip_password
    WHERE upper(USERNAME) = upper(ip_username);
    COMMIT;
  END;
  PROCEDURE sp_update(
      ip_id        NUMBER,
      ip_firstname VARCHAR2,
      ip_lastname  VARCHAR2,
      ip_address   VARCHAR2,
      ip_city      VARCHAR2,
      ip_state     VARCHAR2,
      ip_country   VARCHAR2,
      ip_zipcode   VARCHAR2,
      ip_phone     VARCHAR2,
      ip_email     VARCHAR2,
      op_status OUT NUMBER,
      op_message OUT VARCHAR2)
  AS
  BEGIN
    UPDATE xxfs_customer
    SET First_Name    = ip_firstname,
      Last_Name       = ip_lastname,
      Address         = ip_address,
      City            = ip_city,
      State           = ip_state,
      Zip_Code        = ip_zipcode,
      Country         = ip_country,
      Phone           = ip_phone,
      Email           = ip_email
    WHERE customer_id = ip_id;
    COMMIT;
  END;
END XXFS_CUSTOMER_PKG;